using Microsoft.AspNetCore.Http;
using ReelJunkies.Enums;
using ReelJunkies.Models.TmDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReelJunkies.Models.Database
{
    public class TV
    {
        public int Id { get; set; }
        public int TmDbMovieId { get; set; }

        public string Homepage { get; set; }
        public string Name { get; set; }
        public string TagLine { get; set; }
        public string Overview { get; set; }

        public int? NumberOfEpisodes { get; set; }

        public int? NumberOfSeasons { get; set; }

        public ICollection<TVSeasons> Seasons { get; set; } = new HashSet<TVSeasons>();
        public ICollection<TmdbGenreDetail> Genres { get; set; } = new HashSet<TmdbGenreDetail>();

        [DataType(DataType.Date)]
        [Display(Name = "First Aired")]
        public DateTime FirstAirDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Last Aired")]
        public DateTime LastAirDate { get; set; }

        public decimal VoteAverage { get; set; }

        [Display(Name = "Poster")]
        public byte[] Poster { get; set; }

        public string PosterType { get; set; }


        [Display(Name = "Backdrop")]
        public byte[] Backdrop { get; set; }

        public string BackDropType { get; set; }



        [NotMapped]
        [Display(Name = "Poster Image")]
        public IFormFile PosterFile { get; set; }

        [NotMapped]
        [Display(Name = "Backdrop Image")]
        public IFormFile BackdropFile { get; set; }

        public ICollection<DbMovieReview> Reviews { get; set; } = new HashSet<DbMovieReview>();

        public ICollection<MovieCast> Cast { get; set; } = new HashSet<MovieCast>();

        public ICollection<MovieCrew> Crew { get; set; } = new HashSet<MovieCrew>();

        public ICollection<SimilarMovie> Similar { get; set; } = new HashSet<SimilarMovie>();

    }
}
