using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Models.ViewModels;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Tools;

namespace TrueBalances.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly TrueBalancesDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;

        public ExpenseController(TrueBalancesDbContext context, UserManager<CustomUser> userManager, ICategoryRepository categoryRepository, IUserService userService)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int groupId)
        {
            var currentUserId = _userManager.GetUserId(User);

            var users = await _userService.GetAllUsersAsync(groupId);

            var expenses = await _context.Expenses
                .Where(e => e.GroupId == groupId)
                .Include(e => e.Category)
                .Include(e => e.Participants)
                .ToListAsync();

            var debts = DebtOperator.GetSomeoneDebts(expenses, users, currentUserId);

            ExpenseIndexViewModel viewModel = new ExpenseIndexViewModel()
            {
                GroupId = groupId,
                CurrentUserId = currentUserId,
                Users = users,
                Expenses = expenses,
                Debts = debts
            };

            return View(viewModel);
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
            var users = await _userService.GetAllUsersAsync(groupId);
            var expense = new Expense
            {
                Date = DateTime.Now,
                UserId = _userManager.GetUserId(User),
                GroupId = groupId
            };

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.Authors = new SelectList(users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }), "Id", "FullName");
            ViewBag.Users = users;

            Console.WriteLine();

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
            var users = await _userService.GetAllUsersAsync(expense.GroupId);
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
            ViewBag.Authors = new SelectList(users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }), "Id", "FullName");
            ViewBag.Users = users;
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
            if (user == null || user.Id != expense.UserId)
            {
                return View("404");
            }

            // Préparation des données pour la vue
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
            ViewBag.Users = await _userService.GetAllUsersAsync(expense.GroupId);

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
            ViewBag.Users = await _userService.GetAllUsersAsync(expense.GroupId);
            return View(expense);
        }

        private bool ExpenseExists(int id) // Vérifie si une dépense avec l'ID spécifié existe dans la base de données.// 
        {
            return _context.Expenses.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Vérifier si l'id est valide
            if (id <= 0)
            {
                return NotFound();
            }

            // Récupérer l'objet Expense à supprimer
            var expense = await _context.Expenses
                .Include(e => e.Group)  // Inclure le groupe si nécessaire
                .FirstOrDefaultAsync(e => e.Id == id);

            // Vérifier si l'objet Expense a été trouvé
            if (expense == null)
            {
                return NotFound();
            }

            var groupId = expense.GroupId; // Assurez-vous que GroupId est défini

            ViewBag.GroupId = groupId;

            // Empêcher l'accès quand la dépense n'appartient pas à l'utilisateur
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != expense.UserId)
            {
                return NotFound();
            }

            // Passer l'objet Expense à la vue pour confirmation
            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DepenseDeleteConfirmed(int id)
        {
            // Récupérer l'objet Expense à supprimer
            var expense = await _context.Expenses
                .Include(e => e.Group)  // Inclure le groupe si nécessaire
                .FirstOrDefaultAsync(e => e.Id == id);

            // Vérifier si l'objet Expense a été trouvé
            if (expense == null)
            {
                return NotFound();
            }

            // Empêcher l'accès quand la dépense n'appartient pas à l'utilisateur
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != expense.UserId)
            {
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'objet Expense de la base de données
            _context.Expenses.Remove(expense);

            // Enregistrer les modifications dans la base de données
            await _context.SaveChangesAsync();

            // Rediriger vers la vue de gestion des dépenses du groupe
            return RedirectToAction("Index", new { groupId = expense.GroupId });
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
            ViewBag.Users = await _userService.GetAllUsersAsync(groupId);

            // Calculer les soldes
            ViewBag.DebtsOfEverybody = DebtOperator.GetDebtsOfEverybody(expenses, ViewBag.Users);
            // Passer l'ID du groupe à la vue
            ViewBag.GroupId = groupId;

            return View(expenses);
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

            if (user.Id != expense.UserId)
            {
                return RedirectToAction("Index", new { groupId = expense.GroupId });
                return RedirectToAction(nameof(Index));
            }

            expense.CategoryId = null;
            _context.Update(expense);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { groupId = expense.GroupId });
            return RedirectToAction(nameof(Index));
        }
    }
}