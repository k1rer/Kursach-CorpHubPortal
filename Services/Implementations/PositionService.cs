using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Data.Entities;
using Kursach_CorpHubPortal.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Kursach_CorpHubPortal.Services.Implementations
{
    public class PositionService : IPositionService
    {
        private readonly ApplicationDbContext _context;

        public PositionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message)> CreatePositionAsync(PositionDTO model)
        {
            bool exists = await _context.Positions
            .AnyAsync(p => p.Title.ToLower() == model.Title.ToLower());

            if (exists) return (false, "Такая должность уже существует");

            var newPosition = new Position
            {
                Title = model.Title,
                Description = model.Description,
                Salary = model.Salary
            };

            try
            {
                _context.Positions.Add(newPosition);
                await _context.SaveChangesAsync();
                return (true, "Должность успешно создана");
            }
            catch (Exception)
            {
                return (false, "Произошла системная ошибка при сохранении");
            }
        }

        public Task<List<Data.Entities.Position>> GetAllPositionsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
