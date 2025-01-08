using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AktualizacjiaPotrzebnaDoZadan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Exercise");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "UserExercise",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "UserExercise");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Exercise",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
