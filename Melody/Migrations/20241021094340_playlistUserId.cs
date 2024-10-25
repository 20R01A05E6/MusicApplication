using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melody.Migrations
{
    /// <inheritdoc />
    public partial class playlistUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PlaylistSongs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlaylistSongs");
        }
    }
}
