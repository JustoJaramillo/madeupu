using Microsoft.EntityFrameworkCore.Migrations;

namespace madeupu.API.Migrations
{
    public partial class updatingParticipationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Participations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Participations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Comments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Cities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_Id",
                table: "Participations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_ProjectId",
                table: "Participations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_UserId",
                table: "Participations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_AspNetUsers_UserId",
                table: "Participations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Projects_ProjectId",
                table: "Participations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_AspNetUsers_UserId",
                table: "Participations");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Projects_ProjectId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_Id",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_ProjectId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_UserId",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Participations");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Comments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
