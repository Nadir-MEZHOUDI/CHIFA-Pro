using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CHIFA.Gateway.Data.Migrations
{
    /// <inheritdoc />
    public partial class PushNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "push_subscriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pharmacy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    endpoint = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    p256dh = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    auth = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_push_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_push_subscriptions_mobile_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "mobile_devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_push_subscriptions_pharmacies_pharmacy_id",
                        column: x => x.pharmacy_id,
                        principalTable: "pharmacies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vapid_keys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    public_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    private_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vapid_keys", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_push_subscriptions_device_id",
                table: "push_subscriptions",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_push_subscriptions_endpoint",
                table: "push_subscriptions",
                column: "endpoint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_push_subscriptions_pharmacy_id",
                table: "push_subscriptions",
                column: "pharmacy_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "push_subscriptions");

            migrationBuilder.DropTable(
                name: "vapid_keys");
        }
    }
}
