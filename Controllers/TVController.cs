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
        public async Task<IActionResult> GetMovieTrailer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvDetail = await _tmdbMovieService.TvDetailAsync((int)id);
            if (tvDetail == null) return NotFound();

            TV tv = new TV();
            tv = await _tmdbMappingService.MapTvDetailAsync(tvDetail);
            return Ok(tv);

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

        // GET: TVController1
        //      public ActionResult Index()
        //      {
        //          return View();
        //      }
        //
        //      // GET: TVController1/Details/5
        //      public ActionResult Details(int id)
        //   {
        //       return View();
        //   }
        // //
        // //     // GET: TVController1/Create
        // //     public ActionResult Create()
        //   {
        //       return View();
        //   }
        //
        //      // POST: TVController1/Create
        //      [HttpPost]
        //      [ValidateAntiForgeryToken]
        //      public ActionResult Create(IFormCollection collection)
        //     {
        //         try
        //         {
        //             return RedirectToAction(nameof(Index));
        //         }
        //         catch
        //         {
        //             return View();
        //         }
        //     }
        ////
        ////      // GET: TVController1/Edit/5
        ////      public ActionResult Edit(int id)
        //     {
        //         return View();
        //     }
        ////
        ////      // POST: TVController1/Edit/5
        ////      [HttpPost]
        ////      [ValidateAntiForgeryToken]
        ////      public ActionResult Edit(int id, IFormCollection collection)
        ////      {
        ////          try
        ////          {
        ////              return RedirectToAction(nameof(Index));
        ////          }
        ////          catch
        ////          {
        ////              return View();
        ////          }
        ////      }
        ////
        ////      // GET: TVController1/Delete/5
        ////      public ActionResult Delete(int id)
        //     {
        //         return View();
        //     }
        //
        //      // POST: TVController1/Delete/5
        //      [HttpPost]
        //      [ValidateAntiForgeryToken]
        //      public ActionResult Delete(int id, IFormCollection collection)
        //      {
        //          try
        //          {
        //              return RedirectToAction(nameof(Index));
        //          }
        //          catch
        //          {
        //              return View();
        //          }
        //      }
        //  }
    }
}
