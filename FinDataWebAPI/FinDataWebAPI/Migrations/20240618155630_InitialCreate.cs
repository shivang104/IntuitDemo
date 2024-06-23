using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinDataWebAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialData",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    metric1 = table.Column<double>(type: "float", nullable: false),
                    metric2 = table.Column<int>(type: "int", nullable: false),
                    metric3 = table.Column<double>(type: "float", nullable: false),
                    metric4 = table.Column<int>(type: "int", nullable: false),
                    metric5 = table.Column<double>(type: "float", nullable: false),
                    metric6 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialData", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialData");
        }
    }
}
