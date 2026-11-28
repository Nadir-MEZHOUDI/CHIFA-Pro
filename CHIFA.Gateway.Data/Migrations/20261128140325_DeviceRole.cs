using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CHIFA.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeviceRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "pairing_codes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "owner");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "mobile_devices",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "owner");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "pairing_codes");

            migrationBuilder.DropColumn(
                name: "role",
                table: "mobile_devices");
        }
    }
}
