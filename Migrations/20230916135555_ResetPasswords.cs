using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class ResetPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResetPasswordRequests",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordRequests", x => x.Username);
                    table.ForeignKey(
                        name: "FK_ResetPasswordRequests_Profiles_Username",
                        column: x => x.Username,
                        principalTable: "Profiles",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordRequests_Code",
                table: "ResetPasswordRequests",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResetPasswordRequests");
        }
    }
}
