using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReelJunkies.Data;
using ReelJunkies.Enums;
using ReelJunkies.Models;
using ReelJunkies.Models.ViewModels;
using ReelJunkies.Services.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReelJunkies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly IRemoteMovieService _tmdbMovieService;

        public HomeController(ApplicationDbContext context,
                              IRemoteMovieService tmdbMovieService,
                              ILogger<HomeController> logger
                              )
        {
            _context = context;
            _tmdbMovieService = tmdbMovieService;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            const int count = 4;
            var Upcomming = await _tmdbMovieService.MovieSearchAsync(MovieCategory.upcoming, 4);
            Upcomming.results = Upcomming.results.OrderByDescending(x => x.release_date).ToArray();

            var NowPlaying = await _tmdbMovieService.MovieSearchAsync(MovieCategory.now_playing, 8);
            NowPlaying.results = NowPlaying.results.OrderByDescending(x => x.release_date).ToArray();

            var data = new VM_LandingPage()
            {
                CustomCollections = await _context
                                           .Collection
                                           .Include(c => c.MovieCollection)
                                           .ThenInclude(m => m.Movie)
                                           .ToListAsync(),
                NowPlaying = NowPlaying,
                Popular = await _tmdbMovieService.MovieSearchAsync(MovieCategory.popular, count),
                TopRated = await _tmdbMovieService.MovieSearchAsync(MovieCategory.top_rated, count),
                Upcomming = Upcomming,
                Horror = await _tmdbMovieService.MovieSearchByGenre("27,53"),
                Indies = await _tmdbMovieService.MovieSearchByGenre("27,53"),

            };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
