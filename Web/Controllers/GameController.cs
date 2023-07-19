using BLL.DTO;
using BLL.Interfaces;
using BLL.Interfaces.RecommendationSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class GameController : Controller
    {
        readonly IGameService _gameService;
        readonly ICommentService _commentService;
        readonly IGenreService _genreService;
        readonly IRecommendationService _recommendationService;

        public GameController(IGameService gameService, ICommentService commentService, IGenreService genreService, IRecommendationService recommendationService)
        {
            _gameService = gameService;
            _commentService = commentService;
            _genreService = genreService;
            _recommendationService = recommendationService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var games = await _gameService.GetAllAsync();
            var genres = await _genreService.GetAllAsync();

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int pageSize = 8;
            var paginatedGames = PaginatedList<GameDTO>.CreateAsync(games.ToList(), pageNumber ?? 1, pageSize);

            GameCardsViewModel viewModel = new()
            {
                Games = games,
                Genres = genres,
                PaginatedGames = paginatedGames,
            };

            if (currentUserId != null)
            {
                int recGamesCount = 6;
                double minAvgRating = 4;

                var recommendedGames = await _recommendationService.GetAgregatedRecommendationsAsync(recGamesCount, minAvgRating, currentUserId);

                int rangeSize = 3;
                CarouselRangesCreator.CreateCarouselRanges(recommendedGames, viewModel, rangeSize);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> GameDetails(int id)
        {
            var game = await _gameService.GetByIdAsync(id);

            ViewBag.gameRating = await _gameService.CalculateGameRatingScoreAsync(id);
            ViewBag.ratingsNumber = await _gameService.GetGameRatingsCountAsync(id);
            ViewBag.hasUserRated = await _gameService.HasUserRatedGame(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(game);
        }

        public async Task<IActionResult> SearchGame(string searchString, int? pageNumber)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var genres = await _genreService.GetAllAsync();
                var games = await _gameService.SearchAsync(searchString);

                int pageSize = 8;
                var paginatedGames = PaginatedList<GameDTO>.CreateAsync(games.ToList(), pageNumber ?? 1, pageSize);

                GameCardsViewModel viewModel = new()
                {
                    Games = games,
                    Genres = genres,
                    PaginatedGames = paginatedGames
                };

                ViewBag.searchString = searchString;

                return View(viewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> FilterGamesByGenre(int? gameGenreId, int? pageNumber)
        {
            if (gameGenreId != null)
            {
                var genres = await _genreService.GetAllAsync();
                var games = await _gameService.FilterByGenreAsync((int)gameGenreId);

                int pageSize = 8;
                var paginatedGames = PaginatedList<GameDTO>.CreateAsync(games.ToList(), pageNumber ?? 1, pageSize);

                GameCardsViewModel viewModel = new()
                {
                    Games = games,
                    Genres = genres,
                    PaginatedGames = paginatedGames
                };

                ViewBag.gameGenreId = gameGenreId;
                ViewBag.gameGenre = await _genreService.GetByIdAsync((int)gameGenreId);

                return View(viewModel);
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

            return RedirectToAction(nameof(GameDetails), new { id = gameId });
        }

    }
}
