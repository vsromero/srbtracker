using Microsoft.EntityFrameworkCore.Migrations;

namespace SRBugTracker.Migrations
{
    public partial class changedcreatedby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_AspNetUsers_OwnerId",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_OwnerId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Issue_OwnerId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Issue");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Project",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                newName: "IX_Project_CreatedById");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Issue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issue_CreatedById",
                table: "Issue",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedById",
                table: "Comment",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_CreatedById",
                table: "Comment",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_AspNetUsers_CreatedById",
                table: "Issue",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_CreatedById",
                table: "Project",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_CreatedById",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_AspNetUsers_CreatedById",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_CreatedById",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Issue_CreatedById",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Comment_CreatedById",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Project",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_CreatedById",
                table: "Project",
                newName: "IX_Project_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Issue",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_OwnerId",
                table: "Issue",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_AspNetUsers_OwnerId",
                table: "Issue",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_OwnerId",
                table: "Project",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
