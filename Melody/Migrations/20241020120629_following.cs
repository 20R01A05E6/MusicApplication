using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melody.Migrations
{
    /// <inheritdoc />
    public partial class following : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFollowing",
                table: "Artists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollowing",
                table: "Artists");
        }
    }
}
