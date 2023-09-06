using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "friend_requests",
                columns: table => new
                {
                    Sender = table.Column<string>(type: "text", nullable: false),
                    Receiver = table.Column<string>(type: "text", nullable: false),
                    TimeAdded = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friend_requests", x => new { x.Sender, x.Receiver });
                    table.ForeignKey(
                        name: "FK_friend_requests_Profiles_Receiver",
                        column: x => x.Receiver,
                        principalTable: "Profiles",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_friend_requests_Profiles_Sender",
                        column: x => x.Sender,
                        principalTable: "Profiles",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    User1 = table.Column<string>(type: "text", nullable: false),
                    User2 = table.Column<string>(type: "text", nullable: false),
                    TimeAdded = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => new { x.User1, x.User2 });
                    table.ForeignKey(
                        name: "FK_Friendship_Profiles_User1",
                        column: x => x.User1,
                        principalTable: "Profiles",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendship_Profiles_User2",
                        column: x => x.User2,
                        principalTable: "Profiles",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_friend_requests_Receiver",
                table: "friend_requests",
                column: "Receiver");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_User2",
                table: "Friendship",
                column: "User2");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Email",
                table: "Profiles",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friend_requests");

            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
