using Microsoft.EntityFrameworkCore.Migrations;

namespace ReelJunkies.Migrations
{
    public partial class dropauthorfrommoviereview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbMovieReview_DbReviewAuthor_AuthorDetailsId",
                table: "DbMovieReview");

            migrationBuilder.DropForeignKey(
                name: "FK_DbMovieReview_Movie_MovieId",
                table: "DbMovieReview");

            migrationBuilder.DropIndex(
                name: "IX_DbMovieReview_AuthorDetailsId",
                table: "DbMovieReview");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DbTVReview",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "DbMovieReview",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DbMovieReview",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DbMovieReview_Movie_MovieId",
                table: "DbMovieReview",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbMovieReview_Movie_MovieId",
                table: "DbMovieReview");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DbTVReview",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "DbMovieReview",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DbMovieReview",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000);

            migrationBuilder.CreateIndex(
                name: "IX_DbMovieReview_AuthorDetailsId",
                table: "DbMovieReview",
                column: "AuthorDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbMovieReview_DbReviewAuthor_AuthorDetailsId",
                table: "DbMovieReview",
                column: "AuthorDetailsId",
                principalTable: "DbReviewAuthor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DbMovieReview_Movie_MovieId",
                table: "DbMovieReview",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
