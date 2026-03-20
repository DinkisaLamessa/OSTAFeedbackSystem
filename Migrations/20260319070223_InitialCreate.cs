using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OstaFeedbackApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Commitment = table.Column<int>(type: "integer", nullable: false),
                    Transparency = table.Column<int>(type: "integer", nullable: false),
                    Innovation = table.Column<int>(type: "integer", nullable: false),
                    CommunityImpact = table.Column<int>(type: "integer", nullable: false),
                    YouthOpportunity = table.Column<int>(type: "integer", nullable: false),
                    Impressed = table.Column<string>(type: "text", nullable: true),
                    Improvement = table.Column<string>(type: "text", nullable: true),
                    DigitalService = table.Column<string>(type: "text", nullable: true),
                    YouthSupport = table.Column<string>(type: "text", nullable: true),
                    AgeGroup = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");
        }
    }
}
