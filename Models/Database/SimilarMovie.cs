using System;

namespace ReelJunkies.Models.Database
{
    public class SimilarMovie
    {
        public int Id { get; set; }
        public int TmDbId { get; set; }

        public string Title { get; set; }

        public string PosterPath { get; set; }
        public int[] GenreIds { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
