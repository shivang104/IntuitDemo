using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinDataWebAPI.Migrations
{
    public partial class updatedEntitiesFinMetrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "finMetrics_Capacity",
                table: "FinancialData");

            migrationBuilder.CreateTable(
                name: "FinMetrics",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    metric1 = table.Column<double>(type: "float", nullable: false),
                    metric2 = table.Column<int>(type: "int", nullable: false),
                    metric3 = table.Column<double>(type: "float", nullable: false),
                    metric4 = table.Column<int>(type: "int", nullable: false),
                    metric5 = table.Column<double>(type: "float", nullable: false),
                    metric6 = table.Column<int>(type: "int", nullable: false),
                    financialDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinMetrics", x => x.id);
                    table.ForeignKey(
                        name: "FK_FinMetrics_FinancialData_financialDataId",
                        column: x => x.financialDataId,
                        principalTable: "FinancialData",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinMetrics_financialDataId",
                table: "FinMetrics",
                column: "financialDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinMetrics");

            migrationBuilder.AddColumn<int>(
                name: "finMetrics_Capacity",
                table: "FinancialData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
