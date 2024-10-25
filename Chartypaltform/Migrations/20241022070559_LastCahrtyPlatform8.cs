using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class LastCahrtyPlatform8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminActions_adminUsers_AdminUserId",
                table: "AdminActions");

            migrationBuilder.DropTable(
                name: "adminUsers");

            migrationBuilder.AddColumn<string>(
                name: "AdminFullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminActions_AspNetUsers_AdminUserId",
                table: "AdminActions",
                column: "AdminUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminActions_AspNetUsers_AdminUserId",
                table: "AdminActions");

            migrationBuilder.DropColumn(
                name: "AdminFullName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "adminUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdminFullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adminUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adminUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adminUsers_UserId",
                table: "adminUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminActions_adminUsers_AdminUserId",
                table: "AdminActions",
                column: "AdminUserId",
                principalTable: "adminUsers",
                principalColumn: "Id");
        }
    }
}
