using Microsoft.EntityFrameworkCore.Migrations;

namespace madeupu.API.Migrations
{
    public partial class AddTableParticipation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipationTypeId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participations_ParticipationTypes_ParticipationTypeId",
                        column: x => x.ParticipationTypeId,
                        principalTable: "ParticipationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participations_ParticipationTypeId",
                table: "Participations",
                column: "ParticipationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Cities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
