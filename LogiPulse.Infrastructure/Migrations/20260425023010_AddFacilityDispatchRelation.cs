using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFacilityDispatchRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_facility_id",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_facility_id1",
                table: "dispatches");

            migrationBuilder.RenameColumn(
                name: "facility_id1",
                table: "dispatches",
                newName: "origin_facility_id");

            migrationBuilder.RenameColumn(
                name: "facility_id",
                table: "dispatches",
                newName: "destiny_facility_id");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_facility_id1",
                table: "dispatches",
                newName: "ix_dispatches_origin_facility_id");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_facility_id",
                table: "dispatches",
                newName: "ix_dispatches_destiny_facility_id");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "facilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_facilities_destiny_facility_id",
                table: "dispatches",
                column: "destiny_facility_id",
                principalTable: "facilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_dispatches_facilities_origin_facility_id",
                table: "dispatches",
                column: "origin_facility_id",
                principalTable: "facilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_destiny_facility_id",
                table: "dispatches");

            migrationBuilder.DropForeignKey(
                name: "fk_dispatches_facilities_origin_facility_id",
                table: "dispatches");

            migrationBuilder.DropColumn(
                name: "type",
                table: "facilities");

            migrationBuilder.RenameColumn(
                name: "origin_facility_id",
                table: "dispatches",
                newName: "facility_id1");

            migrationBuilder.RenameColumn(
                name: "destiny_facility_id",
                table: "dispatches",
                newName: "facility_id");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_origin_facility_id",
                table: "dispatches",
                newName: "ix_dispatches_facility_id1");

            migrationBuilder.RenameIndex(
                name: "ix_dispatches_destiny_facility_id",
                table: "dispatches",
                newName: "ix_dispatches_facility_id");

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
    }
}
