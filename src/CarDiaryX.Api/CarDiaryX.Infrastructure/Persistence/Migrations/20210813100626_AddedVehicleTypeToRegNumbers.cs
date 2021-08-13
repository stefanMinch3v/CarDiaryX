using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedVehicleTypeToRegNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "RegistrationNumbers",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "RegistrationNumbers");
        }
    }
}
