using Kursach_CorpHubPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Kursach_CorpHubPortal.Data.Enums;

namespace Kursach_CorpHubPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_User_Email_Format",
                        "[Email] LIKE '%_@__%.__%'");

                    tb.HasCheckConstraint("CK_User_Phone_Format",
                        "[PhoneNumber] IS NULL OR LEN([PhoneNumber]) >= 10");

                    tb.HasComment("Таблица пользователей (сотрудников) системы");
                });

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasComment("Уникальный идентификатор пользователя");

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasComment("Имя пользователя");

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasComment("Фамилия пользователя");

                entity.Property(e => e.MiddleName)
                      .HasMaxLength(50)
                      .HasComment("Отчество пользователя");

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasComment("Электронная почта (уникальная)");

                entity.Property(e => e.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(256)
                      .HasComment("Хеш пароля");

                entity.Property(e => e.PasswordSalt)
                      .IsRequired()
                      .HasMaxLength(128)
                      .HasComment("Соль для пароля");

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20)
                      .HasComment("Номер телефона");

                entity.Property(e => e.ProfilePictureUrl)
                      .HasMaxLength(500)
                      .HasComment("URL фотографии профиля");

                entity.Property(e => e.Address)
                      .HasMaxLength(200)
                      .HasComment("Адрес проживания");

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .HasDefaultValue(UserStatus.Active)
                      .HasComment("Статус учетной записи");

                entity.Property(e => e.Role)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .HasDefaultValue(UserRole.Employee)
                      .HasComment("Роль пользователя в системе");

                entity.Property(e => e.DateOfBirth)
                      .HasColumnType("date")
                      .HasComment("Дата рождения");

                entity.Property(e => e.HireDate)
                      .IsRequired()
                      .HasColumnType("date")
                      .HasDefaultValueSql("GETDATE()")
                      .HasComment("Дата приема на работу");

                entity.Property(e => e.TerminationDate)
                      .HasColumnType("date")
                      .HasComment("Дата увольнения");

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()")
                      .HasComment("Дата создания записи");

                entity.Property(e => e.UpdatedAt)
                      .HasComment("Дата последнего обновления");

                entity.HasOne(u => u.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(u => u.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Users_Departments");

                entity.HasOne(u => u.Position)
                      .WithMany(p => p.Employees)
                      .HasForeignKey(u => u.PositionId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Users_Positions");

                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Email_Unique")
                      .HasFilter("[Email] IS NOT NULL");

                entity.HasIndex(e => new { e.LastName, e.FirstName })
                      .HasDatabaseName("IX_Users_Name")
                      .IncludeProperties(e => new { e.MiddleName, e.Email });

                entity.HasIndex(e => e.Status)
                      .HasDatabaseName("IX_Users_Status")
                      .HasFilter("[Status] = 'Active'");

                entity.HasIndex(e => e.Role)
                      .HasDatabaseName("IX_Users_Role");

                entity.HasIndex(e => e.DepartmentId)
                      .HasDatabaseName("IX_Users_DepartmentId")
                      .HasFilter("[DepartmentId] IS NOT NULL");

                entity.HasIndex(e => e.PositionId)
                      .HasDatabaseName("IX_Users_PositionId")
                      .HasFilter("[PositionId] IS NOT NULL");

                entity.HasIndex(e => e.HireDate)
                      .HasDatabaseName("IX_Users_HireDate");

                entity.HasIndex(e => new { e.DepartmentId, e.Status })
                      .HasDatabaseName("IX_Users_Department_Status");
            });


            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_Department_Name_Length",
                        "LEN([Name]) >= 2 AND LEN([Name]) <= 100");

                    tb.HasCheckConstraint("CK_Department_Name_HasLetters",
                        "[Name] LIKE N'%[A-Za-z]%' OR [Name] LIKE N'%[А-Яа-я]%'");

                    tb.HasComment("Таблица отделов компании");
                });

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasComment("Уникальный идентификатор отдела");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasComment("Название отдела");

                entity.Property(e => e.Description)
                      .HasMaxLength(500)
                      .HasComment("Описание отдела и его функций");

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()")
                      .HasComment("Дата создания записи");

                entity.HasOne(d => d.Manager)
                      .WithMany()
                      .HasForeignKey(d => d.ManagerId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Departments_Users_Manager");

                entity.HasIndex(e => e.Name)
                      .IsUnique()
                      .HasDatabaseName("IX_Departments_Name_Unique")
                      .HasFilter("[Name] IS NOT NULL");

                entity.HasIndex(e => e.ManagerId)
                      .HasDatabaseName("IX_Departments_ManagerId")
                      .HasFilter("[ManagerId] IS NOT NULL");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("IX_Departments_CreatedAt")
                      .IsDescending();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_Position_Title_Length",
                        "LEN([Title]) >= 2 AND LEN([Title]) <= 100");

                    tb.HasCheckConstraint("CK_Position_Title_HasLetters",
                        "[Title] LIKE N'%[A-Za-z]%' OR [Title] LIKE N'%[А-Яа-я]%'");

                    tb.HasCheckConstraint("CK_Position_Salary_Positive",
                        "[Salary] IS NULL OR [Salary] >= 0");

                    tb.HasCheckConstraint("CK_Position_Salary_Max",
                        "[Salary] IS NULL OR [Salary] <= 10000000");


                    tb.HasComment("Справочник должностей компании");
                });

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasComment("Уникальный идентификатор должности");

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasComment("Название должности");

                entity.Property(e => e.Description)
                      .HasMaxLength(500)
                      .HasComment("Описание должностных обязанностей");

                entity.Property(e => e.Salary)
                      .HasColumnType("decimal(10,2)")
                      .HasComment("Уровень зарплаты для должности");

                entity.HasIndex(e => e.Title)
                      .IsUnique()
                      .HasDatabaseName("IX_Positions_Title_Unique")
                      .HasFilter("[Title] IS NOT NULL");

                entity.HasMany(p => p.Employees)
                      .WithOne(u => u.Position)
                      .HasForeignKey(u => u.PositionId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Entities.Task>(entity =>
            {
                entity.ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_Task_Title_Length",
                        "LEN([Title]) >= 3 AND LEN([Title]) <= 128");

                    tb.HasCheckConstraint("CK_Task_Dates_Logical",
                        @"([StartDate] IS NULL OR [StartDate] >= [CreatedAt]) AND
                        ([CompletedDate] IS NULL OR [CompletedDate] >= [StartDate]) AND
                        ([DueDate] IS NULL OR [DueDate] >= [CreatedAt])");

                    tb.HasCheckConstraint("CK_Task_Priority_Values",
                        "[Priority] IN ('Low', 'Medium', 'High', 'Critical')");

                    tb.HasComment("Таблица задач и поручений");
                });

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasComment("Уникальный идентификатор задачи");

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(128)
                      .HasComment("Название задачи");

                entity.Property(e => e.Description)
                      .HasMaxLength(2000)
                      .HasComment("Описание задачи");

                entity.Property(e => e.Result)
                      .HasMaxLength(1000)
                      .HasComment("Результат выполнения");

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .HasDefaultValue(Enums.TaskStatus.New)
                      .HasComment("Статус задачи");

                entity.Property(e => e.Priority)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(15)
                      .HasDefaultValue(TaskPriority.Medium)
                      .HasComment("Приоритет задачи");

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()")
                      .HasComment("Дата создания");

                entity.Property(e => e.DueDate)
                      .HasComment("Срок выполнения");

                entity.Property(e => e.StartDate)
                      .HasComment("Дата начала");

                entity.Property(e => e.CompletedDate)
                      .HasComment("Дата завершения");

                entity.HasOne(t => t.CreatedBy)
                      .WithMany(u => u.CreatedTasks)
                      .HasForeignKey(t => t.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Tasks_Users_CreatedBy");

                entity.HasOne(t => t.AssignedTo)
                      .WithMany(u => u.AssignedTasks)
                      .HasForeignKey(t => t.AssignedToId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Tasks_Users_AssignedTo");

                entity.HasIndex(e => e.Status)
                      .HasDatabaseName("IX_Tasks_Status")
                      .HasFilter("[Status] IN ('New', 'InProgress')");

                entity.HasIndex(e => e.Priority)
                      .HasDatabaseName("IX_Tasks_Priority");

                entity.HasIndex(e => e.DueDate)
                      .HasDatabaseName("IX_Tasks_DueDate")
                      .HasFilter("[DueDate] IS NOT NULL");

                entity.HasIndex(e => new { e.AssignedToId, e.Status })
                      .HasDatabaseName("IX_Tasks_AssignedTo_Status");

                entity.HasIndex(e => e.CreatedById)
                      .HasDatabaseName("IX_Tasks_CreatedBy");

                entity.HasIndex(e => new { e.Status, e.Priority, e.DueDate })
                      .HasDatabaseName("IX_Tasks_Status_Priority_DueDate");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("IX_Tasks_CreatedAt")
                      .IsDescending();
            });
            // Отключаем каскадное удаление для всех связей (кроме слабых сущностей)
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                                                           .SelectMany(e => e.GetForeignKeys()))
            {
                //// Сохраняем Restrict для основных сущностей
                //if (relationship.DeclaringEntityType.ClrType != typeof(TaskComment) &&
                //    relationship.DeclaringEntityType.ClrType != typeof(TimeLog))
                //{
                //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
                //}
            }

            // Настройка точности для decimal полей
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Model.GetEntityTypes()
                        .SelectMany(t => t.GetForeignKeys())
                        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                        .ToList()
                        .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
        }
    }
}
