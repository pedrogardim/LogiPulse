using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "dispatches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "dispatches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TaxCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_TenantId",
                table: "products",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_products_TenantId1",
                table: "products",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_TenantId",
                table: "product_categories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_dispatches_ProductId",
                table: "dispatches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_dispatches_TenantId",
                table: "dispatches",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_dispatches_TenantId1",
                table: "dispatches",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_tenants_TaxCode",
                table: "tenants",
                column: "TaxCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_dispatches_products_ProductId",
                table: "dispatches",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dispatches_tenants_TenantId",
                table: "dispatches",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dispatches_tenants_TenantId1",
                table: "dispatches",
                column: "TenantId1",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_categories_tenants_TenantId",
                table: "product_categories",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_products_tenants_TenantId",
                table: "products",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_products_tenants_TenantId1",
                table: "products",
                column: "TenantId1",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dispatches_products_ProductId",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_dispatches_tenants_TenantId",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_dispatches_tenants_TenantId1",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_product_categories_tenants_TenantId",
                table: "product_categories");

            migrationBuilder.DropForeignKey(
                name: "FK_products_tenants_TenantId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_tenants_TenantId1",
                table: "products");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropIndex(
                name: "IX_products_TenantId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_TenantId1",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_product_categories_TenantId",
                table: "product_categories");

            migrationBuilder.DropIndex(
                name: "IX_dispatches_ProductId",
                table: "dispatches");

            migrationBuilder.DropIndex(
                name: "IX_dispatches_TenantId",
                table: "dispatches");

            migrationBuilder.DropIndex(
                name: "IX_dispatches_TenantId1",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "dispatches");
        }
    }
}
