using Microsoft.EntityFrameworkCore.Migrations;

namespace SRBugTracker.Migrations
{
    public partial class issuestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Issue",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Issue",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
