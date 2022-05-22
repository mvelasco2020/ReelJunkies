using ReelJunkies.Enums;
using ReelJunkies.Models.TmDb;
using System.Threading.Tasks;

namespace ReelJunkies.Services.Interfaces
{
    public interface IRemoteMovieService
    {
        Task<MovieDetail> MovieDetailAsync(int id);
        Task<MovieSearch> MovieSearchAsync(MovieCategory category, int count, int page);

        Task<MovieSearch> MovieSearchByGenre(string genre, int count = 6);
        Task<ActorDetail> ActorDetailAsync(int id);
        Task<CombinedCredits> ActorCombinedCreditsAsync(int id);

        Task<QueryAll> QueryAll(string queryString, int page);

        Task<TvSearch> TVSearchAsync(TVCategory category, int count, int page);
        Task<TvSearch> TvDiscoverAsync(string queryStrings, int page, int count);

        Task<TVDetail> TvDetailAsync(int id);

    }
}
