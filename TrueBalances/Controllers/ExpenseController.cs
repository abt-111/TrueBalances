using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly Data.UserContext _context;
        private readonly UserManager<CustomUser> _userManager;

        
        public ExpenseController(Data.UserContext context,UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: ExpenseController
        public async Task<ActionResult> Index()
        {
            var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();

            // Récupération de la liste de tous les utilisateurs
            var customUsers = await _userManager.Users.ToListAsync();

            // Utilisation du ViewBag pour récupérer la liste dans la vue Index
            ViewBag.customUsers = customUsers;

            return View(expenses);
        }


        // GET: ExpenseController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category) // Inclure la catégorie
                .Include(e => e.Participants) // Inclure les participants
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }


        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            // Récupérer tous les utilisateurs disponibles
            var allUsers = _context.Users.ToList();
            ViewBag.Users = allUsers;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title,Amount,Date,CategoryId")] Expense expense, string[] SelectedUserIds)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                expense.CustomUserId = user.Id;

                if (SelectedUserIds != null && SelectedUserIds.Length > 0)
                {
                    foreach (var userId in SelectedUserIds)
                    {
                        var selectedUser = await _context.Users.FindAsync(userId);
                        if (selectedUser != null)
                        {
                            expense.Participants.Add(selectedUser);
                        }
                    }
                }

                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            ViewBag.Users = _context.Users.ToList();
            return View(expense);
        }



        // GET: ExpenseController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.Participants) // Inclure les participants
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            // Créer une SelectList avec Prénom + Nom
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            ViewBag.Users = new SelectList(
                _context.Users.Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName }),
                "Id",
                "FullName"
            );

            return View(expense);
        }



        // POST: ExpenseController/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Expense expense, string[] selectedUserIds)
{
    if (id != expense.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            var existingExpense = await _context.Expenses
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingExpense == null)
            {
                return NotFound();
            }

            // Mettre à jour les propriétés de l'expense
            existingExpense.Title = expense.Title;
            existingExpense.Amount = expense.Amount;
            existingExpense.Date = expense.Date;
            existingExpense.CategoryId = expense.CategoryId;
            existingExpense.CustomUserId = expense.CustomUserId;

            // Mettre à jour la liste des participants
            existingExpense.Participants.Clear();
            if (selectedUserIds != null)
            {
                var selectedUsers = await _context.Users
                    .Where(u => selectedUserIds.Contains(u.Id))
                    .ToListAsync();
                foreach (var user in selectedUsers)
                {
                    existingExpense.Participants.Add(user);
                }
            }

            _context.Update(existingExpense);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ExpenseExists(expense.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // Recharger les catégories et les utilisateurs en cas d'échec de validation
    ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
    ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");

    return View(expense);
}


// GET: ExpenseController/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var expense = await _context.Expenses
        .Include(e => e.Category) // Inclure la catégorie
        .FirstOrDefaultAsync(e => e.Id == id);
    
    if (expense == null)
    {
        return NotFound();
    }

    return View(expense);
}


        // POST: ExpenseController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Attendre la tâche asynchrone pour obtenir l'objet Expense
            var expense = await _context.Expenses.FindAsync(id);
            
            // Supprimer l'objet Expense de la base de données
            _context.Expenses.Remove(expense);
            
            // Enregistrer les modifications dans la base de données
            await _context.SaveChangesAsync();
            
                return RedirectToAction(nameof(Index));
            
            
        }
        private bool ExpenseExists(int id) // Vérifie si une dépense avec l'ID spécifié existe dans la base de données.// 
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}