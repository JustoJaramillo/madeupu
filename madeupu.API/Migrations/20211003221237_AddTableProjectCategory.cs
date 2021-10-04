using Microsoft.EntityFrameworkCore.Migrations;

namespace madeupu.API.Migrations
{
    public partial class AddTableProjectCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCategories_Description",
                table: "ProjectCategories",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectCategories");
        }
    }
}
