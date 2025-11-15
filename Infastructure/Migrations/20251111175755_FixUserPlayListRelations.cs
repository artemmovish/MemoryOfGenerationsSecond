using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserPlayListRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestForAuthId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HelpTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestForAuth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestForAuth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPlayList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PathPhoto = table.Column<string>(type: "TEXT", nullable: false),
                    UserId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlayList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlayList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlayList_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPlayListMusic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MusicId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserPlayListId = table.Column<int>(type: "INTEGER", nullable: false),
                    MusicId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    UserPlayListId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlayListMusic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlayListMusic_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlayListMusic_Musics_MusicId1",
                        column: x => x.MusicId1,
                        principalTable: "Musics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPlayListMusic_UserPlayList_UserPlayListId",
                        column: x => x.UserPlayListId,
                        principalTable: "UserPlayList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlayListMusic_UserPlayList_UserPlayListId1",
                        column: x => x.UserPlayListId1,
                        principalTable: "UserPlayList",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_QuestForAuthId",
                table: "Users",
                column: "QuestForAuthId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayList_UserId",
                table: "UserPlayList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayList_UserId1",
                table: "UserPlayList",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusic_MusicId_UserPlayListId",
                table: "UserPlayListMusic",
                columns: new[] { "MusicId", "UserPlayListId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusic_MusicId1",
                table: "UserPlayListMusic",
                column: "MusicId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusic_UserPlayListId",
                table: "UserPlayListMusic",
                column: "UserPlayListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusic_UserPlayListId1",
                table: "UserPlayListMusic",
                column: "UserPlayListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users",
                column: "QuestForAuthId",
                principalTable: "QuestForAuth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "HelpTexts");

            migrationBuilder.DropTable(
                name: "QuestForAuth");

            migrationBuilder.DropTable(
                name: "UserPlayListMusic");

            migrationBuilder.DropTable(
                name: "UserPlayList");

            migrationBuilder.DropIndex(
                name: "IX_Users_QuestForAuthId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QuestForAuthId",
                table: "Users");
        }
    }
}
