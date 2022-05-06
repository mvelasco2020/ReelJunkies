using Microsoft.AspNetCore.Mvc;
using ReelJunkies.Models.ViewModels;
using ReelJunkies.Services.Interfaces;
using System.Threading.Tasks;

namespace ReelJunkies.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _mappingService;

        public ActorsController(IRemoteMovieService tmdbMovieService,
                                IDataMappingService mappingService)
        {
            _tmdbMovieService = tmdbMovieService;
            _mappingService = mappingService;
        }
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _tmdbMovieService.ActorDetailAsync(id);
            actor = _mappingService.MapActorDetail(actor);

            var combinedCredits = await _tmdbMovieService.ActorCombinedCreditsAsync(id);

            var actorCombinedCredits = new VM_ActorMovies()
            {
                ActorDetail = actor,
                CombinedCredits = combinedCredits,
            };

            return View(actorCombinedCredits);
        }
    }
}
