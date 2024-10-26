using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class Donation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_UserId",
                table: "Campaigns");

            migrationBuilder.CreateTable(
                name: "donations",
                columns: table => new
                {
                    DonationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_donations", x => x.DonationId);
                    table.ForeignKey(
                        name: "FK_donations_AspNetUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_donations_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_donations_CampaignId",
                table: "donations",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_donations_DonorId",
                table: "donations",
                column: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_UserId",
                table: "Campaigns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_UserId",
                table: "Campaigns");

            migrationBuilder.DropTable(
                name: "donations");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_UserId",
                table: "Campaigns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
