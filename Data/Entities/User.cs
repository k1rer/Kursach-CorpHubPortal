using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kursach_CorpHubPortal.Data.Enums;
using Kursach_CorpHubPortal.Services.Implementations;
namespace Kursach_CorpHubPortal.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [MaxLength(50)]
        [Display(Name = "Имя")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [MaxLength(50)]
        [Display(Name = "Фамилия")]
        public required string LastName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [NotMapped]
        [Display(Name = "Полное имя")]
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public required string PasswordHash { get; set; }

        [Required]
        [Display(Name = "Соль для пароля")]
        public required string PasswordSalt { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Телефон")]
        public required string PhoneNumber { get; set; }

        [NotMapped]
        [Display(Name = "Форматированный телефон")]
        public string FormattedPhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length < 11)
                    return PhoneNumber;

                return $"+7 ({PhoneNumber.Substring(1, 3)}) {PhoneNumber.Substring(4, 3)}-{PhoneNumber.Substring(7, 2)}-{PhoneNumber.Substring(9, 2)}";
            }
        }

        [ForeignKey(nameof(Department))]
        [Display(Name = "Отдел")]
        public int? DepartmentId { get; set; }
        
        [Display(Name = "Отдел")]
        public virtual Department? Department { get; set; }

        [ForeignKey(nameof(Position))]
        [Display(Name = "Должность")]
        public int? PositionId { get; set; }
        
        [Display(Name = "Должность")]
        public virtual Position? Position { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public UserStatus Status { get; set; } = UserStatus.Active;
        
        [Required]
        [Display(Name = "Роль")]
        public UserRole Role { get; set; } = UserRole.Employee;

        [MaxLength(500)]
        [Display(Name = "Фото")]
        public string? ProfilePictureUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(200)]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата приема")]
        [Column(TypeName = "date")]
        public DateTime HireDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Дата увольнения")]
        [Column(TypeName = "date")]
        public DateTime? TerminationDate { get; set; }

        [Required]
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Дата обновления")]
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty(nameof(Task.AssignedTo))]
        public virtual ICollection<Task> AssignedTasks { get; set; } = new List<Task>();

        [InverseProperty(nameof(Task.CreatedBy))]
        public virtual ICollection<Task> CreatedTasks { get; set; } = new List<Task>();

        [NotMapped]
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [NotMapped]
        public bool IsActive => Status == UserStatus.Active && !TerminationDate.HasValue;

        [NotMapped]
        public int? YearsOfService
        {
            get
            {
                var endDate = TerminationDate ?? DateTime.Today;
                var years = endDate.Year - HireDate.Year;
                if (HireDate.Date > endDate.AddYears(-years)) years--;
                return years;
            }
        }
    }
}
