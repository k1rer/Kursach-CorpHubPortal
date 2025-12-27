using System.ComponentModel.DataAnnotations;

namespace Kursach_CorpHubPortal.Data.Enums
{
    public enum UserStatus
    {
        [Display(Name = "Активный")]
        Active,

        [Display(Name = "Неактивный")]
        Inactive,

        [Display(Name = "Заблокирован")]
        Blocked,

        [Display(Name = "В отпуске")]
        OnVacation,

        [Display(Name = "На больничном")]
        OnSickLeave
    }

    public enum UserRole
    {
        [Display(Name = "Администратор")]
        Admin,

        [Display(Name = "Менеджер")]
        Manager,

        [Display(Name = "Сотрудник")]
        Employee,

        [Display(Name = "HR-специалист")]
        HR,

        [Display(Name = "ИТ-поддержка")]
        ITSupport
    }
}
