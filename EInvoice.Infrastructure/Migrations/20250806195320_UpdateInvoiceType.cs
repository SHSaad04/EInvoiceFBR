using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EInvoice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "buyerRegistrationType",
                table: "Invoices",
                newName: "BuyerRegistrationType");

            migrationBuilder.CreateTable(
                name: "InvoiceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTypes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTypes");

            migrationBuilder.RenameColumn(
                name: "BuyerRegistrationType",
                table: "Invoices",
                newName: "buyerRegistrationType");
        }
    }
}
