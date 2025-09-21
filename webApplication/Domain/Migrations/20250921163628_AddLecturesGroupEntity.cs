using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddLecturesGroupEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grouplecture",
                columns: table => new
                {
                    groupsgroupid = table.Column<int>(type: "integer", nullable: false),
                    lectureslectureid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grouplecture", x => new { x.groupsgroupid, x.lectureslectureid });
                    table.ForeignKey(
                        name: "FK_grouplecture_groups_groupsgroupid",
                        column: x => x.groupsgroupid,
                        principalTable: "groups",
                        principalColumn: "groupid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grouplecture_lectures_lectureslectureid",
                        column: x => x.lectureslectureid,
                        principalTable: "lectures",
                        principalColumn: "lectureid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_grouplecture_lectureslectureid",
                table: "grouplecture",
                column: "lectureslectureid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grouplecture");
        }
    }
}
