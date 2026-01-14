using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP2.DAL.Migrations
{
    /// <inheritdoc />
    public partial class resetpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeResetPasssword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetCodeExpiery",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeResetPasssword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetCodeExpiery",
                table: "Users");
        }
    }
}
