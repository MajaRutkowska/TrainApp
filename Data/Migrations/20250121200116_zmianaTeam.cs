using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class zmianaTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Team");

            migrationBuilder.RenameColumn(
                name: "CreationYear",
                table: "Team",
                newName: "PlayersBirthYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayersBirthYear",
                table: "Team",
                newName: "CreationYear");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Team",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
