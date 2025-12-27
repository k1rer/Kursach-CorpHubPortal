using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kursach_CorpHubPortal.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Уникальный идентификатор должности")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Название должности"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Описание должностных обязанностей"),
                    Salary = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: true, comment: "Уровень зарплаты для должности")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.CheckConstraint("CK_Position_Salary_Max", "[Salary] IS NULL OR [Salary] <= 10000000");
                    table.CheckConstraint("CK_Position_Salary_Positive", "[Salary] IS NULL OR [Salary] >= 0");
                    table.CheckConstraint("CK_Position_Title_HasLetters", "[Title] LIKE '%[A-Za-zА-Яа-я]%'");
                    table.CheckConstraint("CK_Position_Title_Length", "LEN([Title]) >= 2 AND LEN([Title]) <= 100");
                },
                comment: "Справочник должностей компании");

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Уникальный идентификатор отдела")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Название отдела"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Описание отдела и его функций"),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Дата создания записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.CheckConstraint("CK_Department_Name_HasLetters", "[Name] LIKE '%[A-Za-zА-Яа-я]%'");
                    table.CheckConstraint("CK_Department_Name_Length", "LEN([Name]) >= 2 AND LEN([Name]) <= 100");
                },
                comment: "Таблица отделов компании");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Уникальный идентификатор пользователя")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Имя пользователя"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Фамилия пользователя"),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Отчество пользователя"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Электронная почта (уникальная)"),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "Хеш пароля"),
                    PasswordSalt = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, comment: "Соль для пароля"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "Номер телефона"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active", comment: "Статус учетной записи"),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Employee", comment: "Роль пользователя в системе"),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "URL фотографии профиля"),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true, comment: "Дата рождения"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Адрес проживания"),
                    HireDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "GETDATE()", comment: "Дата приема на работу"),
                    TerminationDate = table.Column<DateTime>(type: "date", nullable: true, comment: "Дата увольнения"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_User_Email_Format", "[Email] LIKE '%_@__%.__%'");
                    table.CheckConstraint("CK_User_Phone_Format", "[PhoneNumber] IS NULL OR LEN([PhoneNumber]) >= 10");
                    table.ForeignKey(
                        name: "FK_Users_Departments",
                        column: x => x.DepartmentId,
                        principalSchema: "dbo",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Positions",
                        column: x => x.PositionId,
                        principalSchema: "dbo",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Таблица пользователей (сотрудников) системы");

            migrationBuilder.CreateTable(
                name: "Tasks",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Уникальный идентификатор задачи")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, comment: "Название задачи"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Описание задачи"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "New", comment: "Статус задачи"),
                    Priority = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false, defaultValue: "Medium", comment: "Приоритет задачи"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Дата создания"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Срок выполнения"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата начала"),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Дата завершения"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    AssignedToId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Результат выполнения")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.CheckConstraint("CK_Task_Dates_Logical", "([StartDate] IS NULL OR [StartDate] >= [CreatedAt]) AND\r\n                  ([CompletedDate] IS NULL OR [CompletedDate] >= [StartDate]) AND\r\n                  ([DueDate] IS NULL OR [DueDate] >= [CreatedAt])");
                    table.CheckConstraint("CK_Task_Priority_Values", "[Priority] IN ('Low', 'Medium', 'High', 'Critical')");
                    table.CheckConstraint("CK_Task_Title_Length", "LEN([Title]) >= 3 AND LEN([Title]) <= 128");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssignedTo",
                        column: x => x.AssignedToId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedBy",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Таблица задач и поручений");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedAt",
                schema: "dbo",
                table: "Departments",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerId",
                schema: "dbo",
                table: "Departments",
                column: "ManagerId",
                filter: "[ManagerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name_Unique",
                schema: "dbo",
                table: "Departments",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Title_Unique",
                schema: "dbo",
                table: "Positions",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedTo_Status",
                schema: "dbo",
                table: "Tasks",
                columns: new[] { "AssignedToId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedAt",
                schema: "dbo",
                table: "Tasks",
                column: "CreatedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedBy",
                schema: "dbo",
                table: "Tasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DueDate",
                schema: "dbo",
                table: "Tasks",
                column: "DueDate",
                filter: "[DueDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Priority",
                schema: "dbo",
                table: "Tasks",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                schema: "dbo",
                table: "Tasks",
                column: "Status",
                filter: "[Status] IN ('New', 'InProgress')");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status_Priority_DueDate",
                schema: "dbo",
                table: "Tasks",
                columns: new[] { "Status", "Priority", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Department_Status",
                schema: "dbo",
                table: "Users",
                columns: new[] { "DepartmentId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                schema: "dbo",
                table: "Users",
                column: "DepartmentId",
                filter: "[DepartmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Unique",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HireDate",
                schema: "dbo",
                table: "Users",
                column: "HireDate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                schema: "dbo",
                table: "Users",
                columns: new[] { "LastName", "FirstName" })
                .Annotation("SqlServer:Include", new[] { "MiddleName", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PositionId",
                schema: "dbo",
                table: "Users",
                column: "PositionId",
                filter: "[PositionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                schema: "dbo",
                table: "Users",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Status",
                schema: "dbo",
                table: "Users",
                column: "Status",
                filter: "[Status] = 'Active'");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_Manager",
                schema: "dbo",
                table: "Departments",
                column: "ManagerId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_Manager",
                schema: "dbo",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "Tasks",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "dbo");
        }
    }
}
