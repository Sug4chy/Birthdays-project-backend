using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDeletedAtUtcFieldFromEverywhere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "Wishes");

            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "DeletingTime",
                table: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "Wishes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "WishLists",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "Profiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "Images",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletingTime",
                table: "Chats",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
