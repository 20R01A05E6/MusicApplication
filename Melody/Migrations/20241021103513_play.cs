using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melody.Migrations
{
    /// <inheritdoc />
    public partial class play : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "Songs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "Albums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PlaylistId",
                table: "Songs",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_PlaylistId",
                table: "Albums",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Playlists_PlaylistId",
                table: "Albums",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Playlists_PlaylistId",
                table: "Songs",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "PlaylistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Playlists_PlaylistId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Playlists_PlaylistId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_PlaylistId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Albums_PlaylistId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Albums");
        }
    }
}
