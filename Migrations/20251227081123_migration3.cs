using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kursach_CorpHubPortal.Migrations
{
    /// <inheritdoc />
    public partial class migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Task_Dates_Logical",
                schema: "dbo",
                table: "Tasks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Position_Title_HasLetters",
                schema: "dbo",
                table: "Positions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Department_Name_HasLetters",
                schema: "dbo",
                table: "Departments");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Task_Dates_Logical",
                schema: "dbo",
                table: "Tasks",
                sql: "([StartDate] IS NULL OR [StartDate] >= [CreatedAt]) AND\r\n                        ([CompletedDate] IS NULL OR [CompletedDate] >= [StartDate]) AND\r\n                        ([DueDate] IS NULL OR [DueDate] >= [CreatedAt])");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Position_Title_HasLetters",
                schema: "dbo",
                table: "Positions",
                sql: "[Title] LIKE N'%[A-Za-z]%' OR [Title] LIKE N'%[А-Яа-я]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Department_Name_HasLetters",
                schema: "dbo",
                table: "Departments",
                sql: "[Name] LIKE N'%[A-Za-z]%' OR [Name] LIKE N'%[А-Яа-я]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Task_Dates_Logical",
                schema: "dbo",
                table: "Tasks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Position_Title_HasLetters",
                schema: "dbo",
                table: "Positions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Department_Name_HasLetters",
                schema: "dbo",
                table: "Departments");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Task_Dates_Logical",
                schema: "dbo",
                table: "Tasks",
                sql: "([StartDate] IS NULL OR [StartDate] >= [CreatedAt]) AND\r\n                  ([CompletedDate] IS NULL OR [CompletedDate] >= [StartDate]) AND\r\n                  ([DueDate] IS NULL OR [DueDate] >= [CreatedAt])");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Position_Title_HasLetters",
                schema: "dbo",
                table: "Positions",
                sql: "[Title] LIKE '%[A-Za-zА-Яа-я]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Department_Name_HasLetters",
                schema: "dbo",
                table: "Departments",
                sql: "[Name] LIKE '%[A-Za-zА-Яа-я]%'");
        }
    }
}
