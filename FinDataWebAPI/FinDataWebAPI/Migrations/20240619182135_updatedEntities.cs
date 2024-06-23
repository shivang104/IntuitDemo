using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinDataWebAPI.Migrations
{
    public partial class updatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metric1",
                table: "FinancialData");

            migrationBuilder.DropColumn(
                name: "metric2",
                table: "FinancialData");

            migrationBuilder.DropColumn(
                name: "metric3",
                table: "FinancialData");

            migrationBuilder.DropColumn(
                name: "metric4",
                table: "FinancialData");

            migrationBuilder.DropColumn(
                name: "metric5",
                table: "FinancialData");

            migrationBuilder.RenameColumn(
                name: "metric6",
                table: "FinancialData",
                newName: "finMetrics_Capacity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "finMetrics_Capacity",
                table: "FinancialData",
                newName: "metric6");

            migrationBuilder.AddColumn<double>(
                name: "metric1",
                table: "FinancialData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "metric2",
                table: "FinancialData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "metric3",
                table: "FinancialData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "metric4",
                table: "FinancialData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "metric5",
                table: "FinancialData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
