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

            ExpenseViewModel viewModel = new ExpenseViewModel()
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

            return View(expense);
        }

        public async Task<IActionResult> Create(int groupId)
        {
            var categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            var users = await _userService.GetAllUsersAsync(groupId);
            var authors = new SelectList(users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }), "Id", "FullName");

            ExpenseViewModel viewModel = new ExpenseViewModel()
            {
                GroupId = groupId,
                Categories = categories,
                Users = users,
                Authors = authors
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseViewModel viewModel)
        {
            if (viewModel.SelectedUserIds == null || !viewModel.SelectedUserIds.Any())
            {
                ModelState.AddModelError(string.Empty, "Veuillez sélectionner au moins un participant.");
            }

            if (ModelState.IsValid)
            {
                if (viewModel.SelectedUserIds != null && viewModel.SelectedUserIds.Count > 0)
                {
                    viewModel.Expense.Participants = await _context.Users.Where(u => viewModel.SelectedUserIds.Contains(u.Id)).ToListAsync();
                }

                _context.Expenses.Add(viewModel.Expense);
                await _context.SaveChangesAsync();

                // Rediriger vers la page de gestion des dépenses pour le groupe
                return RedirectToAction("Index", new { groupId = viewModel.Expense.GroupId });
            }

            // Recharger des données pour le formulaire en cas d'échec de validation
            viewModel.GroupId = viewModel.Expense.GroupId;
            viewModel.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            viewModel.Users = await _userService.GetAllUsersAsync(viewModel.GroupId);
            viewModel.Authors = new SelectList(viewModel.Users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }), "Id", "FullName");

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id, int groupId)
        {
            // Vérification des paramètres
            if (id == 0 || groupId == 0)
            {
                return NotFound();
            }

            // Recherche de la dépense par son ID
            var expense = await _context.Expenses
                .Include(e => e.Category)     // Inclure les catégories
                .Include(e => e.Participants) // Inclure les participants
                .FirstOrDefaultAsync(e => e.Id == id);

            // Vérification de l'existence de la dépense ou appartient bien au groupe spécifié
            if (expense == null || expense.GroupId != groupId)
            {
                return NotFound();
            }

            // Vérification si l'utilisateur connecté est bien le propriétaire de la dépense
            var currentUserId = _userManager.GetUserId(User);

            if (currentUserId == null || currentUserId != expense.UserId)
            {
                return NotFound();
            }

            // Préparation des données pour la vue
            var categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
            var users = await _userService.GetAllUsersAsync(groupId);
            var authors = new SelectList(users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }), "Id", "FullName");

            ExpenseViewModel viewModel = new ExpenseViewModel()
            {
                GroupId = groupId,
                Expense = expense,
                Categories = categories,
                Users = users,
                Authors = authors
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExpenseViewModel viewModel)
        {
            if (viewModel.SelectedUserIds == null || !viewModel.SelectedUserIds.Any())
            {
                ModelState.AddModelError(string.Empty, "Veuillez sélectionner au moins un participant.");
            }

            if (ModelState.IsValid)
            {
                var existingExpense = await _context.Expenses
                    .Include(e => e.Participants)
                    .FirstOrDefaultAsync(e => e.Id == viewModel.Expense.Id);

                if (existingExpense == null)
                {
                    return NotFound();
                }

                // Vérifiez que la dépense appartient toujours au bon groupe
                if (existingExpense.GroupId != viewModel.Expense.GroupId)
                {
                    return NotFound(); // Ou gérer l'erreur selon vos besoins
                }

                // Mettre à jour les propriétés de l'expense
                existingExpense.Title = viewModel.Expense.Title;
                existingExpense.Amount = viewModel.Expense.Amount;
                existingExpense.Date = viewModel.Expense.Date;
                existingExpense.CategoryId = viewModel.Expense.CategoryId;

                // Mettre à jour la liste des participants
                existingExpense.Participants.Clear();

                if (viewModel.SelectedUserIds != null && viewModel.SelectedUserIds.Count > 0)
                {
                    viewModel.Expense.Participants = await _context.Users.Where(u => viewModel.SelectedUserIds.Contains(u.Id)).ToListAsync();
                    existingExpense.Participants = viewModel.Expense.Participants;
                }

                _context.Update(existingExpense);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { groupId = viewModel.Expense.GroupId });
            }

            viewModel.GroupId = viewModel.Expense.GroupId;
            viewModel.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            viewModel.Users = await _userService.GetAllUsersAsync(viewModel.GroupId);
            viewModel.Authors = new SelectList(viewModel.Users.Select(u => new { u.Id, FullName = $"{u.FirstName} {u.LastName}" }), "Id", "FullName");

            return View(viewModel);
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
            var currentUserId = _userManager.GetUserId(User);

            if (currentUserId != expense.UserId)
            {
                return NotFound();
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

            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            // Empêcher l'accès quand la dépenses n'appartient pas à l'utilisateur
            var user = await _userManager.GetUserAsync(User);

            if (user.Id != expense.UserId)
            {
                return RedirectToAction("Index", new { groupId = expense.GroupId });
            }

            expense.CategoryId = null;
            _context.Update(expense);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { groupId = expense.GroupId });
        }
    }
}