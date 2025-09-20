using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class MigratonName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    cityid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cityname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    postalcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cities_pkey", x => x.cityid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    avatar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, defaultValueSql: "'/def.png'::character varying"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    surname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    patronymic = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    telephonnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    passwordhash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "institution",
                columns: table => new
                {
                    institutionid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    street = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    website = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    cityid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("institution_pkey", x => x.institutionid);
                    table.ForeignKey(
                        name: "institution_cityid_fkey",
                        column: x => x.cityid,
                        principalTable: "cities",
                        principalColumn: "cityid");
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notificationid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
                    isread = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("notifications_pkey", x => x.notificationid);
                    table.ForeignKey(
                        name: "notifications_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "portfolio",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false),
                    achievement = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    addedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("portfolio_pkey", x => new { x.userid, x.achievement });
                    table.ForeignKey(
                        name: "portfolio_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    groupid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    course = table.Column<int>(type: "integer", nullable: true),
                    curatorid = table.Column<int>(type: "integer", nullable: false),
                    specialty = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    institutionid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("groups_pkey", x => x.groupid);
                    table.ForeignKey(
                        name: "groups_curatorid_fkey",
                        column: x => x.curatorid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "groups_institutionid_fkey",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    roomid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roomnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    institutionid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rooms_pkey", x => x.roomid);
                    table.ForeignKey(
                        name: "rooms_institutionid_fkey",
                        column: x => x.institutionid,
                        principalTable: "institution",
                        principalColumn: "institutionid");
                });

            migrationBuilder.CreateTable(
                name: "students_groups",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false),
                    groupid = table.Column<int>(type: "integer", nullable: false),
                    enrolledat = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "students_groups_groupid_fkey",
                        column: x => x.groupid,
                        principalTable: "groups",
                        principalColumn: "groupid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "students_groups_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lectures",
                columns: table => new
                {
                    lectureid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lecturename = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    starttime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    endtime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    teacherid = table.Column<int>(type: "integer", nullable: false),
                    roomid = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("lectures_pkey", x => x.lectureid);
                    table.ForeignKey(
                        name: "lectures_roomid_fkey",
                        column: x => x.roomid,
                        principalTable: "rooms",
                        principalColumn: "roomid");
                    table.ForeignKey(
                        name: "lectures_teacherid_fkey",
                        column: x => x.teacherid,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "room_equipment",
                columns: table => new
                {
                    roomid = table.Column<int>(type: "integer", nullable: false),
                    equipment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("room_equipment_pkey", x => new { x.roomid, x.equipment });
                    table.ForeignKey(
                        name: "room_equipment_roomid_fkey",
                        column: x => x.roomid,
                        principalTable: "rooms",
                        principalColumn: "roomid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attendance",
                columns: table => new
                {
                    attendanceid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lectureid = table.Column<int>(type: "integer", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    ispresent = table.Column<bool>(type: "boolean", nullable: false),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    recordedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("attendance_pkey", x => x.attendanceid);
                    table.ForeignKey(
                        name: "attendance_lectureid_fkey",
                        column: x => x.lectureid,
                        principalTable: "lectures",
                        principalColumn: "lectureid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "attendance_userid_fkey",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lectures_groups",
                columns: table => new
                {
                    groupid = table.Column<int>(type: "integer", nullable: false),
                    lectureid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lectures_groups_pkey", x => new { x.groupid, x.lectureid });
                    table.ForeignKey(
                        name: "lectures_groups_groupid_fkey",
                        column: x => x.groupid,
                        principalTable: "groups",
                        principalColumn: "groupid");
                    table.ForeignKey(
                        name: "lectures_groups_lectureid_fkey",
                        column: x => x.lectureid,
                        principalTable: "lectures",
                        principalColumn: "lectureid");
                });

            migrationBuilder.CreateIndex(
                name: "attendance_lectureid_userid_key",
                table: "attendance",
                columns: new[] { "lectureid", "userid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attendance_userid",
                table: "attendance",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "cities_cityname_key",
                table: "cities",
                column: "cityname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_groups_curatorid",
                table: "groups",
                column: "curatorid");

            migrationBuilder.CreateIndex(
                name: "IX_groups_institutionid",
                table: "groups",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "IX_institution_cityid",
                table: "institution",
                column: "cityid");

            migrationBuilder.CreateIndex(
                name: "IX_lectures_roomid",
                table: "lectures",
                column: "roomid");

            migrationBuilder.CreateIndex(
                name: "IX_lectures_teacherid",
                table: "lectures",
                column: "teacherid");

            migrationBuilder.CreateIndex(
                name: "IX_lectures_groups_lectureid",
                table: "lectures_groups",
                column: "lectureid");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_userid",
                table: "notifications",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_institutionid",
                table: "rooms",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "rooms_roomnumber_institutionid_key",
                table: "rooms",
                columns: new[] { "roomnumber", "institutionid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_groups_groupid",
                table: "students_groups",
                column: "groupid");

            migrationBuilder.CreateIndex(
                name: "IX_students_groups_userid",
                table: "students_groups",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance");

            migrationBuilder.DropTable(
                name: "lectures_groups");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "portfolio");

            migrationBuilder.DropTable(
                name: "room_equipment");

            migrationBuilder.DropTable(
                name: "students_groups");

            migrationBuilder.DropTable(
                name: "lectures");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "institution");

            migrationBuilder.DropTable(
                name: "cities");
        }
    }
}
