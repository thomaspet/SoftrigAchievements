using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftrigAchievements.Migrations
{
    public partial class FixAchievementForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "UniEntity",
                table: "Achievements");

            migrationBuilder.AddColumn<int>(
                name: "AchievementId",
                table: "Achievements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_AchievementId",
                table: "Achievements",
                column: "AchievementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Achievement_AchievementId",
                table: "Achievements",
                column: "AchievementId",
                principalTable: "Achievement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Achievement_AchievementId",
                table: "Achievements");

            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_AchievementId",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "AchievementId",
                table: "Achievements");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniEntity",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
