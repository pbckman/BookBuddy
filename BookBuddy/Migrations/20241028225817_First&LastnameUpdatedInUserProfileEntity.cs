using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddy.Migrations
{
    /// <inheritdoc />
    public partial class FirstLastnameUpdatedInUserProfileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Profiles",
                newName: "ProfileLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Profiles",
                newName: "ProfileFirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileLastName",
                table: "Profiles",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "ProfileFirstName",
                table: "Profiles",
                newName: "FirstName");
        }
    }
}
