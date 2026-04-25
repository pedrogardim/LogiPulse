using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductRuleToRequirement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rules",
                table: "products",
                newName: "requirements");

            migrationBuilder.RenameColumn(
                name: "rules",
                table: "product_categories",
                newName: "requirements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "requirements",
                table: "products",
                newName: "rules");

            migrationBuilder.RenameColumn(
                name: "requirements",
                table: "product_categories",
                newName: "rules");
        }
    }
}
