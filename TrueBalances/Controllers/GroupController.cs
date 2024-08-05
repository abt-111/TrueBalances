using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public IActionResult Index()
        {

            return View();
        }

        // Create Group (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Create Group (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _groupService.CreateGroupAsync(group, userId);
                return RedirectToAction(actionName: "Index", controllerName: "Group");
            }
            return View(group);
        }

        // Edit Group (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var group = await _groupService.GetGroupAsync(id);
            if (group is null)
            {
                return NotFound();
            }
            return View(group);
        }

        // Edit Group (Post)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _groupService.UpdateGroupAsync(group);
                    return RedirectToAction(actionName: "Index", controllerName: "Group");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Une erreur s'est produite lors de la mise à jour du groupe.");
                }
            }
            return View(group);
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
        public async Task<IActionResult> AddMember(int groupId, string userId)
        {
            await _groupService.AddMemberAsync(groupId, userId);
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

        // Group Details
        public async Task<IActionResult> Details(int id)
        {
            var group = await _groupService.GetGroupAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            //var summary = _groupService.CalculateSummary(group);
            //ViewBag.Summary = summary;

            return View(group);
        }

        private bool GroupExists(int id)
        {
            var group = _groupService.GetGroupAsync(id).Result;
            return group != null;
        }
    }
}
