using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class EditPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "ClaimValue",
                value: "Roles:Read");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "ClaimValue",
                value: "Roles:Add");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "ClaimValue",
                value: "Roles:Update");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fc79759d675",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGGZTV7U1QQUo+X+/oXbm/J4dl58ATg+YosTf9MnoeztNXtK5fqew3wUu9EhMnn9Dg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "ClaimValue",
                value: "Polls:Read");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "ClaimValue",
                value: "Polls:Add");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "ClaimValue",
                value: "Polls:Update");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fc79759d675",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBTC4bF1qQSI3hPCuuo6jHa+eCsCWPSScGzIVycU4XOlXx/c+1Tbc1YYW2xVZ/GhYQ==");
        }
    }
}
