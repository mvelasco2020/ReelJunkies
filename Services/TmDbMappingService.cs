using Microsoft.Extensions.Options;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.Settings;
using ReelJunkies.Models.TmDb;
using ReelJunkies.Services.Interfaces;
using System;
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
            throw new System.NotImplementedException();
        }

        public async Task<Movie> MapMovieDetailAsync(MovieDetail movie)
        {
            Movie newMovie = null;

            try
            {
                newMovie = new Movie()
                {
                    TmDbMovieId = movie.id,
                    Title = movie.title,
                    TagLine = movie.tagline,
                    Overview = movie.overview,
                    RunTime = movie.runtime,
                    VoteAverage = movie.vote_average,
                    ReleaseDate = DateTime.Parse(movie.release_date),
                    TrailerUrl = BuildTrailerPath(movie.videos),
                    Backdrop = await EncodeBackdropImageAsync(movie.backdrop_path)
                };
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        private Task<byte[]> EncodeBackdropImageAsync(string backdrop_path)
        {
            throw new NotImplementedException();
        }

        private string BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.results.FirstOrDefault(r => r.type.ToLower().Trim() == "trailer" && r.key != "")?.key;
            return string.IsNullOrEmpty(videoKey) ? videoKey : $"{_appSettings.TmDbSettings.BaseYouTubePath}{videoKey}";
        }
    }
}
