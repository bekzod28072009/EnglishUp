using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Auth.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "Streaks");

            migrationBuilder.RenameColumn(
                name: "DaysInRow",
                table: "Streaks",
                newName: "LongestStreak");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DailyChallenges",
                newName: "AvailableDate");

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CommentText",
                table: "UserCourses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "UserCourses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "Streaks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "DailyChallenggeId",
                table: "Streaks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastActivityDate",
                table: "Streaks",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "RewardPoints",
                table: "DailyChallenges",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DailyChallenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StreakLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StreakId = table.Column<long>(type: "bigint", nullable: false),
                    ActivityDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreakLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreakLogs_Streaks_StreakId",
                        column: x => x.StreakId,
                        principalTable: "Streaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Streaks_DailyChallenggeId",
                table: "Streaks",
                column: "DailyChallenggeId");

            migrationBuilder.CreateIndex(
                name: "IX_StreakLogs_StreakId",
                table: "StreakLogs",
                column: "StreakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Streaks_DailyChallenges_DailyChallenggeId",
                table: "Streaks",
                column: "DailyChallenggeId",
                principalTable: "DailyChallenges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Streaks_DailyChallenges_DailyChallenggeId",
                table: "Streaks");

            migrationBuilder.DropTable(
                name: "StreakLogs");

            migrationBuilder.DropIndex(
                name: "IX_Streaks_DailyChallenggeId",
                table: "Streaks");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CommentText",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "Streaks");

            migrationBuilder.DropColumn(
                name: "DailyChallenggeId",
                table: "Streaks");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                table: "Streaks");

            migrationBuilder.DropColumn(
                name: "RewardPoints",
                table: "DailyChallenges");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "DailyChallenges");

            migrationBuilder.RenameColumn(
                name: "LongestStreak",
                table: "Streaks",
                newName: "DaysInRow");

            migrationBuilder.RenameColumn(
                name: "AvailableDate",
                table: "DailyChallenges",
                newName: "Date");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "Streaks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
