using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class DBMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BMP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Magic = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Reserved = table.Column<long>(type: "bigint", nullable: false),
                    Offset = table.Column<long>(type: "bigint", nullable: false),
                    HeaderSize = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Planes = table.Column<int>(type: "int", nullable: false),
                    BPP = table.Column<int>(type: "int", nullable: false),
                    Compression = table.Column<long>(type: "bigint", nullable: false),
                    ImgSize = table.Column<long>(type: "bigint", nullable: false),
                    XPixelPerm = table.Column<int>(type: "int", nullable: false),
                    YPixelPerm = table.Column<int>(type: "int", nullable: false),
                    ColoursUsed = table.Column<long>(type: "bigint", nullable: false),
                    ImportantColours = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PGA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PGA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PLT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RAF",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Frames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLTs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PGAs = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RPF",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CLUT = table.Column<byte>(type: "tinyint", nullable: false),
                    PLT = table.Column<int>(type: "int", nullable: false),
                    PGA = table.Column<int>(type: "int", nullable: false),
                    Animated = table.Column<byte>(type: "tinyint", nullable: false),
                    RAF = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<byte>(type: "tinyint", nullable: false),
                    Height = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RPF", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BMP");

            migrationBuilder.DropTable(
                name: "PGA");

            migrationBuilder.DropTable(
                name: "PLT");

            migrationBuilder.DropTable(
                name: "RAF");

            migrationBuilder.DropTable(
                name: "RPF");
        }
    }
}
