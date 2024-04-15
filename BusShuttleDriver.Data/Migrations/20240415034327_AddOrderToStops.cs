using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderToStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Loops_LoopId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_LoopId",
                table: "Stops");

            migrationBuilder.RenameColumn(
                name: "LoopId",
                table: "Stops",
                newName: "RouteId");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Stops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Loops",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_RouteId_Order",
                table: "Stops",
                columns: new[] { "RouteId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_BusId",
                table: "Routes",
                column: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Buses_BusId",
                table: "Routes",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Routes_RouteId",
                table: "Stops",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Buses_BusId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Routes_RouteId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_RouteId_Order",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Routes_BusId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Stops");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "Stops",
                newName: "LoopId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Loops",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Stops_LoopId",
                table: "Stops",
                column: "LoopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Loops_LoopId",
                table: "Stops",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
