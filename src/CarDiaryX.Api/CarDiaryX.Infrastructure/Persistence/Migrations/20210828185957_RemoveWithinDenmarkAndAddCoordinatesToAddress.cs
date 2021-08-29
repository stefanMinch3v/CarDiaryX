using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class RemoveWithinDenmarkAndAddCoordinatesToAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalAddressWithinDenmark",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DepartureAddressWithinDenmark",
                table: "Trips");

            migrationBuilder.AlterColumn<string>(
                name: "DepartureAddress",
                table: "Trips",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArrivalAddress",
                table: "Trips",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAddressX",
                table: "Trips",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAddressY",
                table: "Trips",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureAddressX",
                table: "Trips",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureAddressY",
                table: "Trips",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalAddressX",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ArrivalAddressY",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DepartureAddressX",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DepartureAddressY",
                table: "Trips");

            migrationBuilder.AlterColumn<string>(
                name: "DepartureAddress",
                table: "Trips",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "ArrivalAddress",
                table: "Trips",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAddressWithinDenmark",
                table: "Trips",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureAddressWithinDenmark",
                table: "Trips",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
