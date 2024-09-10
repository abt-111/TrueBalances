using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Models.ViewModels;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Services.Interfaces;

namespace TrueBalances.Controllers
{
    public class GroupController : Controller
    {
        private readonly TrueBalancesDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IGroupService _groupService;

        public GroupController(TrueBalancesDbContext context, IUserService userService, UserManager<CustomUser> userManager, IGroupService groupService)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _groupService = groupService;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return View();
            }

            var groups = await _groupService.GetGroupsByUserIdAsync(currentUser.Id);

            GroupViewModel viewModel = new GroupViewModel()
            {
                CurrentUser = currentUser,
                Groups = groups
            };

            return View(viewModel);
        }

        // Create Group (GET)
        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetAllUsersAsync();
            var currentUserId = _userManager.GetUserId(User);

            var viewModel = new GroupViewModel
            {
                Users = users.Where(u => u.Id != currentUserId).ToList()
            };

            return View(viewModel);
        }

        // Create Group (POST)
        [HttpPost]
        public async Task<IActionResult> Create(GroupViewModel viewModel)
        {
            var currentUserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                var group = new Models.Group
                {
                    Name = viewModel.Group.Name,
                    Members = viewModel.SelectedUserIds.Select(id => new UserGroup { CustomUserId = id }).ToList()
                };

                // Ajouter l'utilisateur courant comme membre du groupe
                if (!string.IsNullOrEmpty(currentUserId))
                {
                    group.Members.Add(new UserGroup { CustomUserId = currentUserId });
                }

                await _groupService.AddAsync(group);

                return RedirectToAction("Index");
            }

            var users = await _userService.GetAllUsersAsync();
            viewModel.Users = users.Where(u => u.Id != currentUserId).ToList();

            return View(viewModel);
        }

        // Group Edit(Get)
        public async Task<IActionResult> Edit(int id)
        {
            // Empêche l'accès au non-membre du groupe
            var currentUserId = _userManager.GetUserId(User);

            if (!_groupService.UserIsInGroup(currentUserId, id))
            {
                return NotFound();
            }

            var group = await _groupService.GetByIdWithExpensesAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var users = await _userService.GetAllUsersAsync();

            var viewModel = new GroupViewModel
            {
                Group = group,
                Users = users
            };

            return View(viewModel);
        }

        // Group Edit(Post)
        [HttpPost]
        public async Task<IActionResult> Edit(GroupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Récupérer le groupe à mettre à jour
                var existingGroup = await _groupService.GetByIdWithExpensesAsync(viewModel.Group.Id);

                if (existingGroup == null)
                {
                    return NotFound();
                }

                // Mettre à jour le nom du groupe
                existingGroup.Name = viewModel.Group.Name;

                // Ajouter les nouveaux membres
                existingGroup.Members.Clear();
                existingGroup.Members = viewModel.SelectedUserIds.Select(id => new UserGroup { CustomUserId = id }).ToList();

                await _groupService.UpdateAsync(existingGroup);

                return RedirectToAction("Details", new { id = viewModel.Group.Id });
            }

            viewModel.Users = await _userService.GetAllUsersAsync();
            return View(viewModel);
        }

        // Group Details
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Empêche l'accès au non-membre du groupe
            var currentUserId = _userManager.GetUserId(User);

            if (!_groupService.UserIsInGroup(currentUserId, id))
            {
                return NotFound();
            }

            // Récupérer le groupe avec les participants, la catégorie et les dépenses associées
            var group = await _context.Groups
                .Include(g => g.Expenses)
                .ThenInclude(e => e.Category)
                .Include(g => g.Members)
                //.ThenInclude(m => m.CustomUser) // Inclure les utilisateurs associés aux membres
                .Include(g => g.Expenses) // Inclure les dépenses associées
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            List<string> categoriesChoosed = new List<string>();
            categoriesChoosed = group.Expenses.Select(e => e.Category.Name).Distinct().ToList();

            var viewModel = new GroupViewModel
            {
                Group = group,
                //Users = await _userService.GetAllUsersAsync(), // Si nécessaire pour d'autres fonctionnalités
                SelectedUserIds = group.Members.Select(m => m.CustomUserId).ToList(),
                CategoriesChoosed = categoriesChoosed
            };

            return View(viewModel);
        }

        // Delete Group (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Empêche l'accès au non-membre du groupe
            var currentUserId = _userManager.GetUserId(User);

            if (!_groupService.UserIsInGroup(currentUserId, id))
            {
                return NotFound();
            }

            var group = await _groupService.GetByIdWithExpensesAsync(id);
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
            await _groupService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
