using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Data.Entities;
using Kursach_CorpHubPortal.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Kursach_CorpHubPortal.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message)> CreateDepartmentAsync(DepartmentDTO model)
        {
            bool exists = await _context.Departments
                .AnyAsync(d => d.Name.ToLower() == model.Name.ToLower());

            if (exists) return (false, "Отдел с таким названием уже существует");

            var department = new Department
            {
                Name = model.Name,
                Description = model.Description,
                ManagerId = model.ManagerId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return (true, "Отдел успешно создан");
            }
            catch (Exception)
            {
                return (false, "Ошибка при сохранении отдела");
            }
        }
    }
}
