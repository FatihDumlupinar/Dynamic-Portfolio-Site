using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicPortfolioSite.Repository.Migrations
{
    public partial class ChangeAboutTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AboutId",
                table: "Works",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "Works",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AboutId",
                table: "Skills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AboutId",
                table: "Educations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "Educations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "AboutId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "AboutId",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "Educations");
        }
    }
}
