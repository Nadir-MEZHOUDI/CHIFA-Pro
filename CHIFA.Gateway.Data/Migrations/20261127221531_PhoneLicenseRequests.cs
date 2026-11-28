using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CHIFA.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhoneLicenseRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "license_request_id",
                table: "pharmacies",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "license_request_token",
                table: "pharmacies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "license_requested_at",
                table: "pharmacies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "machine_id",
                table: "pharmacies",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "license_request_id",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "license_request_token",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "license_requested_at",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "machine_id",
                table: "pharmacies");
        }
    }
}
