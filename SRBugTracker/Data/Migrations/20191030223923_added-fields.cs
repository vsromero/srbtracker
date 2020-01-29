using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SRBugTracker.Migrations
{
    public partial class addedfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "Issue",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Issue",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Issue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Issue",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_LastModifiedById",
                table: "Project",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_LastModifiedById",
                table: "Issue",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ProjectId",
                table: "Issue",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Project_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_AspNetUsers_LastModifiedById",
                table: "Issue",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Project_ProjectId",
                table: "Issue",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_LastModifiedById",
                table: "Project",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Project_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_AspNetUsers_LastModifiedById",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Project_ProjectId",
                table: "Issue");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_LastModifiedById",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_LastModifiedById",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Issue_LastModifiedById",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_ProjectId",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AspNetUsers");
        }
    }
}
