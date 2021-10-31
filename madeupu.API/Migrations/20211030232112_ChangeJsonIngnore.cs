using Microsoft.EntityFrameworkCore.Migrations;

namespace madeupu.API.Migrations
{
    public partial class ChangeJsonIngnore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_ParticipationTypes_ParticipationTypeId",
                table: "Participations");

            migrationBuilder.AlterColumn<int>(
                name: "ParticipationTypeId",
                table: "Participations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_ParticipationTypes_ParticipationTypeId",
                table: "Participations",
                column: "ParticipationTypeId",
                principalTable: "ParticipationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_ParticipationTypes_ParticipationTypeId",
                table: "Participations");

            migrationBuilder.AlterColumn<int>(
                name: "ParticipationTypeId",
                table: "Participations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_ParticipationTypes_ParticipationTypeId",
                table: "Participations",
                column: "ParticipationTypeId",
                principalTable: "ParticipationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
