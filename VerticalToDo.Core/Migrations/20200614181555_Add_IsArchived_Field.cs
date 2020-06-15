using Microsoft.EntityFrameworkCore.Migrations;

namespace VerticalToDo.Core.Migrations
{
    public partial class Add_IsArchived_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "ToDos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "ToDos");
        }
    }
}
