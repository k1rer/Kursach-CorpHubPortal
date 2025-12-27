using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Model.DTO;
using Kursach_CorpHubPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kursach_CorpHubPortal.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskService _taskService;

        public TaskController(ApplicationDbContext context, ITaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create(int? assignedToId)
        {
            var users = await _context.Users
                .Select(u => new {
                    u.Id,
                    FullName = u.LastName + " " + u.FirstName
                })
                .ToListAsync();

            ViewBag.Users = new SelectList(users, "Id", "FullName", assignedToId);

            var model = new TaskCreateDto { AssignedToId = assignedToId ?? 0 };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create(TaskCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var creatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var result = await _taskService.CreateTaskAsync(dto, creatorId);
                if (result)
                {
                    TempData["Success"] = "Задача успешно создана!";
                    return RedirectToAction("Details", "Profile", new { id = dto.AssignedToId });
                }

                ModelState.AddModelError("", "Произошла ошибка при сохранении задачи.");
            }

            var users = await _context.Users
                .Select(u => new { u.Id, FullName = u.LastName + " " + u.FirstName })
                .ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "FullName");

            return View(dto);
        }
    }
}
