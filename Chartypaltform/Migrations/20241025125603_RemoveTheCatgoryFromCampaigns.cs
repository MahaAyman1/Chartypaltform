using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chartypaltform.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTheCatgoryFromCampaigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_Campaigns_CampaignId",
                table: "categories");

            migrationBuilder.DropTable(
                name: "TUI");

            migrationBuilder.DropIndex(
                name: "IX_categories_CampaignId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TUI",
                columns: table => new
                {
                    TUIID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    TUIName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUI", x => x.TUIID);
                    table.ForeignKey(
                        name: "FK_TUI_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_CampaignId",
                table: "categories",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TUI_CampaignId",
                table: "TUI",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_Campaigns_CampaignId",
                table: "categories",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "CampaignId");
        }
    }
}
