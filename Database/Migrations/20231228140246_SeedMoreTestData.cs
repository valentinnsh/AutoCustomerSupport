using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "QuestionAnswerEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "AnswerId",
                value: 2L);

            migrationBuilder.InsertData(
                table: "answers",
                columns: new[] { "Id", "Answer" },
                values: new object[,]
                {
                    { 3L, "MoreAnswers" },
                    { 4L, "MoreAnswers 2" }
                });

            migrationBuilder.InsertData(
                table: "questions",
                columns: new[] { "Id", "Question" },
                values: new object[] { 2L, "Will I finish in time?" });

            migrationBuilder.InsertData(
                table: "QuestionAnswerEntity",
                columns: new[] { "Id", "AnswerId", "QuestionId", "Rank" },
                values: new object[,]
                {
                    { 3L, 2L, 2L, 2 },
                    { 4L, 4L, 2L, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuestionAnswerEntity",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "QuestionAnswerEntity",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "answers",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "answers",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.UpdateData(
                table: "QuestionAnswerEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "AnswerId",
                value: 1L);
        }
    }
}
