using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EInvoice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateItemAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SroItemSerialNo",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "SroScheduleNo",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "TotalValues",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueSalesExcludingST",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalesTaxWithheldAtSource",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SalesTaxApplicable",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "FurtherTax",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FixedNotifiedValueOrRetailPrice",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "FedPayable",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtraTax",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "InvoiceItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalValue",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HsCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UoM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SroScheduleNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SroItemSerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxRate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: false),
                    IsFixedNotified = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ProductId",
                table: "InvoiceItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_OrganizationId",
                table: "Product",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Product_ProductId",
                table: "InvoiceItems",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Product_ProductId",
                table: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_ProductId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<double>(
                name: "ValueSalesExcludingST",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "SalesTaxWithheldAtSource",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "SalesTaxApplicable",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Rate",
                table: "InvoiceItems",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "FurtherTax",
                table: "InvoiceItems",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FixedNotifiedValueOrRetailPrice",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "FedPayable",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "ExtraTax",
                table: "InvoiceItems",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "SroItemSerialNo",
                table: "InvoiceItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SroScheduleNo",
                table: "InvoiceItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TotalValues",
                table: "InvoiceItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
