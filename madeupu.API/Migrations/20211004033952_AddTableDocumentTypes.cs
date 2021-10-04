using Microsoft.EntityFrameworkCore.Migrations;

namespace madeupu.API.Migrations
{
    public partial class AddTableDocumentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_participationTypes",
                table: "participationTypes");

            migrationBuilder.RenameTable(
                name: "participationTypes",
                newName: "ParticipationTypes");

            migrationBuilder.RenameIndex(
                name: "IX_participationTypes_Description",
                table: "ParticipationTypes",
                newName: "IX_ParticipationTypes_Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParticipationTypes",
                table: "ParticipationTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "documentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_documentTypes_Description",
                table: "documentTypes",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParticipationTypes",
                table: "ParticipationTypes");

            migrationBuilder.RenameTable(
                name: "ParticipationTypes",
                newName: "participationTypes");

            migrationBuilder.RenameIndex(
                name: "IX_ParticipationTypes_Description",
                table: "participationTypes",
                newName: "IX_participationTypes_Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_participationTypes",
                table: "participationTypes",
                column: "Id");
        }
    }
}
