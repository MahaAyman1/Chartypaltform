using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class Volunteering2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Volunteerings_AspNetUsers_DonorId1",
                table: "Volunteerings");

            migrationBuilder.DropIndex(
                name: "IX_Volunteerings_DonorId1",
                table: "Volunteerings");

            migrationBuilder.DropColumn(
                name: "DonorId1",
                table: "Volunteerings");

            migrationBuilder.AlterColumn<string>(
                name: "DonorId",
                table: "Volunteerings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Volunteerings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteerings_DonorId",
                table: "Volunteerings",
                column: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteerings_AspNetUsers_DonorId",
                table: "Volunteerings",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Volunteerings_AspNetUsers_DonorId",
                table: "Volunteerings");

            migrationBuilder.DropIndex(
                name: "IX_Volunteerings_DonorId",
                table: "Volunteerings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Volunteerings");

            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "Volunteerings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "DonorId1",
                table: "Volunteerings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteerings_DonorId1",
                table: "Volunteerings",
                column: "DonorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteerings_AspNetUsers_DonorId1",
                table: "Volunteerings",
                column: "DonorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
