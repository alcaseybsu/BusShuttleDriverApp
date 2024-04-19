using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeLoopIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Loops_LoopId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Routes");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Stops",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "LoopId",
                table: "Routes",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Loops_LoopId",
                table: "Routes",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Loops_LoopId",
                table: "Routes");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Stops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LoopId",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Loops_LoopId",
                table: "Routes",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
