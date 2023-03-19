using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            indexViewModel.games = await _gameService.GetAllAsync();

            if (indexViewModel.genres != null)
            {
                indexViewModel.genres = await _genreService.GetAllAsync();
            }

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId != null)
            {
                var recommendedGames = _recommenderService.GetPersonalizedRecommendations(currentUserId);
                CreateCarouselRanges(recommendedGames);
            }

            return View(indexViewModel);
        }

        public async Task<IActionResult> GameDetails(int? id)
        {
            if (id == null)
                return BadRequest();

            var game = await _gameService.GetByIdAsync((int)id);

            ViewBag.gameRating = _gameService.CalculateGameRatingScore((int)id);
            ViewBag.ratingsNumber = _gameService.GetGameRatingsNumber((int)id);

            return View(game);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.genres = await _genreService.GetAllGenresOrderedByAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameDTO gameDTO, string[] selectedGenres)
        {
            if (selectedGenres != null)
            {
                gameDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _gameService.AddGameWithGenreAsync(gameDTO, selectedGenres);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                indexViewModel.games = await _gameService.Search(searchString);

                return View("Search", indexViewModel);
            }
            return BadRequest();
        }

        public async Task<IActionResult> FilterByGenre(int? gameGenreId)
        {
            if (gameGenreId != null)
            {
                indexViewModel.games = await _gameService.FilterByGenre((int)gameGenreId);

                return View("Search", indexViewModel);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetGamesByCollectionId(int collectionId)
        {
            var collection = await _gameService.GetGamesByCollectionId(collectionId);

            return View("GameCollection", collection);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameDTO gameDTO)
        {
            await _gameService.UpdateAsync(gameDTO);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _gameService.DeleteByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RateGame(int gameId, int rating)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _gameService.AddRatingToGame(currentUserId, gameId, rating);
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
