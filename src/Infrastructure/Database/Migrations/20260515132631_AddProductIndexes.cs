using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddProductIndexes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "ix_products_category",
            table: "products",
            column: "category");

        migrationBuilder.CreateIndex(
            name: "ix_products_category_price",
            table: "products",
            columns: ["category", "price"]);

        migrationBuilder.CreateIndex(
            name: "ix_products_name",
            table: "products",
            column: "name");

        migrationBuilder.CreateIndex(
            name: "ix_products_price",
            table: "products",
            column: "price");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_products_category",
            table: "products");

        migrationBuilder.DropIndex(
            name: "ix_products_category_price",
            table: "products");

        migrationBuilder.DropIndex(
            name: "ix_products_name",
            table: "products");

        migrationBuilder.DropIndex(
            name: "ix_products_price",
            table: "products");
    }
}
