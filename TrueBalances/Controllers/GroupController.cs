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
    }
}
