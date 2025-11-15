using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users",
                column: "QuestForAuthId",
                principalTable: "QuestForAuth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_QuestForAuth_QuestForAuthId",
                table: "Users");

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
