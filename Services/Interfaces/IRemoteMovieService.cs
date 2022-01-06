using ReelJunkies.Enums;
using ReelJunkies.Models.TmDb;
using System.Threading.Tasks;

namespace ReelJunkies.Services.Interfaces
{
    public interface IRemoteMovieService
    {
        Task<MovieDetail> MovieDetailAsync(int id);
        Task<MovieSearch> MovieSearchAsync(MovieCategory category, int count);

        Task<ActorDetail> ActorDetail(int id);
    }
}
