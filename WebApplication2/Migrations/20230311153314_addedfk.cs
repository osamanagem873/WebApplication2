using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class addedfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemGallery_Items_ItemsId",
                table: "ItemGallery");

            migrationBuilder.DropIndex(
                name: "IX_ItemGallery_ItemsId",
                table: "ItemGallery");

            migrationBuilder.DropColumn(
                name: "ItemsId",
                table: "ItemGallery");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGallery_ItemId",
                table: "ItemGallery",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGallery_Items_ItemId",
                table: "ItemGallery",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemGallery_Items_ItemId",
                table: "ItemGallery");

            migrationBuilder.DropIndex(
                name: "IX_ItemGallery_ItemId",
                table: "ItemGallery");

            migrationBuilder.AddColumn<int>(
                name: "ItemsId",
                table: "ItemGallery",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemGallery_ItemsId",
                table: "ItemGallery",
                column: "ItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGallery_Items_ItemsId",
                table: "ItemGallery",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
