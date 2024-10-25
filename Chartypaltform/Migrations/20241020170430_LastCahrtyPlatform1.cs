using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class LastCahrtyPlatform1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TUIs_Campaigns_CampaignId",
                table: "TUIs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TUIs",
                table: "TUIs");

            migrationBuilder.DropColumn(
                name: "CharityOrganizationId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "TUIs",
                newName: "TUI");

            migrationBuilder.RenameIndex(
                name: "IX_TUIs_CampaignId",
                table: "TUI",
                newName: "IX_TUI_CampaignId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TUI",
                table: "TUI",
                column: "TUIID");

            migrationBuilder.CreateTable(
                name: "successCampaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    impact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_successCampaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_successCampaigns_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_successCampaigns_CampaignId",
                table: "successCampaigns",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_TUI_Campaigns_CampaignId",
                table: "TUI",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TUI_Campaigns_CampaignId",
                table: "TUI");

            migrationBuilder.DropTable(
                name: "successCampaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TUI",
                table: "TUI");

            migrationBuilder.RenameTable(
                name: "TUI",
                newName: "TUIs");

            migrationBuilder.RenameIndex(
                name: "IX_TUI_CampaignId",
                table: "TUIs",
                newName: "IX_TUIs_CampaignId");

            migrationBuilder.AddColumn<int>(
                name: "CharityOrganizationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TUIs",
                table: "TUIs",
                column: "TUIID");

            migrationBuilder.AddForeignKey(
                name: "FK_TUIs_Campaigns_CampaignId",
                table: "TUIs",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
