using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "QuizResults",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "QuizResults");
        }
    }
}
