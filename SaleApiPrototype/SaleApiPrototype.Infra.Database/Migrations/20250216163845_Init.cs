using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleApiPrototype.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhenWasMade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleNumber);
                });

            migrationBuilder.CreateTable(
                name: "SaleProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleNumber = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(7,5)", precision: 7, scale: 5, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(38,2)", precision: 38, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleProducts_Sales_SaleNumber",
                        column: x => x.SaleNumber,
                        principalTable: "Sales",
                        principalColumn: "SaleNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleProducts_SaleNumber_ProductName",
                table: "SaleProducts",
                columns: new[] { "SaleNumber", "ProductName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleProducts");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
