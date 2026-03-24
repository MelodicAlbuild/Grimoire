using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grimoire.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEmulatorBinaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmulatorBinaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmulatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    RuntimeId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    FileHash = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmulatorBinaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmulatorBinaries_Emulators_EmulatorId",
                        column: x => x.EmulatorId,
                        principalTable: "Emulators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmulatorBinaries_EmulatorId_RuntimeId",
                table: "EmulatorBinaries",
                columns: new[] { "EmulatorId", "RuntimeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmulatorBinaries");
        }
    }
}
