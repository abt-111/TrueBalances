using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Repositories.Services;

namespace TrueBalances.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public GroupController(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
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

            var viewModel = new GroupDetailsViewModel
            {
                AvailableUsers = availableUsers.Select(u => new CustomUser { Id = u.Id, UserName = u.UserName }).ToList(),
                SelectedUserIds = new List<string>()
            };

            return View(viewModel);

        }
        // Create Group (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var group = new Group
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

        // Group Edit
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


            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GroupDetailsViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                var availableUsers = await _userService.GetAllUsersAsync();
                viewModel.AvailableUsers = availableUsers;
                return View(viewModel);
            }

            var group = await _groupService.GetGroupAsync(viewModel.Group.Id);
            if (group == null)
            {
                return NotFound();
            }

            group.Name = viewModel.Group.Name;

            await _groupService.UpdateGroupAsync(group);

            // Gestion des membres (ajout et suppression)
            await _groupService.UpdateGroupMembersAsync(group.Id, viewModel.SelectedUserIds);

            return RedirectToAction("Index");
        }


        // Group Details
        public async Task<IActionResult> Details(int id)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _groupService.DeleteGroupAsync(id);
            return RedirectToAction(actionName: "Index", controllerName: "Group");
        }

        // Add Member (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
    }
}

