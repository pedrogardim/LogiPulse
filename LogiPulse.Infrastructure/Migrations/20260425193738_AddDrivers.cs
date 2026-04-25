using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDrivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_vehicles_vehicle_id1",
                table: "dispatches");

            migrationBuilder.RenameColumn(
                name: "vehicle_id1",
                table: "dispatches",
                newName: "driver_id");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_vehicle_id1",
                table: "dispatches",
                newName: "ix_dispatches_driver_id");

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    license_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    license_expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drivers", x => x.id);
                    table.ForeignKey(
                        name: "fk_drivers_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_drivers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_drivers_external_id_tenant_id",
                table: "drivers",
                columns: new[] { "external_id", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_drivers_tenant_id_license_number",
                table: "drivers",
                columns: new[] { "tenant_id", "license_number" });

            migrationBuilder.CreateIndex(
                name: "ix_drivers_user_id",
                table: "drivers",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_drivers_driver_id",
                table: "dispatches",
                column: "driver_id",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_drivers_driver_id",
                table: "dispatches");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                table: "dispatches",
                newName: "vehicle_id1");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_driver_id",
                table: "dispatches",
                newName: "ix_dispatches_vehicle_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_vehicles_vehicle_id1",
                table: "dispatches",
                column: "vehicle_id1",
                principalTable: "vehicles",
                principalColumn: "id");
        }
    }
}
