using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Data.Enums
{
    public enum TaskStatus
    {
        [Display(Name = "Новая")]
        New,

        [Display(Name = "В работе")]
        InProgress,

        [Display(Name = "Завершена")]
        Completed,

        [Display(Name = "Отложена")]
        OnHold
    }

    public enum TaskPriority
    {
        [Display(Name = "Низкий", Description = "gray")]
        Low,

        [Display(Name = "Средний", Description = "blue")]
        Medium,

        [Display(Name = "Высокий", Description = "orange")]
        High,

        [Display(Name = "Критический", Description = "red")]
        Critical
    }
}
