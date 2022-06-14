using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZyPanel.Migrations {
    public partial class init2 : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable (
                name: "TblKarset",
                columns : table => new {
                    Id = table.Column<long> (type: "bigint", nullable : false)
                        .Annotation ("SqlServer:Identity", "1, 1"),
                        ProductTonnage = table.Column<int> (type: "int", nullable : false),
                        PlanningTonnage = table.Column<int> (type: "int", nullable : false),
                        KarsetType = table.Column<byte> (type: "tinyint", nullable : false)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_TblKarset", x => x.Id);
                });
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable (
                name: "TblKarset");
        }
    }
}