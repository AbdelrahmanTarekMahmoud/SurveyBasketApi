using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendTheUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fc79759d675",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEB6bXrSww+MSMmnCE1d22SqK9ui6qUycF2JJr9aw1uV8S9x4qSMrvytfDyVyyVdgPA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0193c565-3bd9-75bb-b95b-4fc79759d675",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGGZTV7U1QQUo+X+/oXbm/J4dl58ATg+YosTf9MnoeztNXtK5fqew3wUu9EhMnn9Dg==");
        }
    }
}
