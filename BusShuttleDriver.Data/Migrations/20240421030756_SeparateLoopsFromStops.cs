using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparateLoopsFromStops : Migration
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

            migrationBuilder.DropColumn(
                name: "LoopId",
                table: "Stops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Stops",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_LoopId",
                table: "Stops",
                column: "LoopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Loops_LoopId",
                table: "Stops",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id");
        }
    }
}
