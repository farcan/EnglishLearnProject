using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishLearningProject.Migrations
{
    /// <inheritdoc />
    public partial class migrationTestLogUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedAnswer",
                table: "TestLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrueWordTR",
                table: "TestLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "trueSentences",
                table: "TestLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedAnswer",
                table: "TestLog");

            migrationBuilder.DropColumn(
                name: "TrueWordTR",
                table: "TestLog");

            migrationBuilder.DropColumn(
                name: "trueSentences",
                table: "TestLog");
        }
    }
}
