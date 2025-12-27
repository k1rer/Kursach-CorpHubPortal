using Kursach_CorpHubPortal.Model.DTO;

namespace Kursach_CorpHubPortal.Services
{
    public interface IDepartmentService
    {
        public Task<(bool success, string message)> CreateDepartmentAsync(DepartmentDTO model);
    }
}
