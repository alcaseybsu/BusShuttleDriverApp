using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixLoops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "RouteSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RouteSessions_RouteId",
                table: "RouteSessions",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteSessions_Routes_RouteId",
                table: "RouteSessions",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteSessions_Routes_RouteId",
                table: "RouteSessions");

            migrationBuilder.DropIndex(
                name: "IX_RouteSessions_RouteId",
                table: "RouteSessions");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "RouteSessions");
        }
    }
}
