using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SRBugTracker.Migrations
{
    public partial class modifiedcommentsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "Comment",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Comment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LastModifiedById",
                table: "Comment",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_LastModifiedById",
                table: "Comment",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_LastModifiedById",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_LastModifiedById",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Comment");
        }
    }
}
