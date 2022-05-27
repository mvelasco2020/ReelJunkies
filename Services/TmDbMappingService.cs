using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReelJunkies.Data;
using ReelJunkies.Enums;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.Settings;
using ReelJunkies.Models.TmDb;
using ReelJunkies.Services.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReelJunkies.Services
{
    public class TmDbMappingService : IDataMappingService
    {
        private readonly AppSettings _appSettings;
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _dbContext;

        //needs to be ioptions bec we are using configured instance
        public TmDbMappingService(IOptions<AppSettings> appSettings,
                                          IImageService imageService,
                                          ApplicationDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _imageService = imageService;
            _dbContext = dbContext;
        }
        public ActorDetail MapActorDetail(ActorDetail actor)
        {
            //1. Image
            actor.profile_path = BuildCastImage(actor.profile_path);

            //2. Bio
            if (string.IsNullOrEmpty(actor.biography))
            {
                actor.biography = "Not Available";
            }

            //Place of birth
            if (string.IsNullOrEmpty(actor.place_of_birth))
            {
                actor.place_of_birth = "Not Available";
            }

            //Birthday
            if (string.IsNullOrEmpty(actor.birthday))
                actor.birthday = "Not Available";
            else
                actor.birthday = DateTime.Parse(actor.birthday).ToString("MMM dd, yyyy");

            return actor;
        }

        public async Task<Movie> MapMovieDetailAsync(MovieDetail movie)
        {
            Movie newMovie = new Movie();

            try
            {
                newMovie.TmDbMovieId = movie.id;
                newMovie.Title = movie.title;
                newMovie.TagLine = movie.tagline;
                newMovie.Overview = movie.overview;
                newMovie.RunTime = movie.runtime;
                newMovie.VoteAverage = movie.vote_average;
                newMovie.ReleaseDate = DateTime.Parse(movie.release_date);
                newMovie.TrailerUrl = BuildTrailerPath(movie.videos);
                newMovie.Backdrop = await EncodeBackdropImageAsync(movie.backdrop_path);
                newMovie.BackDropType = BuildImageType(movie.poster_path);
                newMovie.Poster = await EncodePosterImageAsync(movie.poster_path);
                newMovie.PosterType = BuildImageType(movie.poster_path);
                newMovie.Rating = GetRating(movie.release_dates);


                var castMembers = movie.credits
                                        .cast
                                        .OrderByDescending(c => c.popularity)
                                        .GroupBy(c => c.id)
                                        .Select(g => g.FirstOrDefault())
                                        .Take(20)
                                        .ToList();

                castMembers.ForEach(member =>
                {
                    newMovie.Cast.Add(new MovieCast()
                    {
                        CastId = member.id,
                        Department = member.known_for_department,
                        Name = member.name,
                        Character = member.character,
                        ImageUrl = BuildCastImage(member.profile_path),
                    });
                });

                var genres = movie.genres.ToList();
                genres.ForEach(genre =>
                    newMovie.Genres.Add(
                    new TmdbGenreDetail()
                    {
                        Id = genre.id,
                        Name = genre.name
                    })
                );

                var reviews = movie.reviews.results.ToList();
                reviews.ForEach(review =>
                newMovie.Reviews.Add(
                new DbMovieReview()
                {
                    AuthorUsername = review.author,
                    AuthorDetailsId = review.author_details.username,
                    Content = review.content,
                    CreateDate = DateTime.Parse(review.created_at),
                    UpdateDate = DateTime.Parse(review.updated_at),
                    Url = review.url
                }));

                var tvReviewsFromDb = await _dbContext
                        .DbMovieReview
                        .Where(r => r.MovieId == movie.id).ToListAsync();
                if (tvReviewsFromDb != null)
                {
                    tvReviewsFromDb.ForEach(review =>
                    {
                        newMovie.Reviews.Add(review);
                    });
                }

                var crewMembers = movie.credits.crew
                        .OrderByDescending(c => c.popularity)
                        .GroupBy(c => c.id)
                        .Select(g => g.FirstOrDefault())
                        .Take(20)
                        .ToList();


                crewMembers.ForEach(member =>
                {
                    newMovie.Crew.Add(new MovieCrew()
                    {
                        CrewID = member.id,
                        Department = member.department,
                        Name = member.name,
                        Job = member.job,
                        ImageUrl = BuildCastImage(member.profile_path),
                    });
                });

                foreach (var similarMovie in movie.similar.results.ToList().Take(4))
                {
                    newMovie.Similar.Add(new SimilarMovie()
                    {
                        TmDbId = similarMovie.id,
                        PosterPath = $"{_appSettings.TmDbSettings.BaseImagePath}" +
                        $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                        $"/{similarMovie.poster_path}",

                        GenreIds = similarMovie.genre_ids,
                        Title = similarMovie.title,
                        ReleaseDate = DateTime.Parse(similarMovie.release_date)
                    });
                }

            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"Exception in MapMovieDetailAsync: {ex.Message}");
            }

            return newMovie;
        }


        public async Task<TV> MapTvDetailAsync(TVDetail tv)
        {
            TV newTV = new TV();

            try
            {
                newTV.TmDbTVId = tv.id;
                newTV.Homepage = tv.homepage;
                newTV.Name = tv.name;
                newTV.TagLine = tv.tagline;
                newTV.Overview = tv.overview;
                newTV.NumberOfEpisodes = tv.number_of_episodes;
                newTV.NumberOfSeasons = tv.number_of_seasons;

                foreach (var season in tv.seasons.ToList())
                {
                    newTV.Seasons.Add(
                        new TVSeasons()
                        {
                            AirDate = season.air_date,
                            TmDbId = season.id,
                            Name = season.name,
                            Overview = season.overview,
                            PosterPath = await EncodePosterImageAsync(season.poster_path),
                            SeasonNumber = season.season_number,
                        }
                        );
                }
                newTV.VoteAverage = tv.vote_average;
                newTV.FirstAirDate = DateTime.Parse(tv.first_air_date);
                newTV.LastAirDate = DateTime.Parse(tv.last_air_date);
                newTV.NumberOfEpisodes = tv.number_of_episodes;
                newTV.NumberOfSeasons = tv.number_of_seasons;
                newTV.Backdrop = await EncodeBackdropImageAsync(tv.backdrop_path);
                newTV.BackDropType = BuildImageType(tv.poster_path);
                newTV.Poster = await EncodePosterImageAsync(tv.poster_path);
                newTV.PosterType = BuildImageType(tv.poster_path);



                var castMembers = tv.credits
                                        .cast
                                        .OrderByDescending(c => c.popularity)
                                        .GroupBy(c => c.id)
                                        .Select(g => g.FirstOrDefault())
                                        .Take(20)
                                        .ToList();

                castMembers.ForEach(member =>
                {
                    newTV.Cast.Add(new MovieCast()
                    {
                        CastId = member.id,
                        Department = member.known_for_department,
                        Name = member.name,
                        Character = member.character,
                        ImageUrl = BuildCastImage(member.profile_path),
                    });
                });

                var genres = tv.genres.ToList();
                genres.ForEach(genre =>
                    newTV.Genres.Add(
                    new TmdbGenreDetail()
                    {
                        Id = genre.id,
                        Name = genre.name
                    })
                );

                var reviews = tv.reviews.results.ToList();
                reviews.ForEach(review =>
                newTV.Reviews.Add(
                new DbTVReview()
                {
                    AuthorUsername = review.author,
                    AuthorDetailsId = review.author_details.username,
                    Content = review.content,
                    CreateDate = DateTime.Parse(review.created_at),
                    UpdateDate = DateTime.Parse(review.updated_at),
                    Url = review.url
                }));

                var tvReviewsFromDb = await _dbContext
                                        .DbTVReview
                                        .Where(r => r.TVId == tv.id).ToListAsync();
                if (tvReviewsFromDb != null)
                {
                    tvReviewsFromDb.ForEach(review =>
                    {
                        newTV.Reviews.Add(review);
                    });
                }



                var crewMembers = tv.credits.crew
                        .OrderByDescending(c => c.popularity)
                        .GroupBy(c => c.id)
                        .Select(g => g.FirstOrDefault())
                        .Take(20)
                        .ToList();


                crewMembers.ForEach(member =>
                {
                    newTV.Crew.Add(new MovieCrew()
                    {
                        CrewID = member.id,
                        Department = member.department,
                        Name = member.name,
                        Job = member.job,
                        ImageUrl = BuildCastImage(member.profile_path),
                    });
                });

                foreach (var similarTV in tv.similar.results.ToList().Take(4))
                {
                    newTV.Similar.Add(new SimilarMovie()
                    {
                        TmDbId = similarTV.id,
                        PosterPath = $"{_appSettings.TmDbSettings.BaseImagePath}" +
                        $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                        $"/{similarTV.poster_path}",

                        GenreIds = similarTV.genre_ids,
                        Title = similarTV.name,
                        ReleaseDate = DateTime.Parse(similarTV.first_air_date)
                    });
                }

            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"Exception in MapMovieDetailAsync: {ex.Message}");
            }
            return newTV;
        }

        public QueryAll MapQueryAllDetails(QueryAll queryResult)

        {
            foreach (var result in queryResult.results)
            {
                if (result.media_type == MediaType.person.ToString())
                {
                    if (result.profile_path is null)
                    {
                        result.profile_path = _appSettings.ReelJunkiesSettings.PosterUnavailable;
                    }
                    else
                    {
                        result.profile_path = BuildCastImage(result.profile_path);
                    }
                }
                else
                {
                    if (result.poster_path is null)
                    {
                        result.poster_path = _appSettings.ReelJunkiesSettings.PosterUnavailable;
                        result.backdrop_path = _appSettings.ReelJunkiesSettings.PosterUnavailable;
                    }
                    else
                    {

                        result.poster_path = $"{_appSettings.TmDbSettings.BaseImagePath}" +
                                         $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                                         $"/{result.poster_path}";
                        result.backdrop_path = $"{_appSettings.TmDbSettings.BaseImagePath}" +
                              $"/{_appSettings.ReelJunkiesSettings.DefaultBackdropSize}" +
                              $"/{result.backdrop_path}";
                    }
                }
            }
            return queryResult;
        }

        private string BuildCastImage(string profile_path)
        {
            if (string.IsNullOrEmpty(profile_path))
                return _appSettings.ReelJunkiesSettings.DefaultCastImage;

            return $"{_appSettings.TmDbSettings.BaseImagePath}" +
                $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                $"/{profile_path}";
        }

        private MovieRating GetRating(Release_Dates release_dates)
        {
            var movieRating = MovieRating.NR;
            var certification = release_dates.results.FirstOrDefault(r => r.iso_3166_1 == "US");
            if (certification is not null)
            {
                var apiRating = certification
                                .release_dates
                                .FirstOrDefault(c => c.certification != "")?
                                .certification.Replace("-", "");
                if (!string.IsNullOrEmpty(apiRating))
                {
                    movieRating = (MovieRating)Enum.Parse(typeof(MovieRating), apiRating, true);
                }
            }
            return movieRating;
        }

        private async Task<byte[]> EncodePosterImageAsync(string path)
        {
            var posterPath = $"{_appSettings.TmDbSettings.BaseImagePath}" +
                             $"/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}" +
                             $"/{path}";

            return await _imageService
                         .EncodeImageURLAsync(posterPath);
        }

        private string BuildImageType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            return $"image/{Path.GetExtension(path).TrimStart('.')}";
        }

        private async Task<byte[]> EncodeBackdropImageAsync(string backdrop_path)
        {
            var backdropPath = $"{_appSettings.TmDbSettings.BaseImagePath}" +
                               $"/{_appSettings.ReelJunkiesSettings.DefaultBackdropSize}" +
                               $"/{backdrop_path}";

            return await _imageService
                         .EncodeImageURLAsync(backdropPath);
        }

        private string BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.results.FirstOrDefault(r => r.type.ToLower().Trim() == "trailer" && r.key != "")?.key;
            return string.IsNullOrEmpty(videoKey) ? videoKey : $"{_appSettings.TmDbSettings.BaseYouTubePath}{videoKey}";
        }
    }
}
