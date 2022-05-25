using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.TmDb;

namespace ReelJunkies.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Collection> Collection { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieCollection> MovieCollection { get; set; }
        public DbSet<DbMovieReview> DbMovieReview { get; set; }
        public DbSet<DbTVReview> DbTVReview { get; set; }
        public DbSet<DbReviewAuthor> DbReviewAuthor { get; set; }
    }
}
