using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MermerSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentEn",
                table: "ProjectItems");

            migrationBuilder.DropColumn(
                name: "ContentNl",
                table: "ProjectItems");

            migrationBuilder.RenameColumn(
                name: "ContentTr",
                table: "ProjectItems",
                newName: "Country");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "ProjectItems",
                newName: "ContentTr");

            migrationBuilder.AddColumn<string>(
                name: "ContentEn",
                table: "ProjectItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentNl",
                table: "ProjectItems",
                type: "TEXT",
                nullable: true);
        }
    }
}
