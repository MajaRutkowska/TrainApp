using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamExerciseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamId",
                table: "Exercise",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_TeamId",
                table: "Exercise",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Team_TeamId",
                table: "Exercise",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Team_TeamId",
                table: "Exercise");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_TeamId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Exercise");
        }
    }
}
