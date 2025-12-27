using Kursach_CorpHubPortal.Data.Entities;
using Kursach_CorpHubPortal.Model.DTO;

namespace Kursach_CorpHubPortal.Services
{
    public interface IPositionService
    {
        Task<(bool success, string message)> CreatePositionAsync(PositionDTO model);
        Task<List<Position>> GetAllPositionsAsync();
    }
}
