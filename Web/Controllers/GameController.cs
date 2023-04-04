using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Helpers;
using Web.ViewModels;

namespace Web.Controllers
{
    public class GameController : Controller
    {
        readonly IGameService _gameService;
        readonly ICommentService _commentService;
        readonly IGenreService _genreService;
        readonly IRecommenderService _recommenderService;
        readonly IndexViewModel indexViewModel;

        public GameController(IGameService gameService, ICommentService commentService, IGenreService genreService, IRecommenderService recommenderService)
        {
            _gameService = gameService;
            _commentService = commentService;
            _genreService = genreService;
            _recommenderService = recommenderService;
            indexViewModel = new()
            {
                genres = _genreService.GetAllAsync().Result
            };
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var games = await _gameService.GetAllAsync();
            indexViewModel.games = games;

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId != null)
            {
                var recommendedGames = await _recommenderService.GetPersonalizedRecommendationsAsync(currentUserId);
                CreateCarouselRanges(recommendedGames);
            }

            int pageSize = 8;
            indexViewModel.PaginatedGames = PaginatedList<GameDTO>.CreateAsync(games.ToList(), pageNumber ?? 1, pageSize);

            return View(indexViewModel);
        }
        
        public async Task<IActionResult> GameDetails(int? id)
        {
            if (id == null)
                return BadRequest();

            var game = await _gameService.GetByIdAsync((int)id);

            ViewBag.gameRating = await _gameService.CalculateGameRatingScoreAsync((int)id);
            ViewBag.ratingsNumber = await _gameService.GetGameRatingsCountAsync((int)id);

            return View(game);
        }

        public async Task<IActionResult> SearchGame(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                indexViewModel.games = await _gameService.SearchAsync(searchString);

                return View(indexViewModel);
            }
            return BadRequest();
        }

        public async Task<IActionResult> FilterGamesByGenre(int? gameGenreId)
        {
            if (gameGenreId != null)
            {
                indexViewModel.games = await _gameService.FilterByGenreAsync((int)gameGenreId);

                return View(nameof(SearchGame), indexViewModel);
            }
            return BadRequest();
        }

        public async Task<IActionResult> GetGamesByCollectionId(int collectionId)
        {
            var collection = await _gameService.GetGamesByCollectionIdAsync(collectionId);

            return View("GameCollection", collection);
        }

        [Authorize]
        public async Task<IActionResult> RateGame(int gameId, int ratingScore)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _gameService.AddRatingToGameAsync(currentUserId, gameId, ratingScore);
            return RedirectToAction("GameDetails", new { id = gameId });
        }

        private void CreateCarouselRanges(List<GameDTO> recommendedGames)
        {
            List<GameDTO> recommendedGamesFirstRange = new();
            indexViewModel.recommendedGamesFirstRange = new List<GameDTO>();

            for (int i = 0; i < recommendedGames.Count; i++)
            {
                recommendedGamesFirstRange.Add(recommendedGames[i]);
            }

            indexViewModel.recommendedGamesFirstRange = recommendedGamesFirstRange.Take(3);
            recommendedGames.RemoveRange(0, Math.Min(3, recommendedGamesFirstRange.Count));

            List<GameDTO> recommendedGamesSecondRange = new();
            indexViewModel.recommendedGamesSecondRange = new List<GameDTO>();

            for (int i = 0; i < recommendedGames.Count; i++)
            {
                recommendedGamesSecondRange.Add(recommendedGames[i]);
            }

            indexViewModel.recommendedGamesSecondRange = recommendedGamesSecondRange.Take(3);
            recommendedGames.RemoveRange(0, Math.Min(3, recommendedGamesSecondRange.Count));
        }

    }
}
