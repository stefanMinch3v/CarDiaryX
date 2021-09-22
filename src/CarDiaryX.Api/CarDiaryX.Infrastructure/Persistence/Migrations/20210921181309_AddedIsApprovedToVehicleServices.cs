using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedIsApprovedToVehicleServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "VehicleServices",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "VehicleServices");
        }
    }
}
