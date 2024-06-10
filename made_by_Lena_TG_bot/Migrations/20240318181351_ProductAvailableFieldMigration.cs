using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace made_by_Lena_TG_bot.Migrations
{
    /// <inheritdoc />
    public partial class ProductAvailableFieldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Products");
        }
    }
}
