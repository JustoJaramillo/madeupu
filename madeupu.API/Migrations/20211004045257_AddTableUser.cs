using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace madeupu.API.Migrations
{
    public partial class AddTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_documentTypes",
                table: "documentTypes");

            migrationBuilder.RenameTable(
                name: "documentTypes",
                newName: "DocumentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_documentTypes_Description",
                table: "DocumentTypes",
                newName: "IX_DocumentTypes_Description");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Document",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DocumentTypeId",
                table: "AspNetUsers",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DocumentTypes_DocumentTypeId",
                table: "AspNetUsers",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DocumentTypes_DocumentTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentTypes",
                table: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DocumentTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Document",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "DocumentTypes",
                newName: "documentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentTypes_Description",
                table: "documentTypes",
                newName: "IX_documentTypes_Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_documentTypes",
                table: "documentTypes",
                column: "Id");
        }
    }
}
