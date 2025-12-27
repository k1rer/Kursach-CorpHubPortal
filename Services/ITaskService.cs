using Kursach_CorpHubPortal.Model.DTO;

namespace Kursach_CorpHubPortal.Services
{
    public interface ITaskService
    {
        public Task<bool> CreateTaskAsync(TaskCreateDto dto, int creatorId);
        public Task<bool> UpdateTaskStatusAsync(int taskId, Kursach_CorpHubPortal.Data.Enums.TaskStatus newStatus);
        public Task<bool> DeleteTaskAsync(int taskId);
        public Task<bool> EditTaskAsync(int taskId, TaskCreateDto dto);
    }
}
