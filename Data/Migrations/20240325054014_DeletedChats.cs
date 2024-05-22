using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletedChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BirthdayManId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatType = table.Column<string>(type: "text", nullable: false),
                    ChatUrl = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    CreatingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.UniqueConstraint("AK_Chats_ChatUrl", x => x.ChatUrl);
                    table.ForeignKey(
                        name: "FK_Chats_Profiles_BirthdayManId",
                        column: x => x.BirthdayManId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_BirthdayManId",
                table: "Chats",
                column: "BirthdayManId");
        }
    }
}
