namespace Kursach_CorpHubPortal.Model.View
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Kursach_CorpHubPortal.Data.Enums.TaskStatus Status { get; set; }
        public Kursach_CorpHubPortal.Data.Enums.TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        // Имена для отображения без подгрузки всей сущности User
        public string AssignedToName { get; set; } = null!;
        public string CreatedByName { get; set; } = null!;

        public bool IsOverdue { get; set; }
        public bool IsInProgress { get; set; }
    }
}
