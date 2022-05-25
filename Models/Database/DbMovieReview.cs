using ReelJunkies.Models.Database;
using System;
using System.ComponentModel.DataAnnotations;

namespace ReelJunkies.Models.TmDb
{
    public class DbMovieReview
    {

        public int Id { get; set; }

        public string AuthorUsername { get; set; }
        public string AuthorDetailsId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [Required]
        [MaxLength(5000), MinLength(10)]
        public string Content { get; set; }

        public int MovieId { get; set; }
        public string Url { get; set; }
    }
}
