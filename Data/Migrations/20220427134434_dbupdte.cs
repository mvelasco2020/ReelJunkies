using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ReelJunkies.Data.Migrations
{
    public partial class dbupdte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbReviewAuthor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    AvatarPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbReviewAuthor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TmdbGenreDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MovieId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmdbGenreDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TmdbGenreDetail_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DbMovieReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorUsername = table.Column<string>(type: "text", nullable: true),
                    AuthorDetailsId = table.Column<int>(type: "integer", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    MovieId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbMovieReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbMovieReview_DbReviewAuthor_AuthorDetailsId",
                        column: x => x.AuthorDetailsId,
                        principalTable: "DbReviewAuthor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DbMovieReview_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbMovieReview_AuthorDetailsId",
                table: "DbMovieReview",
                column: "AuthorDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DbMovieReview_MovieId",
                table: "DbMovieReview",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_TmdbGenreDetail_MovieId",
                table: "TmdbGenreDetail",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbMovieReview");

            migrationBuilder.DropTable(
                name: "TmdbGenreDetail");

            migrationBuilder.DropTable(
                name: "DbReviewAuthor");
        }
    }
}
