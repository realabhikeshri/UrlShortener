using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToLongUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OriginalUrl",
                table: "ShortUrls",
                newName: "LongUrl");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_LongUrl",
                table: "ShortUrls",
                column: "LongUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_LongUrl",
                table: "ShortUrls");

            migrationBuilder.RenameColumn(
                name: "LongUrl",
                table: "ShortUrls",
                newName: "OriginalUrl");
        }
    }
}
