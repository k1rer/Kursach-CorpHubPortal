using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Model.DTO
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Введите название задачи")]
        [StringLength(128, ErrorMessage = "Название слишком длинное")]
        [Display(Name = "Название задачи")]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Выберите исполнителя")]
        [Display(Name = "Исполнитель")]
        public int AssignedToId { get; set; }

        [Required]
        [Display(Name = "Приоритет")]
        public Kursach_CorpHubPortal.Data.Enums.TaskPriority Priority { get; set; } = Data.Enums.TaskPriority.Medium;

        [Display(Name = "Срок выполнения")]
        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }
    }
}
