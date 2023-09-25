using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class Timestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeCreated",
                table: "Posts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreated",
                table: "Posts");
        }
    }
}
