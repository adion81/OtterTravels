using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OtterTravels.Migrations
{
    public partial class AnOtterMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Associations",
                columns: table => new
                {
                    AssociationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OtterId = table.Column<int>(nullable: false),
                    VacationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.AssociationId);
                    table.ForeignKey(
                        name: "FK_Associations_Otters_OtterId",
                        column: x => x.OtterId,
                        principalTable: "Otters",
                        principalColumn: "OtterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Associations_Vacations_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacations",
                        principalColumn: "VacationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Associations_OtterId",
                table: "Associations",
                column: "OtterId");

            migrationBuilder.CreateIndex(
                name: "IX_Associations_VacationId",
                table: "Associations",
                column: "VacationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Associations");
        }
    }
}
