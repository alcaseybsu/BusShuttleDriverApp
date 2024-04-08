using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumLeft",
                table: "Entries",
                newName: "LeftBehind");

            migrationBuilder.RenameColumn(
                name: "NumBoarded",
                table: "Entries",
                newName: "Boarded");

            migrationBuilder.AddColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteName",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "LeftBehind",
                table: "Entries",
                newName: "NumLeft");

            migrationBuilder.RenameColumn(
                name: "Boarded",
                table: "Entries",
                newName: "NumBoarded");
        }
    }
}
