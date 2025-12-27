using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_CorpHubPortal.Data.Entities
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Название отдела")]
        public required string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [ForeignKey(nameof(Manager))]
        [Display(Name = "Руководитель")]
        public int? ManagerId { get; set; }

        [Display(Name = "Руководитель")]
        public virtual User? Manager { get; set; }

        [InverseProperty(nameof(User.Department))]
        public virtual ICollection<User> Employees { get; set; } = new List<User>();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
