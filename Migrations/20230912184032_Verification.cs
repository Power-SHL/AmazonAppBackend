using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class Verification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_friend_requests_Profiles_Receiver",
                table: "friend_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_friend_requests_Profiles_Sender",
                table: "friend_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Profiles_User1",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_Profiles_User2",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_friend_requests",
                table: "friend_requests");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "Friendships");

            migrationBuilder.RenameTable(
                name: "friend_requests",
                newName: "FriendRequests");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_User2",
                table: "Friendships",
                newName: "IX_Friendships_User2");

            migrationBuilder.RenameIndex(
                name: "IX_friend_requests_Receiver",
                table: "FriendRequests",
                newName: "IX_FriendRequests_Receiver");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                columns: new[] { "User1", "User2" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FriendRequests",
                table: "FriendRequests",
                columns: new[] { "Sender", "Receiver" });

            migrationBuilder.CreateTable(
                name: "UnverifiedProfiles",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    VerificationCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnverifiedProfiles", x => x.Username);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnverifiedProfiles_Email",
                table: "UnverifiedProfiles",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Profiles_Receiver",
                table: "FriendRequests",
                column: "Receiver",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Profiles_Sender",
                table: "FriendRequests",
                column: "Sender",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Profiles_User1",
                table: "Friendships",
                column: "User1",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Profiles_User2",
                table: "Friendships",
                column: "User2",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Profiles_Receiver",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Profiles_Sender",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Profiles_User1",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Profiles_User2",
                table: "Friendships");

            migrationBuilder.DropTable(
                name: "UnverifiedProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FriendRequests",
                table: "FriendRequests");

            migrationBuilder.RenameTable(
                name: "Friendships",
                newName: "Friendship");

            migrationBuilder.RenameTable(
                name: "FriendRequests",
                newName: "friend_requests");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_User2",
                table: "Friendship",
                newName: "IX_Friendship_User2");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_Receiver",
                table: "friend_requests",
                newName: "IX_friend_requests_Receiver");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                columns: new[] { "User1", "User2" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_friend_requests",
                table: "friend_requests",
                columns: new[] { "Sender", "Receiver" });

            migrationBuilder.AddForeignKey(
                name: "FK_friend_requests_Profiles_Receiver",
                table: "friend_requests",
                column: "Receiver",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_friend_requests_Profiles_Sender",
                table: "friend_requests",
                column: "Sender",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Profiles_User1",
                table: "Friendship",
                column: "User1",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_Profiles_User2",
                table: "Friendship",
                column: "User2",
                principalTable: "Profiles",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
