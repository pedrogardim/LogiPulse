using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_destiny_facility_id",
                table: "dispatches");

            migrationBuilder.RenameColumn(
                name: "destiny_facility_id",
                table: "dispatches",
                newName: "destination_facility_id");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_destiny_facility_id",
                table: "dispatches",
                newName: "ix_dispatches_destination_facility_id");

            migrationBuilder.AddColumn<Guid>(
                name: "vehicle_id",
                table: "dispatches",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "vehicle_id1",
                table: "dispatches",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    license_plate = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    home_facility_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    capabilities = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_facilities_home_facility_id",
                        column: x => x.home_facility_id,
                        principalTable: "facilities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_vehicles_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dispatches_current_status",
                table: "dispatches",
                column: "current_status");

            migrationBuilder.CreateIndex(
                name: "ix_dispatches_vehicle_id",
                table: "dispatches",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatches_vehicle_id1",
                table: "dispatches",
                column: "vehicle_id1");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_external_id_tenant_id",
                table: "vehicles",
                columns: new[] { "external_id", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_home_facility_id",
                table: "vehicles",
                column: "home_facility_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_tenant_id",
                table: "vehicles",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_facilities_destination_facility_id",
                table: "dispatches",
                column: "destination_facility_id",
                principalTable: "facilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_vehicles_vehicle_id",
                table: "dispatches",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_vehicles_vehicle_id1",
                table: "dispatches",
                column: "vehicle_id1",
                principalTable: "vehicles",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_destination_facility_id",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_vehicles_vehicle_id",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_vehicles_vehicle_id1",
                table: "dispatches");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropIndex(
                name: "ix_dispatches_current_status",
                table: "dispatches");

            migrationBuilder.DropIndex(
                name: "ix_dispatches_vehicle_id",
                table: "dispatches");

            migrationBuilder.DropIndex(
                name: "ix_dispatches_vehicle_id1",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "vehicle_id",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "vehicle_id1",
                table: "dispatches");

            migrationBuilder.RenameColumn(
                name: "destination_facility_id",
                table: "dispatches",
                newName: "destiny_facility_id");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_destination_facility_id",
                table: "dispatches",
                newName: "ix_dispatches_destiny_facility_id");

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_facilities_destiny_facility_id",
                table: "dispatches",
                column: "destiny_facility_id",
                principalTable: "facilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
