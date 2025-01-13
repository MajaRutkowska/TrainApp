using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addcolorstotraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Training",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Training");
        }
    }
}
