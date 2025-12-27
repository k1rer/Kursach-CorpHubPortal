using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Model.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Почта обязательна")]
        [EmailAddress(ErrorMessage = "Некорректный формат почты")]
        [StringLength(100, ErrorMessage = "Слишком большой адрес почты")]
        [Display(Name = "Почта")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Номер обязателен")]
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        [StringLength(20, ErrorMessage = "Слишком длинный номер")]
        [Display(Name = "Телефон")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "Слишком длинный пароль")]
        [Display(Name = "Пароль")]
        public required string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
