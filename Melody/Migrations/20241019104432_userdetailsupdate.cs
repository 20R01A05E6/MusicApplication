using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melody.Migrations
{
    /// <inheritdoc />
    public partial class userdetailsupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "UserDetails");
        }
    }
}
