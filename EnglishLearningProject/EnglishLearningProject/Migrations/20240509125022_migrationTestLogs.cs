using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishLearningProject.Migrations
{
    /// <inheritdoc />
    public partial class migrationTestLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestLog",
                columns: table => new
                {
                    TestLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizID = table.Column<int>(type: "int", nullable: false),
                    trueWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    falseWord1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    falseWord2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    falseWord3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    logDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    testResult = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestLog", x => x.TestLogID);
                    table.ForeignKey(
                        name: "FK_TestLog_Quiz_QuizID",
                        column: x => x.QuizID,
                        principalTable: "Quiz",
                        principalColumn: "quizID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestLog_QuizID",
                table: "TestLog",
                column: "QuizID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestLog");
        }
    }
}
