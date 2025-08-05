using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EInvoice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaxRateToDecimalInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                table: "Products",
                type: "decimal(18,2)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaxRate",
                table: "Products",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 10);
        }
    }
}
