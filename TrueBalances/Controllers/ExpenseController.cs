using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Data;
using TrueBalances.Models;

namespace TrueBalances.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly UserContext _context;
        private readonly UserManager<CustomUser> _userManager;

        
        public ExpenseController(UserContext context,UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: ExpenseController
        public async Task<ActionResult> Index()
        {
            var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();
            return View(expenses);
        }


        // GET: ExpenseController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Inclure les informations de la catégorie associée
            var expense = await _context.Expenses
                .Include(e => e.Category)  // Cette ligne inclut les données de la catégorie
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
            
            
            
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
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




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Amount,Date,CategoryId")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            var existingExpense = await _context.Expenses.FindAsync(id);
            if (existingExpense == null)
            {
                return NotFound();
            }

            existingExpense.Title = expense.Title;
            existingExpense.Amount = expense.Amount;
            existingExpense.Date = expense.Date;
            existingExpense.CategoryId = expense.CategoryId;

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    await _context.SaveChangesAsync();
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

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
            
        }




        // GET: ExpenseController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Inclure les informations de la catégorie associée
            var expense = await _context.Expenses
                .Include(e => e.Category)  // Cette ligne inclut les données de la catégorie
                .FirstOrDefaultAsync(m => m.Id == id);
        
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