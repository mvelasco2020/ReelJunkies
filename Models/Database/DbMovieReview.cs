using ReelJunkies.Models.Database;
using System;

namespace ReelJunkies.Models.TmDb
{
    public class DbMovieReview
    {

        public int Id { get; set; }

        public DbReviewAuthor Author { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }


    }
}
