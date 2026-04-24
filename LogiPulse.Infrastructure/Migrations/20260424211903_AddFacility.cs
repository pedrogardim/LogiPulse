using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace LogiPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "facility_id",
                table: "dispatches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "facility_id1",
                table: "dispatches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "facilities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<Point>(type: "geography(Point, 4326)", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: false),
                    address_number = table.Column<string>(type: "text", nullable: false),
                    address_complement = table.Column<string>(type: "text", nullable: false),
                    address_city = table.Column<string>(type: "text", nullable: false),
                    address_state = table.Column<string>(type: "text", nullable: false),
                    address_zip_code = table.Column<string>(type: "text", nullable: false),
                    address_country = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_facilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_facilities_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dispatches_facility_id",
                table: "dispatches",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispatches_facility_id1",
                table: "dispatches",
                column: "facility_id1");

            migrationBuilder.CreateIndex(
                name: "ix_facilities_code_tenant_id",
                table: "facilities",
                columns: new[] { "code", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_facilities_external_id_tenant_id",
                table: "facilities",
                columns: new[] { "external_id", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_facilities_tenant_id",
                table: "facilities",
                column: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_facilities_facility_id",
                table: "dispatches",
                column: "facility_id",
                principalTable: "facilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_facilities_facility_id1",
                table: "dispatches",
                column: "facility_id1",
                principalTable: "facilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_facility_id",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_facility_id1",
                table: "dispatches");

            migrationBuilder.DropTable(
                name: "facilities");

            migrationBuilder.DropIndex(
                name: "ix_dispatches_facility_id",
                table: "dispatches");

            migrationBuilder.DropIndex(
                name: "ix_dispatches_facility_id1",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "facility_id",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "facility_id1",
                table: "dispatches");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
