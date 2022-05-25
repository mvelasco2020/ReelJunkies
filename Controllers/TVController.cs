﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReelJunkies.Data;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public TVController(IRemoteMovieService tmdbMovieService,
                            IDataMappingService tmdbMappingService,
                            ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager)
        {
            _tmdbMovieService = tmdbMovieService;
            _tmdbMappingService = tmdbMappingService;
            _dbContext = dbContext;
            _userManager = userManager;
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
        public async Task<IActionResult> GetTvTrailer([FromQuery] int? id, [FromQuery] int page, [FromQuery] int count)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvVideo = await _tmdbMovieService.GetTvVideos((int)id, page, count);
            if (tvVideo == null) return NotFound();


            return Ok(tvVideo);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostReview([Bind("Content,TVId")] DbTVReview review)
        {
            review.Content.Trim();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("",
                   "Something went wrong while posting a review ");
                TempData["ErrorMessage"] = "Review must be between 20 to 5000 characters";
                return RedirectToAction("Details", "TV", new { id = review.TVId });
            }
            var userID = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userID);

            review.AuthorUsername = user.UserName;
            review.AuthorDetailsId = userID;
            review.CreateDate = System.DateTime.Now;
            review.AuthorDetailsId = userID;
            await _dbContext.AddAsync(review);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Details", "TV", new {id = review.TVId });
        }

    
        public async Task<IActionResult> Details(int id)
        {

            TVDetail tVDetail = new TVDetail();
            try
            {
                tVDetail = await _tmdbMovieService.TvDetailAsync(id);
            }
            catch
            {
                return NotFound();

            }
            TV tv = new TV();
            tv = await _tmdbMappingService.MapTvDetailAsync(tVDetail);

            var errorMessage = TempData["ErrorMessage"];
            if (errorMessage != null)
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            return View(tv);

        }


    }
}



