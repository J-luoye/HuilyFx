using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Web.Migrations
{
    public partial class Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faceUser_faceUser_FaceUserId",
                table: "faceUser");

            migrationBuilder.DropIndex(
                name: "IX_faceUser_FaceUserId",
                table: "faceUser");

            migrationBuilder.DropColumn(
                name: "FaceUserId",
                table: "faceUser");

            migrationBuilder.AlterColumn<string>(
                name: "faceid",
                table: "faceTab",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "addtime",
                table: "faceTab",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_faceUser_faceTabId",
                table: "faceUser",
                column: "faceTabId");

            migrationBuilder.AddForeignKey(
                name: "FK_faceUser_faceTab_faceTabId",
                table: "faceUser",
                column: "faceTabId",
                principalTable: "faceTab",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_faceUser_faceTab_faceTabId",
                table: "faceUser");

            migrationBuilder.DropIndex(
                name: "IX_faceUser_faceTabId",
                table: "faceUser");

            migrationBuilder.AddColumn<int>(
                name: "FaceUserId",
                table: "faceUser",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "faceid",
                table: "faceTab",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "addtime",
                table: "faceTab",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateIndex(
                name: "IX_faceUser_FaceUserId",
                table: "faceUser",
                column: "FaceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_faceUser_faceUser_FaceUserId",
                table: "faceUser",
                column: "FaceUserId",
                principalTable: "faceUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
