using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Smartshopping.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<string>(nullable: false),
                    Supplier = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Img = table.Column<string>(nullable: false),
                    Prefix = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: false),
                    Unit = table.Column<string>(nullable: false),
                    Compare = table.Column<double>(nullable: false),
                    Latest = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
