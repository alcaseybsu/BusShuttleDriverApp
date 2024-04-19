using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDriverIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Drivers",
                newName: "DriverId");

            migrationBuilder.AddColumn<int>(
                name: "ActiveRouteSessionId",
                table: "Drivers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RouteSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BusId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoopId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteSessions_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteSessions_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RouteSessions_Loops_LoopId",
                        column: x => x.LoopId,
                        principalTable: "Loops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ActiveRouteSessionId",
                table: "Drivers",
                column: "ActiveRouteSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteSessions_BusId",
                table: "RouteSessions",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteSessions_DriverId",
                table: "RouteSessions",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteSessions_LoopId",
                table: "RouteSessions",
                column: "LoopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_RouteSessions_ActiveRouteSessionId",
                table: "Drivers",
                column: "ActiveRouteSessionId",
                principalTable: "RouteSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_RouteSessions_ActiveRouteSessionId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "RouteSessions");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ActiveRouteSessionId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ActiveRouteSessionId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "Drivers",
                newName: "Id");
        }
    }
}
