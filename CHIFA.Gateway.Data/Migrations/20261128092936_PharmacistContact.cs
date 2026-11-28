using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CHIFA.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class PharmacistContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contact_address",
                table: "pharmacies",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_business_name",
                table: "pharmacies",
                type: "character varying(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_code_ps",
                table: "pharmacies",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_email",
                table: "pharmacies",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_name",
                table: "pharmacies",
                type: "character varying(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_phone",
                table: "pharmacies",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "contact_updated_at",
                table: "pharmacies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_wilaya",
                table: "pharmacies",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contact_address",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_business_name",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_code_ps",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_email",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_name",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_phone",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_updated_at",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "contact_wilaya",
                table: "pharmacies");
        }
    }
}
