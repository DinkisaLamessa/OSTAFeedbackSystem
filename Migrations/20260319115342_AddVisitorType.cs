using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OstaFeedbackApp.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisitorType",
                table: "Feedbacks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitorType",
                table: "Feedbacks");
        }
    }
}
