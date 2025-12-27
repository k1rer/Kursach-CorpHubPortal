using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Data.Entities;
using Kursach_CorpHubPortal.Data.Enums;
using Kursach_CorpHubPortal.Model.DTO;
using Kursach_CorpHubPortal.Services;
using Kursach_CorpHubPortal.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kursach_CorpHubPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccount _accountService;
        private readonly IPositionService _positionService;
        private readonly ApplicationDbContext _context;

        public AdminController(IAccount accountService, IPositionService positionService, ApplicationDbContext context)
        {
            _accountService = accountService;
            _positionService = positionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            await PopulateSelectionLists();
            return View(new RegisterDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectionLists();
                return View(model);
            }

            var (success, message) = await _accountService.RegisterAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = "Сотрудник успешно создан";
                return RedirectToAction("UsersList", "Admin");
            }

            ModelState.AddModelError(string.Empty, message);
            await PopulateSelectionLists();
            return View(model);
        }

        private async System.Threading.Tasks.Task PopulateSelectionLists()
        {
            var departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync();
            var positions = await _context.Positions.OrderBy(p => p.Title).ToListAsync();

            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.Positions = new SelectList(positions, "Id", "Title");
        }

        [HttpGet]
        public IActionResult CreatePosition() => View(new PositionDTO());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePosition(PositionDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _positionService.CreatePositionAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("PositionsList");
            }

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateDepartment()
        {
            var managers = await _context.Users
                .Where(u => u.Role == UserRole.Manager)
                .OrderBy(u => u.LastName)
                .Select(u => new
                {
                    u.Id,
                    FullName = $"{u.LastName} {u.FirstName} {u.MiddleName}".Trim()
                })
                .ToListAsync();
            ViewBag.ManagersList = new SelectList(managers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                department.CreatedAt = DateTime.UtcNow;
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DepartmentsList));
            }

            // Если форма невалидна, нужно заново наполнить список руководителей перед возвратом View
            var managers = await _context.Users
                .Where(u => u.Role == UserRole.Manager)
                .Select(u => new { u.Id, FullName = $"{u.LastName} {u.FirstName}" })
                .ToListAsync();
            ViewBag.ManagersList = new SelectList(managers, "Id", "FullName");

            return View(department);
        }

        public async Task<IActionResult> UsersList(string searchTerm, int? departmentId)
        {
            var query = _context.Users
                .Include(u => u.Department)
                .Include(u => u.Position)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(u =>
                    u.LastName.ToLower().Contains(searchTerm) ||
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(u => u.DepartmentId == departmentId);
            }

            var users = await query.OrderByDescending(u => u.Id).ToListAsync();

            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            ViewBag.SearchTerm = searchTerm;

            return View(users);
        }

        public async Task<IActionResult> DepartmentsList()
        {
            // Обязательно подгружаем руководителя (Manager) и список сотрудников (Employees)
            var departments = await _context.Departments
                .Include(d => d.Manager)
                .Include(d => d.Employees)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return View(departments);
        }

        // Метод для отображения списка должностей
        public async Task<IActionResult> PositionsList()
        {
            // Подгружаем список сотрудников, чтобы сработал .Count в представлении
            var positions = await _context.Positions
                .Include(p => p.Employees)
                .OrderBy(p => p.Title)
                .ToListAsync();

            return View(positions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }


            // var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // if (id == currentUserId) return BadRequest("Нельзя удалить свой аккаунт");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(UsersList));
        }

        // Удаление департамента
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees) // Проверяем наличие сотрудников
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null) return NotFound();

            if (department.Employees.Any())
            {
                // Здесь можно добавить сообщение об ошибке, что в отделе есть люди
                TempData["Error"] = "Нельзя удалить департамент, в котором числятся сотрудники.";
                return RedirectToAction(nameof(DepartmentsList));
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DepartmentsList));
        }

        // Удаление позиции (должности)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _context.Positions
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (position == null) return NotFound();

            if (position.Employees.Any())
            {
                TempData["Error"] = "Нельзя удалить должность, которую занимают сотрудники.";
                return RedirectToAction(nameof(PositionsList));
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PositionsList));
        }
    }
}
