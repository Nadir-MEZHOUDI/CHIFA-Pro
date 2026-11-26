using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CHIFA.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubscriptionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "license_checked_at",
                table: "pharmacies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "license_edition",
                table: "pharmacies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "license_expires_at",
                table: "pharmacies",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "license_checked_at",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "license_edition",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "license_expires_at",
                table: "pharmacies");
        }
    }
}
