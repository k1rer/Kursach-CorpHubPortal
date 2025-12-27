using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Model.DTO
{
    public class DepartmentDTO
    {
        [Required(ErrorMessage = "Название отдела обязательно")]
        [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
        [Display(Name = "Название отдела")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описание не может превышать 500 символов")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Руководитель")]
        public int? ManagerId { get; set; }
    }
}
