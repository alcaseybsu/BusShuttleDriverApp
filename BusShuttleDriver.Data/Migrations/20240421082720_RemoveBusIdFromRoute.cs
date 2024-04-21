using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBusIdFromRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Buses_BusId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_BusId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Routes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Routes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_BusId",
                table: "Routes",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Buses_BusId",
                table: "Routes",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId");
        }
    }
}
