using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Model.DTO
{
    public class PositionDTO
    {
        [Required(ErrorMessage = "Название должности обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
        [Display(Name = "Название должности")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описание не может превышать 500 символов")]
        [Display(Name = "Описание обязанностей")]
        public string? Description { get; set; }

        [Range(0, 10000000, ErrorMessage = "Зарплата должна быть положительным числом")]
        [Display(Name = "Оклад / Зарплата")]
        public decimal? Salary { get; set; }
    }
}
