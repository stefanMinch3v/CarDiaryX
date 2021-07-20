using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedVehicleTablesAndUserRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegistrationNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 20, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDMRs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(maxLength: 20, nullable: false),
                    NextInspectionDate = table.Column<DateTime>(nullable: true),
                    NextGreenTaxDate = table.Column<DateTime>(nullable: true),
                    JsonData = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDMRs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInformations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(maxLength: 20, nullable: false),
                    DataTsId = table.Column<long>(nullable: false),
                    DataId = table.Column<long>(nullable: false),
                    JsonData = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInformations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInspections",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(maxLength: 20, nullable: false),
                    JsonData = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInspections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRegistrationNumbers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RegistrationNumberId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegistrationNumbers", x => new { x.UserId, x.RegistrationNumberId });
                    table.ForeignKey(
                        name: "FK_UserRegistrationNumbers_RegistrationNumbers_RegistrationNumberId",
                        column: x => x.RegistrationNumberId,
                        principalTable: "RegistrationNumbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRegistrationNumbers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationNumbers_Number",
                table: "RegistrationNumbers",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRegistrationNumbers_RegistrationNumberId",
                table: "UserRegistrationNumbers",
                column: "RegistrationNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDMRs_RegistrationNumber",
                table: "VehicleDMRs",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInformations_RegistrationNumber",
                table: "VehicleInformations",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspections_RegistrationNumber",
                table: "VehicleInspections",
                column: "RegistrationNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRegistrationNumbers");

            migrationBuilder.DropTable(
                name: "VehicleDMRs");

            migrationBuilder.DropTable(
                name: "VehicleInformations");

            migrationBuilder.DropTable(
                name: "VehicleInspections");

            migrationBuilder.DropTable(
                name: "RegistrationNumbers");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "AspNetUsers");
        }
    }
}
