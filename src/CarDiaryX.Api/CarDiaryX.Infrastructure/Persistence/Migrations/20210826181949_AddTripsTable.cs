using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddTripsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(maxLength: 20, nullable: false),
                    DepartureDate = table.Column<DateTimeOffset>(nullable: false),
                    DepartureAddress = table.Column<string>(maxLength: 100, nullable: true),
                    DepartureAddressWithinDenmark = table.Column<string>(maxLength: 150, nullable: true),
                    ArrivalDate = table.Column<DateTimeOffset>(nullable: false),
                    ArrivalAddress = table.Column<string>(maxLength: 100, nullable: true),
                    ArrivalAddressWithinDenmark = table.Column<string>(maxLength: 150, nullable: true),
                    Distance = table.Column<int>(nullable: true),
                    Cost = table.Column<decimal>(nullable: true),
                    Note = table.Column<string>(maxLength: 250, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
