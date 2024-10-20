using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melody.Migrations
{
    /// <inheritdoc />
    public partial class followings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollowing",
                table: "Artists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFollowing",
                table: "Artists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
