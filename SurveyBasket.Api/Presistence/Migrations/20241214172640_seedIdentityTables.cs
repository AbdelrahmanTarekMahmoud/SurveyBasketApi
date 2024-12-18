using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class seedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0193c565-3bd9-75bb-b95b-4fc96d452676", "0193c565-3bd9-75bb-b95b-4fcb99ca9be0", false, false, "Admin", "ADMIN" },
                    { "0193c565-3bd9-75bb-b95b-4fcadd48391a", "0193c565-3bd9-75bb-b95b-4fccd4b50282", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0193c565-3bd9-75bb-b95b-4fc79759d675", 0, "0193c565-3bd9-75bb-b95b-4fc83c765156", "Admin@surveybasket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEYBASKET.COM", "ADMIN@SURVEYBASKET.COM", "AQAAAAIAAYagAAAAEBTC4bF1qQSI3hPCuuo6jHa+eCsCWPSScGzIVycU4XOlXx/c+1Tbc1YYW2xVZ/GhYQ==", null, false, "8B2FA06948CA46779CECFBC251BC6460", false, "Admin@surveybasket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "Polls:Read", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 2, "Permissions", "Polls:Add", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 3, "Permissions", "Polls:Update", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 4, "Permissions", "Polls:Delete", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 5, "Permissions", "Questions:Read", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 6, "Permissions", "Questions:Add", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 7, "Permissions", "Questions:Update", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 8, "Permissions", "Users:Read", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 9, "Permissions", "Users:Add", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 10, "Permissions", "Users:Update", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 11, "Permissions", "Polls:Read", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 12, "Permissions", "Polls:Add", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 13, "Permissions", "Polls:Update", "0193c565-3bd9-75bb-b95b-4fc96d452676" },
                    { 14, "Permissions", "Results:Read", "0193c565-3bd9-75bb-b95b-4fc96d452676" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0193c565-3bd9-75bb-b95b-4fc96d452676", "0193c565-3bd9-75bb-b95b-4fc79759d675" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fcadd48391a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0193c565-3bd9-75bb-b95b-4fc96d452676", "0193c565-3bd9-75bb-b95b-4fc79759d675" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fc96d452676");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fc79759d675");
        }
    }
}
