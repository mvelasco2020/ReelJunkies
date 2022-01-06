using ReelJunkies.Models.Database;
using ReelJunkies.Models.TmDb;
using System.Threading.Tasks;

namespace ReelJunkies.Services.Interfaces
{
    public interface IDataMappingService
    {
        Task<Movie> MapMovieDetailAsync(MovieDetail movie);
        ActorDetail MapActorDetail(ActorDetail actor);
    }
}
