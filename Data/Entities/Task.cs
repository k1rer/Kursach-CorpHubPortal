using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kursach_CorpHubPortal.Data.Enums;

namespace Kursach_CorpHubPortal.Data.Entities
{
    [Table("Tasks")]
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [Display(Name = "Название")]
        public required string Title { get; set; }

        [MaxLength(2000)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.New;

        [Required]
        [Display(Name = "Приоритет")]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;


        [Required]
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Срок выполнения")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Дата начала")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Дата завершения")]
        public DateTime? CompletedDate { get; set; }

        [NotMapped]
        [Display(Name = "Затраченное время")]
        public (int Days, int Hours, int Minutes) Duration
        {
            get
            {
                if (!StartDate.HasValue || !CompletedDate.HasValue)
                    return (0, 0, 0);

                TimeSpan span = CompletedDate.Value - StartDate.Value;

                return (
                    Days: span.Days,
                    Hours: span.Hours,
                    Minutes: span.Minutes
                );
            }
        }


        [ForeignKey(nameof(CreatedBy))]
        [Display(Name = "Создатель")]
        public required int CreatedById { get; set; }

        [Display(Name = "Создатель")]
        public virtual User? CreatedBy { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        [Display(Name = "Исполнитель")]
        public required int AssignedToId { get; set; }

        [Display(Name = "Исполнитель")]
        public virtual User? AssignedTo { get; set; }


        [MaxLength(1000)]
        [Display(Name = "Результат")]
        public string? Result { get; set; }

        [NotMapped]
        [Display(Name = "Просрочена")]
        public bool IsOverdue => DueDate.HasValue &&
                                 DueDate.Value < DateTime.UtcNow &&
                                 Status != Enums.TaskStatus.Completed;

        [NotMapped]
        [Display(Name = "В работе")]
        public bool IsInProgress => Status == Enums.TaskStatus.InProgress;
    }
}
