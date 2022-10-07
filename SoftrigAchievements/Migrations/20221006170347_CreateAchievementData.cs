using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftrigAchievements.Migrations
{
    public partial class CreateAchievementData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Achievements",
                columns: new[] { "Name", "Description", "AchievementType", "Count" },
                values: new object[,]
                    {
                        { "Første Faktura!", "Gratulerer! Du har opprettet din første fakura!", 1, 1 },
                        { "Faktura nr. 5 opprettet!", "Gratulerer! Du har opprettet 5 fakturaer!", 1, 5 },
                        { "Faktura sendt din første faktura!", "Gratulerer! Du har sendt din første faktura!", 0, 1 },
                        { "Faktura nr. 5 sendt!", "Gratulerer! Du har sendt 5 fakturaer!", 0, 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
