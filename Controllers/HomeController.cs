using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReelJunkies.Data;
using ReelJunkies.Enums;
using ReelJunkies.Models;
using ReelJunkies.Models.TmDb;
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
        private readonly IDataMappingService _dataMappingService;

        public HomeController(ApplicationDbContext context,
                              IRemoteMovieService tmdbMovieService,
                              ILogger<HomeController> logger,
                              IDataMappingService dataMappingService)
        {
            _context = context;
            _tmdbMovieService = tmdbMovieService;
            _logger = logger;
            _dataMappingService = dataMappingService;
        }


        public async Task<IActionResult> Index()
        {
            const int count = 12;
            const int page = 1;
            var Upcomming = await _tmdbMovieService.MovieSearchAsync(MovieCategory.upcoming, count, page);
            Upcomming.results = Upcomming.results.OrderByDescending(x => x.release_date).ToArray();

            var NowPlaying = await _tmdbMovieService.MovieSearchAsync(MovieCategory.now_playing, count, page);
            NowPlaying.results = NowPlaying.results.OrderByDescending(x => x.release_date).ToArray();

            var data = new VM_LandingPage()
            {
                CustomCollections = await _context
                                           .Collection
                                           .Include(c => c.MovieCollection)
                                           .ThenInclude(m => m.Movie)
                                           .ToListAsync(),
                NowPlaying = NowPlaying,
                Popular = await _tmdbMovieService.MovieSearchAsync(MovieCategory.popular, count, page),
                TopRated = await _tmdbMovieService.MovieSearchAsync(MovieCategory.top_rated, count, page),
                Upcomming = Upcomming,
                Horror = await _tmdbMovieService.MovieSearchByGenre("27,53", count),
                TvPopular = await _tmdbMovieService.TVSearchAsync(TVCategory.popular, count, page),
                NetflixOriginals = await _tmdbMovieService.TvDiscoverAsync("&with_networks=213", 1, count),
                NetflixGems = await _tmdbMovieService.TvDiscoverAsync("&sort_by=vote_average.desc&timezone=America%2FNew_York&vote_average.gte=7&vote_count.gte=5&with_networks=213&include_null_first_air_dates=false&with_watch_monetization_types=flatrate&with_status=0&with_type=0", 1, 8)

            };
            return View(data);
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Search(string query, int page)
        {
            if (page == 0)
                page = 1;

            QueryAll queryResult = await _tmdbMovieService.QueryAll(query, page);
            queryResult = _dataMappingService.MapQueryAllDetails(queryResult);
            ViewData["query"] = query;
            ViewData["page"] = page;
            return View(queryResult);
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
