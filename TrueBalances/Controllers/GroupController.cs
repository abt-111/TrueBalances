using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.DbRepositories;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Tools;

namespace TrueBalances.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly UserContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IGenericRepository<Category> _categoryRepository;

        public GroupController(UserContext context, IGroupService groupService, IUserService userService, UserManager<CustomUser> userManager, IGenericRepository<Category> categoryRepository)
        {
            _groupService = groupService;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var groups = await _groupService.GetAllGroups();
            return View(groups);
        }

        // Create Group (GET)
        public async Task<IActionResult> Create()
        {
            var availableUsers = await _userService.GetAllUsersAsync();
            var currentUserId = _userManager.GetUserId(User);

            var viewModel = new GroupDetailsViewModel
            {
                //AvailableUsers = availableUsers.Select(u => new CustomUser { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName }).ToList(),
                AvailableUsers = availableUsers.Where(u => u.Id != currentUserId).ToList(),
                SelectedUserIds = new List<string>()
            };

            return View(viewModel);
        }

        // Create Group (POST)
        [HttpPost]
        public async Task<IActionResult> Create(GroupDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var group = new Models.Group
                {
                    Name = viewModel.Group.Name,
                    Members = viewModel.SelectedUserIds.Select(id => new UserGroup { CustomUserId = id }).ToList()
                };

                // Ajouter l'utilisateur courant comme membre du groupe
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(userId))
                {
                    group.Members.Add(new UserGroup { CustomUserId = userId });
                }

                await _groupService.CreateGroupAsync(group, userId);

                return RedirectToAction("Index");
            }

            viewModel.AvailableUsers = await _userService.GetAllUsersAsync();
            return View(viewModel);
        }

        // Group Edit(Get)
        public async Task<IActionResult> Edit(int id)
        {
 
                var group = await _groupService.GetGroupAsync(id);
                if (group == null)
                {
                    return NotFound();
                }

                var availableUsers = await _userService.GetAllUsersAsync();

                var viewModel = new GroupDetailsViewModel
                {
                    Group = group,
                    AvailableUsers = availableUsers
                };

                ViewBag.AvailableUsers = availableUsers;

                return View(viewModel);

        }

        // Group Edit(Post)
        [HttpPost]
        public async Task<IActionResult> Edit(GroupDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AvailableUsers = await _userService.GetAllUsersAsync();
                return View(viewModel);
            }

            // Récupérer le groupe à mettre à jour
            var group = await _groupService.GetGroupAsync(viewModel.Group.Id);
            if (group == null)
            {
                return NotFound();
            }

            // Mettre à jour le nom du groupe
            group.Name = viewModel.Group.Name;
            await _groupService.UpdateGroupAsync(group);

            // Ajouter les nouveaux membres
            var selectedUserIds = viewModel.SelectedUserIds ?? new List<string>();
            var currentMemberIds = group.Members.Select(m => m.CustomUserId).ToList();

            // Membres à ajouter
            var membersToAdd = selectedUserIds.Except(currentMemberIds).ToList();
            if (membersToAdd.Any())
            {
                await _groupService.AddMembersAsync(group.Id, membersToAdd);
            }

            // Membres à supprimer
            var membersToRemove = currentMemberIds.Except(selectedUserIds).ToList();
            if (membersToRemove.Any())
            {
                foreach (var userId in membersToRemove)
                {
                    await _groupService.RemoveMemberAsync(group.Id, userId);
                }
            }

            return RedirectToAction("Index");
        }

        // Group Details
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // Récupérer le groupe avec les participants, la catégorie et les dépenses associées
            var group = await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(m => m.CustomUser)  // Inclure les utilisateurs associés aux membres
                .Include(g => g.Category)  // Inclure la catégorie
                .Include(g => g.Expenses)  // Inclure les dépenses associées
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }
            // Charger les catégories disponibles dans ViewBag
            //ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");

            // Initialiser le viewModel avec des vérifications pour éviter les nulls
            var viewModel = new GroupDetailsViewModel
            {
                Group = group,
                AvailableUsers = await _userService.GetAllUsersAsync(), // Si nécessaire pour d'autres fonctionnalités
                SelectedUserIds = group.Members?.Select(m => m.CustomUserId).ToList() ?? new List<string>(),
                CategoryId = group.CategoryId
            };

            return View(viewModel);
        }

        // Delete Group (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _groupService.GetGroupAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // Delete Group (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _groupService.DeleteGroupAsync(id);
            return RedirectToAction(actionName: "Index", controllerName: "Group");
        }

        // Add Member (POST)
        [HttpPost]
        public async Task<IActionResult> AddMembers(int groupId, List<string> selectedUserIds)
        {
            var errors = await _groupService.AddMembersAsync(groupId, selectedUserIds);
            if (errors.Any())
            {
                TempData["Errors"] = errors;
            }

            return RedirectToAction(nameof(Details), new { id = groupId });
        }

        // Remove Member (POST)
        [HttpPost]
        public async Task<IActionResult> RemoveMember(int groupId, string userId)
        {
            await _groupService.RemoveMemberAsync(groupId, userId);
            return RedirectToAction(nameof(Details), new { id = groupId });
        }

        private bool GroupExists(int id)
        {
            var group = _groupService.GetGroupAsync(id).Result;
            return group != null;
        }

        //Methode affichant les récapitulatifs
        public async Task<IActionResult> DepenseIndex(int id)
        {
            var expenses = await _context.Expenses
                .Where(e => e.GroupId == id)
                .Include(e => e.Category)
                .Include(e => e.Participants)
                .ToListAsync();

            // Utilisation du ViewBag pour récupérer l'id de l'utilisateur courant dans la vue
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            // Utilisation du ViewBag pour récupérer la liste des utilisateurs dans la vue
            ViewBag.Users = await _userManager.Users.ToListAsync();

            ViewBag.Debts = DebtOperator.GetSomeoneDebts(expenses, ViewBag.Users, ViewBag.CurrentUserId);

            ViewBag.GroupId = id;

            return View(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int GroupId, int? CategoryId)
        {
            var group = await _groupService.GetGroupAsync(GroupId);
            if (group == null)
            {
                return NotFound();
            }

            group.CategoryId = CategoryId;
            await _groupService.UpdateGroupAsync(group);

            return RedirectToAction("Details", new { id = GroupId });
        }

        //methode pour creer une dépense à partir du group
        public async Task<IActionResult> DepenseCreate(int groupId)
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
        //[Route("Expense/Create")]
        public async Task<IActionResult> DepenseCreate(Expense expense)
        {
            if (ModelState.IsValid)
            {
                if (expense.SelectedUserIds != null && expense.SelectedUserIds.Count > 0)
                {
                    expense.Participants = await _context.Users.Where(u => expense.SelectedUserIds.Contains(u.Id)).ToListAsync();
                }

                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();

                // Rediriger vers la page de gestion des dépenses pour le groupe
                return RedirectToAction("DepenseIndex", new { id = expense.GroupId });
            }

            // Recharger les catégories et les utilisateurs en cas d'échec de validation
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", expense.CategoryId);
            ViewBag.Users = _context.Users.ToList();
            return View(expense);
        }

        //methode pour modifier une dépense à partir du group
        public async Task<IActionResult> DepenseEdit(int id, int groupId)
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

            // Vérification de l'existence de la dépense
            if (expense == null)
            {
                return NotFound();
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


        //methode pour Vérifier si une dépense avec l'ID spécifié existe dans la base de données.

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> DepenseEdit(int id, Expense expense)
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

                    return RedirectToAction("DepenseIndex", new { id = expense.GroupId });
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

        //methode pour voir les détails d'une dépense à partir du group
        public async Task<IActionResult> DepenseDetails(int id)
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

        //methode pour supprimer une dépense à partir du group
        public async Task<IActionResult> DepenseDelete(int id)
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
            if (user.Id != expense.CustomUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            // Passer l'objet Expense à la vue pour confirmation
            return View(expense);
        }

        // POST: ExpenseController/Delete/5
        [HttpPost, ActionName("DepenseDelete")]
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
            if (user.Id != expense.CustomUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'objet Expense de la base de données
            _context.Expenses.Remove(expense);

            // Enregistrer les modifications dans la base de données
            await _context.SaveChangesAsync();

            // Rediriger vers la vue de gestion des dépenses du groupe
            return RedirectToAction("DepenseIndex", new { id = expense.GroupId });
        }

        //action pour la partie solde
        public async Task<IActionResult> DepenseSolde(int groupId)
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



    }

}


    
    
