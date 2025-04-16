using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAnalyzer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageTypeToAnalysisMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "AnalysisMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "AnalysisMessages");
        }
    }
}
