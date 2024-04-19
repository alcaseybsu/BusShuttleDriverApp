using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixStopsOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Routes_RouteId",
                table: "Stops");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Routes_RouteId",
                table: "Stops",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Routes_RouteId",
                table: "Stops");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Routes_RouteId",
                table: "Stops",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
