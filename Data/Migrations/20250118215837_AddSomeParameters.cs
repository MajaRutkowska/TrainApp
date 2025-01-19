using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Dribble",
                table: "Parameters",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Endurance",
                table: "Parameters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Flexibility",
                table: "Parameters",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HighJump",
                table: "Parameters",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "LegStrength",
                table: "Parameters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "ShotPower",
                table: "Parameters",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dribble",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Endurance",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Flexibility",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "HighJump",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "LegStrength",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ShotPower",
                table: "Parameters");
        }
    }
}
