using Microsoft.Extensions.Options;
using ReelJunkies.Enums;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.Settings;
using ReelJunkies.Models.TmDb;
using ReelJunkies.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReelJunkies.Services
{
    public class TmDbMappingService : IDataMappingService
    {
        private readonly AppSettings _appSettings;
        private readonly IImageService _imageService;

        //needs to be ioptions bec we are using configured instance
        public TmDbMappingService(IOptions<AppSettings> appSettings, IImageService imageService)
        {
            _appSettings = appSettings.Value;
            _imageService = imageService;
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


            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"Exception in MapMovieDetailAsync: {ex.Message}");
            }

            return newMovie;
        }

        private string BuildCastImage(string profile_path)
        {
            if (string.IsNullOrEmpty(profile_path))
                return _appSettings.ReelJunkiesSettings.DefaultCastImage;

            return $"{_appSettings.TmDbSettings.BaseImagePath}/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}/{profile_path}";
        }

        private MovieRating GetRating(Release_Dates release_dates)
        {
            var movieRating = MovieRating.NR;
            var certification = release_dates.results.FirstOrDefault(r => r.iso_3166_1 == "US");
            if (certification is not null)
            {
                var apiRating = certification.release_dates.FirstOrDefault(c => c.certification != "")?.certification.Replace("-", "");
                if (!string.IsNullOrEmpty(apiRating))
                {
                    movieRating = (MovieRating)Enum.Parse(typeof(MovieRating), apiRating, true);
                }
            }
            return movieRating;
        }

        private async Task<byte[]> EncodePosterImageAsync(string path)
        {
            var posterPath = $"{_appSettings.TmDbSettings.BaseImagePath}/{_appSettings.ReelJunkiesSettings.DefaultPosterSize}/{path}";
            return await _imageService.EncodeImageURLAsync(posterPath);
        }

        private string BuildImageType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            return $"image/{Path.GetExtension(path).TrimStart('.')}";
        }

        private async Task<byte[]> EncodeBackdropImageAsync(string backdrop_path)
        {
            var backdropPath = $"{_appSettings.TmDbSettings.BaseImagePath}/{_appSettings.ReelJunkiesSettings.DefaultBackdropSize}/{backdrop_path}";
            return await _imageService.EncodeImageURLAsync(backdropPath);
        }

        private string BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.results.FirstOrDefault(r => r.type.ToLower().Trim() == "trailer" && r.key != "")?.key;
            return string.IsNullOrEmpty(videoKey) ? videoKey : $"{_appSettings.TmDbSettings.BaseYouTubePath}{videoKey}";
        }
    }
}
