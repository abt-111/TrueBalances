using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Tools;

namespace TrueBalances.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly UserContext _context;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly UserManager<CustomUser> _userManager;

        public ExpenseController(UserContext context, UserManager<CustomUser> userManager, IGenericRepository<Category> categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        // GET: ExpenseController
        //Methode affichant les récapitulatifs
        public async Task<IActionResult> Index(int groupId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.GroupId == groupId)
                .Include(e => e.Category)
                .Include(e => e.Participants)
                .ToListAsync();

            // Utilisation du ViewBag pour récupérer l'id de l'utilisateur courant dans la vue
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            // Utilisation du ViewBag pour récupérer la liste des utilisateurs dans la vue
            ViewBag.Users = await _userManager.Users.ToListAsync();

            ViewBag.Debts = DebtOperator.GetSomeoneDebts(expenses, ViewBag.Users, ViewBag.CurrentUserId);

            ViewBag.GroupId = groupId;

            return View(expenses);
        }

        public async Task<IActionResult> Solde(int groupId)
        {
            if (groupId <= 0)
            {
                return NotFound(); // Vérifie si l'ID du groupe est valide
            }

            // Récupérer les dépenses associées au groupe spécifié
            var expenses = await _context.Expenses
                .Where(e => e.GroupId == groupId) // Filtrer par ID du groupe
                .Include(e => e.Category)
                .Include(e => e.Participants)
                .ToListAsync();

            // Utiliser ViewBag pour récupérer l'ID de l'utilisateur courant dans la vue
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            // Utiliser ViewBag pour récupérer la liste des utilisateurs dans la vue
            ViewBag.Users = await _userManager.Users.ToListAsync();

            // Calculer les soldes
            ViewBag.DebtsOfEverybody = DebtOperator.GetDebtsOfEverybody(expenses, ViewBag.Users);
            // Passer l'ID du groupe à la vue
            ViewBag.GroupId = groupId;

            return View(expenses);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category) // Inclure la catégorie
                .Include(e => e.Participants) // Inclure les participants
                .Include(e => e.Group) // Inclure le groupe associé
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            // Passer l'ID du groupe à la vue via ViewBag (optionnel, selon vos besoins)
            ViewBag.GroupId = expense.Group?.Id;

            return View(expense);
        }

        public async Task<IActionResult> Create(int groupId)
        {
            var expense = new Expense
            {
                Date = DateTime.Now,
                CustomUserId = _userManager.GetUserId(User),
                GroupId = groupId
            };

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(expense);
        }

        [HttpPost]
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

                // Rediriger vers la page de gestion des dépenses pour le groupe
                return RedirectToAction("Index", new { groupId = expense.GroupId });
            }

            // Recharger les catégories et les utilisateurs en cas d'échec de validation
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
            ViewBag.Users = _context.Users.ToList();
            return View(expense);
        }

        public async Task<IActionResult> Edit(int id, int groupId)
        {
            // Vérification des paramètres
            if (id == 0 || groupId == 0)
            {
                return View("404");
            }

            // Recherche de la dépense par son ID
            var expense = await _context.Expenses
                .Include(e => e.Category)     // Inclure les catégories
                .Include(e => e.Participants) // Inclure les participants
                .FirstOrDefaultAsync(e => e.Id == id);

            // Vérification de l'existence de la dépense
            if (expense == null)
            {
                return View("404");
            }

            // Vérifie si la dépense appartient bien au groupe spécifié
            if (expense.GroupId != groupId)
            {
                return View("404");
            }

            // Vérification si l'utilisateur connecté est bien le propriétaire de la dépense
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Id != expense.CustomUserId)
            {
                return View("404");
            }

            // Préparation des données pour la vue
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
            ViewBag.Users = await _userManager.Users.ToListAsync();

            // Passer le groupId à la vue, si nécessaire pour les formulaires ou autres
            ViewBag.GroupId = groupId;

            // Affichage de la vue de modification de la dépense
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Expense expense)
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

                    // Vérifiez que la dépense appartient toujours au bon groupe
                    if (existingExpense.GroupId != expense.GroupId)
                    {
                        return NotFound(); // Ou gérer l'erreur selon vos besoins
                    }

                    // Mettre à jour les propriétés de l'expense
                    existingExpense.Title = expense.Title;
                    existingExpense.Amount = expense.Amount;
                    existingExpense.Date = expense.Date;
                    existingExpense.CategoryId = expense.CategoryId;

                    // Mettre à jour la liste des participants
                    existingExpense.Participants.Clear();

                    if (expense.SelectedUserIds != null && expense.SelectedUserIds.Count > 0)
                    {
                        expense.Participants = await _context.Users.Where(u => expense.SelectedUserIds.Contains(u.Id)).ToListAsync();
                        existingExpense.Participants = expense.Participants;
                    }

                    _context.Update(existingExpense);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", new { groupId = expense.GroupId });
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

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
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

        public async Task<IActionResult> Alert(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category)  // Inclure les catégories
                .Include(e => e.Participants)  // Inclure les participants
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            // Empêcher l'accès quand la dépenses n'appartient pas à l'utilisateur
            var user = await _userManager.GetUserAsync(User);

            if (user.Id != expense.CustomUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            expense.CategoryId = null;
            _context.Update(expense);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}