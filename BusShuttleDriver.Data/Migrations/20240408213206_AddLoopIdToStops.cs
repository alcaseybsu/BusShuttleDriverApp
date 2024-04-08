using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLoopIdToStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Stops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_LoopId",
                table: "Stops",
                column: "LoopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Loops_LoopId",
                table: "Stops",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "LoopId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
