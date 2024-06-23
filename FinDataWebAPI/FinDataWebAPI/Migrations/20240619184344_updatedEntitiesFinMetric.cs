using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinDataWebAPI.Migrations
{
    public partial class updatedEntitiesFinMetric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinMetrics_FinancialData_financialDataId",
                table: "FinMetrics");

            migrationBuilder.RenameColumn(
                name: "financialDataId",
                table: "FinMetrics",
                newName: "financialDataid");

            migrationBuilder.RenameIndex(
                name: "IX_FinMetrics_financialDataId",
                table: "FinMetrics",
                newName: "IX_FinMetrics_financialDataid");

            migrationBuilder.AddForeignKey(
                name: "FK_FinMetrics_FinancialData_financialDataid",
                table: "FinMetrics",
                column: "financialDataid",
                principalTable: "FinancialData",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinMetrics_FinancialData_financialDataid",
                table: "FinMetrics");

            migrationBuilder.RenameColumn(
                name: "financialDataid",
                table: "FinMetrics",
                newName: "financialDataId");

            migrationBuilder.RenameIndex(
                name: "IX_FinMetrics_financialDataid",
                table: "FinMetrics",
                newName: "IX_FinMetrics_financialDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinMetrics_FinancialData_financialDataId",
                table: "FinMetrics",
                column: "financialDataId",
                principalTable: "FinancialData",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
