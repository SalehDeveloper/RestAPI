using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movies.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddGenersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("3eed0ee4-a18f-4e09-8680-16b1ad4b5c78"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("5c37b1a0-7a1a-4f62-9741-e519cdd62dc7"));

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: new Guid("6c8d365b-09e8-423c-b7d7-54f1863e6adb"));

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genre_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genre_MovieId",
                table: "Genre",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Genres", "Title", "YearOfRelease" },
                values: new object[,]
                {
                    { new Guid("3eed0ee4-a18f-4e09-8680-16b1ad4b5c78"), "[\"a\",\"b\"]", "C# Pro", 2010 },
                    { new Guid("5c37b1a0-7a1a-4f62-9741-e519cdd62dc7"), "[\"a\",\"b\"]", "C#", 2010 },
                    { new Guid("6c8d365b-09e8-423c-b7d7-54f1863e6adb"), "[\"a\",\"b\"]", "C# Hero", 2010 }
                });
        }
    }
}
