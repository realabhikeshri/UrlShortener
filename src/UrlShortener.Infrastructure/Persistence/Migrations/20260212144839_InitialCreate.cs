using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalUrl = table.Column<string>(type: "text", nullable: false),
                    ShortCode = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlAnalytics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShortUrlId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClickCount = table.Column<long>(type: "bigint", nullable: false),
                    LastClickedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlAnalytics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_ShortCode",
                table: "ShortUrls",
                column: "ShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UrlAnalytics_ShortUrlId",
                table: "UrlAnalytics",
                column: "ShortUrlId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrls");

            migrationBuilder.DropTable(
                name: "UrlAnalytics");
        }
    }
}
