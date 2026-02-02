using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MermerSitesi.Migrations
{
    /// <inheritdoc />
    public partial class AddDutchToProjectItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TitleTr",
                table: "ProjectItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentNl",
                table: "ProjectItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleNl",
                table: "ProjectItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentNl",
                table: "ProjectItems");

            migrationBuilder.DropColumn(
                name: "TitleNl",
                table: "ProjectItems");

            migrationBuilder.AlterColumn<string>(
                name: "TitleTr",
                table: "ProjectItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
