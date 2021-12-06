using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    public partial class FixSuffixNameInStudentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Suffix_Name_SuffixId",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "Name_SuffixId",
                table: "Student",
                newName: "NameSuffixId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_Name_SuffixId",
                table: "Student",
                newName: "IX_Student_NameSuffixId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Suffix_NameSuffixId",
                table: "Student",
                column: "NameSuffixId",
                principalTable: "Suffix",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Suffix_NameSuffixId",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "NameSuffixId",
                table: "Student",
                newName: "Name_SuffixId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_NameSuffixId",
                table: "Student",
                newName: "IX_Student_Name_SuffixId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Suffix_Name_SuffixId",
                table: "Student",
                column: "Name_SuffixId",
                principalTable: "Suffix",
                principalColumn: "Id");
        }
    }
}
