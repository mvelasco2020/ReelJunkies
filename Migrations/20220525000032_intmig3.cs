using Microsoft.EntityFrameworkCore.Migrations;

namespace ReelJunkies.Migrations
{
    public partial class intmig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbTVReview_DbReviewAuthor_AuthorDetailsId",
                table: "DbTVReview");

            migrationBuilder.DropIndex(
                name: "IX_DbTVReview_AuthorDetailsId",
                table: "DbTVReview");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DbTVReview_AuthorDetailsId",
                table: "DbTVReview",
                column: "AuthorDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbTVReview_DbReviewAuthor_AuthorDetailsId",
                table: "DbTVReview",
                column: "AuthorDetailsId",
                principalTable: "DbReviewAuthor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
