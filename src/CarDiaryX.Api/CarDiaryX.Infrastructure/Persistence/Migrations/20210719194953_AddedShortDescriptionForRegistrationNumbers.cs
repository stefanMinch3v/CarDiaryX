using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDiaryX.Infrastructure.Common.Persistence.Migrations
{
    public partial class AddedShortDescriptionForRegistrationNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "RegistrationNumbers",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "RegistrationNumbers");
        }
    }
}
