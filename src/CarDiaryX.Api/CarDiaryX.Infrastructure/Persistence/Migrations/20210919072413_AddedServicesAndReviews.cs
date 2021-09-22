using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedServicesAndReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    Address = table.Column<string>(maxLength: 150, nullable: false),
                    AddressX = table.Column<string>(maxLength: 30, nullable: false),
                    AddressY = table.Column<string>(maxLength: 30, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Ratings = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    Prices = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    VehicleServiceId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_VehicleServices_VehicleServiceId",
                        column: x => x.VehicleServiceId,
                        principalTable: "VehicleServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_VehicleServiceId",
                table: "Reviews",
                column: "VehicleServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleServices_Address",
                table: "VehicleServices",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleServices_Name",
                table: "VehicleServices",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "VehicleServices");
        }
    }
}
