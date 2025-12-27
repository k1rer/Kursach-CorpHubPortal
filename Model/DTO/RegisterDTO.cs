using Kursach_CorpHubPortal.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Model.DTO
{
    public class RegisterDTO
    {
        // --- ОБЯЗАТЕЛЬНЫЕ ПОЛЯ (Верхняя часть формы) ---

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может быть длиннее 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, ErrorMessage = "Фамилия не может быть длиннее 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат почты")]
        [StringLength(100)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Display(Name = "Телефон")]
        [RegularExpression(@"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$", ErrorMessage = "Введите телефон в формате +7 (999) 000-00-00")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [StringLength(256, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 256 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; } = string.Empty;


        // --- ДОПОЛНИТЕЛЬНЫЕ ПОЛЯ (Нижняя часть / Свернуто) ---

        [Display(Name = "Отчество")]
        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Display(Name = "Отдел")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Должность")]
        public int? PositionId { get; set; }

        [Required(ErrorMessage = "Необходимо назначить роль")]
        [Display(Name = "Роль пользователя")]
        public UserRole Role { get; set; } = UserRole.Employee;

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
    }
}
