using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class TimeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeAdded",
                table: "friend_requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeAdded",
                table: "friend_requests");
        }
    }
}
