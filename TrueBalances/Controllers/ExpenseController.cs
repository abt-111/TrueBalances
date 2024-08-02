using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;

namespace TrueBalances.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly UserContext _context;

        public ExpenseController(UserContext context)
        {
            _context = context;
        }

        // GET: ExpenseController
        public async Task<ActionResult> Index()
        {
            return View(await _context.Expenses.ToListAsync());
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
            return View();
        }

        // POST: ExpenseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Create(
                [Bind("Id,Title,Date,Username,Amount,Category")]
                Expense
                    expense) // // Reçoit les données du formulaire, crée une nouvelle dépense et la sauvegarde dans la base de données.// 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Expenses.Add(expense);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(expense);
            }
        }

        // GET: ExpenseController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: ExpenseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Title,Date,Author,Amount,Category")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Expenses.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // if ()
                    {
                        
                    }
                    return View();
                }
                
            }
            return RedirectToAction(nameof(Index));
            
        }

        // GET: ExpenseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExpenseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}