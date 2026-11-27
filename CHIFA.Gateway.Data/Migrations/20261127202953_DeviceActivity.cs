using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CHIFA.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeviceActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "device_activities",
                columns: table => new
                {
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<DateOnly>(type: "date", nullable: false),
                    pharmacy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requests = table.Column<int>(type: "integer", nullable: false),
                    active_minutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_device_activities", x => new { x.device_id, x.day });
                    table.ForeignKey(
                        name: "fk_device_activities_mobile_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "mobile_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_device_activities_day",
                table: "device_activities",
                column: "day");

            migrationBuilder.CreateIndex(
                name: "ix_device_activities_pharmacy_id_day",
                table: "device_activities",
                columns: new[] { "pharmacy_id", "day" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_activities");
        }
    }
}
