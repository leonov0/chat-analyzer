using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChatHistoryToAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EncryptedChat",
                table: "Analyses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedChat",
                table: "Analyses");
        }
    }
}
