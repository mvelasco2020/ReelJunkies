using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReelJunkies.Enums;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.TmDb;
using ReelJunkies.Services.Interfaces;
using System.Threading.Tasks;

namespace ReelJunkies.Controllers
{
    public class TVController : Controller
    {
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _tmdbMappingService;

        public TVController(IRemoteMovieService tmdbMovieService,
                            IDataMappingService tmdbMappingService)
        {
            _tmdbMovieService = tmdbMovieService;
            _tmdbMappingService = tmdbMappingService;
        }
        public async Task<ActionResult> GetTVByCategory(TVCategory category,
                                                         int count = 16,
                                                         int page = 1)
        {
            var tvSearch = await _tmdbMovieService
                                    .TVSearchAsync(category, count, page);

            ViewData["category"] = category;

            return View(tvSearch);
        }


        [HttpGet]
        public async Task<IActionResult> GetTvTrailer([FromQuery]int? id, [FromQuery] int page, [FromQuery] int count)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvVideo = await _tmdbMovieService.GetTvVideos((int)id, page, count);
            if (tvVideo == null) return NotFound();


            return Ok(tvVideo);
        }


        public async Task<IActionResult> Details(int id)
        {
            TVDetail tVDetail = new TVDetail();
            tVDetail = await _tmdbMovieService.TvDetailAsync(id);
            try
            {

            }
            catch
            {
                return NotFound();

            }
            TV tv = new TV();
            tv = await _tmdbMappingService.MapTvDetailAsync(tVDetail);



            return View(tv);

        }


    }
}



