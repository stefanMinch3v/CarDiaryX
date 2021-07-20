using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedIAuditableToRegistrationNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "RegistrationNumbers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RegistrationNumbers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RegistrationNumbers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RegistrationNumbers");
        }
    }
}
