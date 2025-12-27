using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Data.Entities;
using Kursach_CorpHubPortal.Model.DTO;

namespace Kursach_CorpHubPortal.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
    
        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task<bool> CreateTaskAsync(TaskCreateDto dto, int creatorId)
        {
            try
            {
                var newTask = new Data.Entities.Task
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    AssignedToId = dto.AssignedToId,
                    CreatedById = creatorId,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                    Status = Data.Enums.TaskStatus.New,
                    CreatedAt = DateTime.UtcNow
                };
    
                _context.Tasks.Add(newTask);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    
        public async Task<bool> UpdateTaskStatusAsync(int taskId, Kursach_CorpHubPortal.Data.Enums.TaskStatus newStatus)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;
    
            task.Status = newStatus;
    
            if (newStatus == Data.Enums.TaskStatus.InProgress && !task.StartDate.HasValue)
            {
                task.StartDate = DateTime.UtcNow;
            }
            else if (newStatus == Data.Enums.TaskStatus.Completed)
            {
                task.CompletedDate = DateTime.UtcNow;
            }
    
            await _context.SaveChangesAsync();
            return true;
        }
    
        public async Task<bool> EditTaskAsync(int taskId, TaskCreateDto dto)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;
    
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.AssignedToId = dto.AssignedToId;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;
    
            await _context.SaveChangesAsync();
            return true;
        }
    
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;
    
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
