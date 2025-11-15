using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayListMusics_UserPlayLists_UserPlayListId1",
                table: "UserPlayListMusics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlayLists_Users_UserId1",
                table: "UserPlayLists");

            migrationBuilder.DropIndex(
                name: "IX_UserPlayLists_UserId1",
                table: "UserPlayLists");

            migrationBuilder.DropIndex(
                name: "IX_UserPlayListMusics_MusicId_UserPlayListId",
                table: "UserPlayListMusics");

            migrationBuilder.DropIndex(
                name: "IX_UserPlayListMusics_UserPlayListId",
                table: "UserPlayListMusics");

            migrationBuilder.DropIndex(
                name: "IX_UserPlayListMusics_UserPlayListId1",
                table: "UserPlayListMusics");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserPlayLists");

            migrationBuilder.DropColumn(
                name: "UserPlayListId1",
                table: "UserPlayListMusics");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserPlayLists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PathPhoto",
                table: "UserPlayLists",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserPlayListId",
                table: "UserPlayListMusics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MusicId",
                table: "UserPlayListMusics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusics_MusicId",
                table: "UserPlayListMusics",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusics_UserPlayListId_MusicId",
                table: "UserPlayListMusics",
                columns: new[] { "UserPlayListId", "MusicId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPlayListMusics_MusicId",
                table: "UserPlayListMusics");

            migrationBuilder.DropIndex(
                name: "IX_UserPlayListMusics_UserPlayListId_MusicId",
                table: "UserPlayListMusics");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserPlayLists",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "PathPhoto",
                table: "UserPlayLists",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserPlayLists",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserPlayListId",
                table: "UserPlayListMusics",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MusicId",
                table: "UserPlayListMusics",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "UserPlayListId1",
                table: "UserPlayListMusics",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayLists_UserId1",
                table: "UserPlayLists",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusics_MusicId_UserPlayListId",
                table: "UserPlayListMusics",
                columns: new[] { "MusicId", "UserPlayListId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusics_UserPlayListId",
                table: "UserPlayListMusics",
                column: "UserPlayListId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlayListMusics_UserPlayListId1",
                table: "UserPlayListMusics",
                column: "UserPlayListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayListMusics_UserPlayLists_UserPlayListId1",
                table: "UserPlayListMusics",
                column: "UserPlayListId1",
                principalTable: "UserPlayLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlayLists_Users_UserId1",
                table: "UserPlayLists",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
