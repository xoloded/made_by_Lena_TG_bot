using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace made_by_Lena_TG_bot.Migrations
{
    /// <inheritdoc />
    public partial class PhotoAndCategoriesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhotoId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PhotoId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CategoryId",
                table: "Reviews",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PhotoId",
                table: "Reviews",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PhotoId",
                table: "Products",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Photos_PhotoId",
                table: "Products",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Categories_CategoryId",
                table: "Reviews",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Photos_PhotoId",
                table: "Reviews",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Photos_PhotoId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Categories_CategoryId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Photos_PhotoId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CategoryId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_PhotoId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PhotoId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Products");
        }
    }
}
