using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusShuttleDriver.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSessionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Sessions_SessionId1",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_SessionId1",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "SessionId1",
                table: "Buses");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_SessionId",
                table: "Buses",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Sessions_SessionId",
                table: "Buses",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Sessions_SessionId",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_SessionId",
                table: "Buses");

            migrationBuilder.AddColumn<int>(
                name: "SessionId1",
                table: "Buses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Accounts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_SessionId1",
                table: "Buses",
                column: "SessionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Sessions_SessionId1",
                table: "Buses",
                column: "SessionId1",
                principalTable: "Sessions",
                principalColumn: "Id");
        }
    }
}
