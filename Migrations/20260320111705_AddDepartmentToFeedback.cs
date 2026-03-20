using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OstaFeedbackApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Feedbacks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Feedbacks");
        }
    }
}
