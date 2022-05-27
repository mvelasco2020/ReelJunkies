﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ReelJunkies.Data;
using ReelJunkies.Enums;
using ReelJunkies.Models.Database;
using ReelJunkies.Models.Settings;
using ReelJunkies.Models.TmDb;
using ReelJunkies.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ReelJunkies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppSettings _appsettings;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _tmdbMappingService;
        private readonly UserManager<IdentityUser> _userManager;

        public MoviesController(IOptions<AppSettings> appsettings,
                    ApplicationDbContext context,
                    IImageService imageService,
                    IDataMappingService tmdbMappingService,
                    IRemoteMovieService tmdbMovieService,
                    UserManager<IdentityUser> userManager)
        {
            _appsettings = appsettings.Value;
            _context = context;
            _imageService = imageService;
            _tmdbMappingService = tmdbMappingService;
            _tmdbMovieService = tmdbMovieService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Import()
        {
            var movies = await _context.Movie.ToListAsync();

            return View(movies);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int id)
        {
            //warn and redirect user if movie already exist
            if (_context.Movie.Any(m => m.TmDbMovieId == id))
            {
                var localMovie = await _context
                                        .Movie
                                        .FirstOrDefaultAsync(
                                        m => m.TmDbMovieId == id);
                return RedirectToAction("Details",
                                        "Movies",
                                        new
                                        {
                                            id = localMovie.Id,
                                            local = true
                                        });
            }

            //get raw data from api
            var movieDetail = await _tmdbMovieService.MovieDetailAsync(id);

            //run data through mapping procedure
            var movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);

            //add new movie to Db
            _context.Add(movie);
            await _context.SaveChangesAsync();

            //Assign it to default All Collection
            await AddToMovieCollection(movie.Id, _appsettings.ReelJunkiesSettings.DefaultCollection.Name);

            return RedirectToAction("Import");

        }


        public async Task<IActionResult> Index(int page = 1)
        {
            string queryStrings = "&sort_by=vote_count.desc&include_adult=false&include_video=false&vote_average.gte=7&with_watch_monetization_types=flatrate";
            var movies = await _tmdbMovieService.MovieDiscoverAsync(queryStrings, page, 30);
            return View(movies);
        }


        // GET: Create
        public IActionResult Create()
        {
            ViewData["CollectionId"] = new SelectList(_context.Collection, "Id", "Name");

            return View();
        }

        // POST: Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TmDbMovieId,Title,TagLine,Overview,RunTime,ReleaseDate,Rating,VoteAverage,Poster,PosterType,Backdrop,BackDropType,TrailerUrl")] Movie movie, int collectionId)
        {
            if (ModelState.IsValid)
            {
                movie.PosterType = movie.PosterFile?.ContentType;
                movie.Poster = await _imageService.EncodeImageAsync(movie.PosterFile);
                movie.BackDropType = movie.BackdropFile?.ContentType;
                movie.Backdrop = await _imageService.EncodeImageAsync(movie.BackdropFile);


                _context.Add(movie);
                await _context.SaveChangesAsync();

                await AddToMovieCollection(movie.Id, collectionId);

                return RedirectToAction("Index", "MovieCollections");
            }
            return View(movie);
        }

        // GET: Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TmDbMovieId,Title,TagLine,Overview,RunTime,ReleaseDate,Rating,VoteAverage,Poster,PosterType,Backdrop,BackDropType,TrailerUrl")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (movie.PosterFile is not null)
                    {
                        movie.PosterType = movie.PosterFile?.ContentType;
                        movie.Poster = await _imageService.EncodeImageAsync(movie.PosterFile);
                    }

                    if (movie.BackdropFile is not null)
                    {
                        movie.BackDropType = movie.BackdropFile?.ContentType;
                        movie.Backdrop = await _imageService.EncodeImageAsync(movie.BackdropFile);
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Movies", new { id = movie.Id, local = true });
            }
            return View(movie);
        }

        // GET: Temp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Temp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Library", "Movies");
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostReview([Bind("Content,MovieId")] DbMovieReview review)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("",
                   "Something went wrong while posting a review ");
                TempData["ErrorMessage"] = "Review must be between 20 to 5000 characters";
                return RedirectToAction("Details", "Movies", new { id = review.MovieId });
            }
            review.Content.Trim();
            var userID = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userID);
            review.AuthorUsername = user.UserName;
            review.AuthorDetailsId = userID;
            review.CreateDate = System.DateTime.Now;
            await _context.AddAsync(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Movies", new { id = review.MovieId });
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview([Bind("Content", "MovieId", "Id")] DbMovieReview review)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("",
                   "Something went wrong while editing a review ");
                TempData["ErrorMessage"] = "Review must be between 20 to 5000 characters";
                return RedirectToAction("Details", "Movies", new { id = review.MovieId });
            }

            DbMovieReview findReview = await _context
                                                .DbMovieReview
                                                .FirstOrDefaultAsync(r =>
                                               r.Id == review.Id);


            if (_userManager.GetUserId(User) != findReview.AuthorDetailsId) return Unauthorized();

            if (findReview == null)
            {
                return RedirectToAction("Details", "Movies", new { id = review.MovieId });
            }

            try
            {
                findReview.Content = review.Content;
                findReview.UpdateDate = System.DateTime.Now;
                await _context.SaveChangesAsync();

            }
            catch (System.Exception)
            {

                throw;
            }
            return RedirectToAction("Details", "Movies", new { id = review.MovieId });
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (id == 0 || id == null)
                return BadRequest();


            DbMovieReview review = await _context
                                     .DbMovieReview
                                     .FindAsync(id);

            if (review is null)
                return NotFound();

            if (review.AuthorDetailsId != _userManager.GetUserId(User))
                return Unauthorized();

            _context.DbMovieReview.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Movies", new { id = review.MovieId });
        }

        public async Task<IActionResult> Details(int? id, bool local = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movie = new();
            if (local)
            {
                //get movie data from db
                movie = await _context
                                .Movie
                                .Include(m => m.Cast)
                                .Include(m => m.Crew)
                                .FirstOrDefaultAsync(m => m.Id == id);
            }
            else
            {
                //get the movie data from tmdb api
                var movieDetail = await _tmdbMovieService.MovieDetailAsync((int)id);
                movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);
            }

            if (movie == null)
            {
                return NotFound();
            }

            ViewData["Local"] = local;

            return View(movie);
        }


        public async Task<ActionResult> GetMovieByCategory(MovieCategory category, int count = 16, int page = 1)
        {
            var movieSearch = await _tmdbMovieService.MovieSearchAsync(category, count, page);
            ViewData["category"] = category;
            return View(movieSearch);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieTrailer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieDetail = await _tmdbMovieService.MovieDetailAsync((int)id);
            Movie movie = new Movie();
            movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);

            if (movie == null) return NotFound();
            string url = movie.TrailerUrl;
            return Ok(url);

        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }

        private async Task AddToMovieCollection(int movieId, string collectionName)
        {
            var collection = await _context
                                    .Collection
                                    .FirstOrDefaultAsync(c =>
                                    c.Name == collectionName);


            _context.Add(new MovieCollection()
            {
                CollectionId = collection.Id,
                MovieId = movieId
            });

            await _context.SaveChangesAsync();
        }


        private async Task AddToMovieCollection(int movieId, int collectionId)
        {
            _context.Add(new MovieCollection()
            {
                CollectionId = collectionId,
                MovieId = movieId
            });

            await _context.SaveChangesAsync();
        }
    }
}
