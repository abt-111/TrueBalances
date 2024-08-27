using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Tools;

namespace TrueBalances.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly UserContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public ExpenseController(UserContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ExpenseController
        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses.Include(e => e.Category).Include(e => e.Participants).ToListAsync();

            // Utilisation du ViewBag pour récupérer l'id de l'utilisateur courant dans la vue
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            // Utilisation du ViewBag pour récupérer la liste des utilisateurs dans la vue
            ViewBag.Users = await _userManager.Users.ToListAsync();

            ViewBag.Debts = DebtOperator.GetSomeoneDebts(expenses, ViewBag.Users, ViewBag.CurrentUserId);

            return View(expenses);
        }

        public async Task<IActionResult> Solde()
        {
            var expenses = await _context.Expenses.Include(e => e.Category).Include(e => e.Participants).ToListAsync();

            // Utilisation du ViewBag pour récupérer l'id de l'utilisateur courant dans la vue
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            // Utilisation du ViewBag pour récupérer la liste des utilisateurs dans la vue
            ViewBag.Users = await _userManager.Users.ToListAsync();

            ViewBag.DebtsOfEverybody = DebtOperator.GetDebtsOfEverybody(expenses, ViewBag.Users);

            return View(expenses);
        }

        // GET: ExpenseController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
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

        public async Task<IActionResult> Create()
        {
            var expense = new Expense
            {
                // Valeur par défaut de la date dans le formulaire
                Date = DateTime.Now,
                // On passe assigne CustomUserId directement dans le contrôleur pour plus de simplicité
                CustomUserId = _userManager.GetUserId(User)
            };

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.Users = await _userManager.Users.ToListAsync(); // Assurez-vous que cela retourne une liste valide
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (expense.SelectedUserIds == null || !expense.SelectedUserIds.Any())
            {
                ModelState.AddModelError(string.Empty, "Veuillez sélectionner au moins un participant.");
            }

            if (ModelState.IsValid)
            {
                if (expense.SelectedUserIds != null && expense.SelectedUserIds.Count > 0)
                {
                    expense.Participants = await _context.Users.Where(u => expense.SelectedUserIds.Contains(u.Id)).ToListAsync();
                }

                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recharger les catégories et les utilisateurs en cas d'échec de validation
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            ViewBag.Users = await _userManager.Users.ToListAsync(); 
            return View(expense);
        }


        // GET: ExpenseController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return View("404");
            }

            var expense = await _context.Expenses
                .Include(e => e.Category)  // Inclure les catégories
                .Include(e => e.Participants)  // Inclure les participants
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return View("404");
            }

            // Empêcher l'accès quand la dépenses n'appartient pas à l'utilisateur
            var user = await _userManager.GetUserAsync(User);

            if (user.Id != expense.CustomUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(expense);
        }

        // POST: ExpenseController/Edit/5
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Expense expense)
{
    if (id != expense.Id)
    {
        return NotFound();
    }

    if (expense.SelectedUserIds == null || !expense.SelectedUserIds.Any())
    {
        ModelState.AddModelError(string.Empty, "Veuillez sélectionner au moins un participant.");
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

            if (expense.SelectedUserIds != null && expense.SelectedUserIds.Count > 0)
            {
                expense.Participants = await _context.Users.Where(u => expense.SelectedUserIds.Contains(u.Id)).ToListAsync();
                existingExpense.Participants = expense.Participants;
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

    ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
    ViewBag.Users = await _userManager.Users.ToListAsync();
    return View(expense);
}


        // GET: ExpenseController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null)
            {
                return NotFound();
            }

            // Empêcher l'accès quand la dépenses n'appartient pas à l'utilisateur
            var user = await _userManager.GetUserAsync(User);

            if (user.Id != expense.CustomUserId)
            {
                return RedirectToAction(nameof(Index));
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