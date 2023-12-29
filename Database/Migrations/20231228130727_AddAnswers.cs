using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Answer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerId = table.Column<long>(type: "bigint", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerEntity_answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerEntity_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "answers",
                columns: new[] { "Id", "Answer" },
                values: new object[,]
                {
                    { 1L, "No, FU" },
                    { 2L, "MaybeMaybe" }
                });

            migrationBuilder.InsertData(
                table: "QuestionAnswerEntity",
                columns: new[] { "Id", "AnswerId", "QuestionId", "Rank" },
                values: new object[,]
                {
                    { 1L, 1L, 1L, 2 },
                    { 2L, 1L, 1L, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerEntity_AnswerId",
                table: "QuestionAnswerEntity",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerEntity_QuestionId",
                table: "QuestionAnswerEntity",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionAnswerEntity");

            migrationBuilder.DropTable(
                name: "answers");
        }
    }
}
