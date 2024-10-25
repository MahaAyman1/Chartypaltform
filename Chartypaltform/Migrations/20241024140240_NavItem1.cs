using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class NavItem1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "navItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "navItems");
        }
    }
}
