using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class AddSuffix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Name_SuffixId",
                table: "Student",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Suffix",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suffix", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Name_SuffixId",
                table: "Student",
                column: "Name_SuffixId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Suffix_Name_SuffixId",
                table: "Student",
                column: "Name_SuffixId",
                principalTable: "Suffix",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Suffix_Name_SuffixId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Suffix");

            migrationBuilder.DropIndex(
                name: "IX_Student_Name_SuffixId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Name_SuffixId",
                table: "Student");
        }
    }
}
