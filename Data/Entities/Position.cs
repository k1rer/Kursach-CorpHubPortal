using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_CorpHubPortal.Data.Entities
{
    [Table("Positions")]
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Название должности")]
        public required string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Зарплата")]
        public decimal? Salary { get; set; }

        [InverseProperty(nameof(User.Position))]
        public virtual ICollection<User> Employees { get; set; } = new List<User>();
    }
}
