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
                .FirstOrDefaultAsync(e => e.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: ExpenseController/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }


        // POST: ExpenseController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Amount,Date,CategoryId")] Expense expense)
        {
        
            
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                expense = new Expense() {Title = expense.Title, Amount = expense.Amount, Date = expense.Date, CategoryId = expense.CategoryId, CustomUserId = user.Id};
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                

            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage);
                }
            }


            var categories = await _context.Categories.ToListAsync();
            var categoryList = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            // Ajouter l'option "Aucune" au début de la liste
            categoryList.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "Aucune"
            });

            ViewBag.Categories = categoryList;
            //ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        // GET: ExpenseController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
        }


        // POST: ExpenseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Amount,Date,CategoryId, CustomUserId")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }
            
            //expense = new Expense() {Title = expense.Title, Amount = expense.Amount, Date = expense.Date, CategoryId = expense.CategoryId, CustomUserId = user.Id };
                                _context.Update(expense);
                                await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    
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

                return RedirectToAction(nameof(Index));
            }

            // Recharger les catégories en cas d'échec de validation
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
        }


        // GET: ExpenseController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        
        {
            if (id == null )
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null)
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