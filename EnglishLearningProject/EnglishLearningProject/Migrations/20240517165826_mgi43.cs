using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishLearningProject.Migrations
{
    /// <inheritdoc />
    public partial class mgi43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_AspNetUsers_UserID",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Word_WordID",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_TestLog_Quiz_QuizID",
                table: "TestLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Word_AspNetUsers_UserID",
                table: "Word");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_AspNetUsers_UserID",
                table: "Quiz",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Word_WordID",
                table: "Quiz",
                column: "WordID",
                principalTable: "Word",
                principalColumn: "WordID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestLog_Quiz_QuizID",
                table: "TestLog",
                column: "QuizID",
                principalTable: "Quiz",
                principalColumn: "quizID");

            migrationBuilder.AddForeignKey(
                name: "FK_Word_AspNetUsers_UserID",
                table: "Word",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_AspNetUsers_UserID",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Word_WordID",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_TestLog_Quiz_QuizID",
                table: "TestLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Word_AspNetUsers_UserID",
                table: "Word");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_AspNetUsers_UserID",
                table: "Quiz",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Word_WordID",
                table: "Quiz",
                column: "WordID",
                principalTable: "Word",
                principalColumn: "WordID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestLog_Quiz_QuizID",
                table: "TestLog",
                column: "QuizID",
                principalTable: "Quiz",
                principalColumn: "quizID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Word_AspNetUsers_UserID",
                table: "Word",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
