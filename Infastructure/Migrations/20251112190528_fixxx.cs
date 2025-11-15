using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayList_Users_UserId",
                table: "UserPlayList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayList_Users_UserId1",
                table: "UserPlayList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusic_Musics_MusicId",
                table: "UserPlayListMusic");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusic_Musics_MusicId1",
                table: "UserPlayListMusic");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusic_UserPlayList_UserPlayListId",
                table: "UserPlayListMusic");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusic_UserPlayList_UserPlayListId1",
                table: "UserPlayListMusic");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPlayListMusic",
                table: "UserPlayListMusic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPlayList",
                table: "UserPlayList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestForAuth",
                table: "QuestForAuth");

            migrationBuilder.RenameTable(
                name: "UserPlayListMusic",
                newName: "UserPlayListMusics");

            migrationBuilder.RenameTable(
                name: "UserPlayList",
                newName: "UserPlayLists");

            migrationBuilder.RenameTable(
                name: "QuestForAuth",
                newName: "QuestForAuths");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusic_UserPlayListId1",
                table: "UserPlayListMusics",
                newName: "IX_UserPlayListMusics_UserPlayListId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusic_UserPlayListId",
                table: "UserPlayListMusics",
                newName: "IX_UserPlayListMusics_UserPlayListId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusic_MusicId1",
                table: "UserPlayListMusics",
                newName: "IX_UserPlayListMusics_MusicId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusic_MusicId_UserPlayListId",
                table: "UserPlayListMusics",
                newName: "IX_UserPlayListMusics_MusicId_UserPlayListId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayList_UserId1",
                table: "UserPlayLists",
                newName: "IX_UserPlayLists_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayList_UserId",
                table: "UserPlayLists",
                newName: "IX_UserPlayLists_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPlayListMusics",
                table: "UserPlayListMusics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPlayLists",
                table: "UserPlayLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestForAuths",
                table: "QuestForAuths",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusics_Musics_MusicId",
                table: "UserPlayListMusics",
                column: "MusicId",
                principalTable: "Musics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusics_Musics_MusicId1",
                table: "UserPlayListMusics",
                column: "MusicId1",
                principalTable: "Musics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusics_UserPlayLists_UserPlayListId",
                table: "UserPlayListMusics",
                column: "UserPlayListId",
                principalTable: "UserPlayLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusics_UserPlayLists_UserPlayListId1",
                table: "UserPlayListMusics",
                column: "UserPlayListId1",
                principalTable: "UserPlayLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayLists_Users_UserId",
                table: "UserPlayLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayLists_Users_UserId1",
                table: "UserPlayLists",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_QuestForAuths_QuestForAuthId",
                table: "Users",
                column: "QuestForAuthId",
                principalTable: "QuestForAuths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusics_Musics_MusicId",
                table: "UserPlayListMusics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusics_Musics_MusicId1",
                table: "UserPlayListMusics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusics_UserPlayLists_UserPlayListId",
                table: "UserPlayListMusics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusics_UserPlayLists_UserPlayListId1",
                table: "UserPlayListMusics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayLists_Users_UserId",
                table: "UserPlayLists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayLists_Users_UserId1",
                table: "UserPlayLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_QuestForAuths_QuestForAuthId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPlayLists",
                table: "UserPlayLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPlayListMusics",
                table: "UserPlayListMusics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestForAuths",
                table: "QuestForAuths");

            migrationBuilder.RenameTable(
                name: "UserPlayLists",
                newName: "UserPlayList");

            migrationBuilder.RenameTable(
                name: "UserPlayListMusics",
                newName: "UserPlayListMusic");

            migrationBuilder.RenameTable(
                name: "QuestForAuths",
                newName: "QuestForAuth");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayLists_UserId1",
                table: "UserPlayList",
                newName: "IX_UserPlayList_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayLists_UserId",
                table: "UserPlayList",
                newName: "IX_UserPlayList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusics_UserPlayListId1",
                table: "UserPlayListMusic",
                newName: "IX_UserPlayListMusic_UserPlayListId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusics_UserPlayListId",
                table: "UserPlayListMusic",
                newName: "IX_UserPlayListMusic_UserPlayListId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusics_MusicId1",
                table: "UserPlayListMusic",
                newName: "IX_UserPlayListMusic_MusicId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserPlayListMusics_MusicId_UserPlayListId",
                table: "UserPlayListMusic",
                newName: "IX_UserPlayListMusic_MusicId_UserPlayListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPlayList",
                table: "UserPlayList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPlayListMusic",
                table: "UserPlayListMusic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestForAuth",
                table: "QuestForAuth",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayList_Users_UserId",
                table: "UserPlayList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayList_Users_UserId1",
                table: "UserPlayList",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusic_Musics_MusicId",
                table: "UserPlayListMusic",
                column: "MusicId",
                principalTable: "Musics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusic_Musics_MusicId1",
                table: "UserPlayListMusic",
                column: "MusicId1",
                principalTable: "Musics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusic_UserPlayList_UserPlayListId",
                table: "UserPlayListMusic",
                column: "UserPlayListId",
                principalTable: "UserPlayList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusic_UserPlayList_UserPlayListId1",
                table: "UserPlayListMusic",
                column: "UserPlayListId1",
                principalTable: "UserPlayList",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users",
                column: "QuestForAuthId",
                principalTable: "QuestForAuth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
