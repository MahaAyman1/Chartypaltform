using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class Volunteering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharityOrganizationId",
                table: "Success_Story",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CharityOrganizationId1",
                table: "Success_Story",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Volunteerings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    DonorId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteerings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Volunteerings_AspNetUsers_DonorId1",
                        column: x => x.DonorId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteeringTaskSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VolunteeringId = table.Column<int>(type: "int", nullable: false),
                    TaskDescription = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteeringTaskSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteeringTaskSelections_Volunteerings_VolunteeringId",
                        column: x => x.VolunteeringId,
                        principalTable: "Volunteerings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Success_Story_CharityOrganizationId1",
                table: "Success_Story",
                column: "CharityOrganizationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteerings_DonorId1",
                table: "Volunteerings",
                column: "DonorId1");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteeringTaskSelections_VolunteeringId",
                table: "VolunteeringTaskSelections",
                column: "VolunteeringId");

            migrationBuilder.AddForeignKey(
                name: "FK_Success_Story_AspNetUsers_CharityOrganizationId1",
                table: "Success_Story",
                column: "CharityOrganizationId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Success_Story_AspNetUsers_CharityOrganizationId1",
                table: "Success_Story");

            migrationBuilder.DropTable(
                name: "VolunteeringTaskSelections");

            migrationBuilder.DropTable(
                name: "Volunteerings");

            migrationBuilder.DropIndex(
                name: "IX_Success_Story_CharityOrganizationId1",
                table: "Success_Story");

            migrationBuilder.DropColumn(
                name: "CharityOrganizationId",
                table: "Success_Story");

            migrationBuilder.DropColumn(
                name: "CharityOrganizationId1",
                table: "Success_Story");
        }
    }
}
