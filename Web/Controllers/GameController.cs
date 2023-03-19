using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Web.Controllers
{
    public class GameController : Controller
    {
        IGameService _gameService;
        ICommentService _commentService;
        IGenreService _genreService;
        IRecommenderService _recommenderService;
        readonly IndexViewModel indexViewModel;

        public GameController(IGameService gameService, ICommentService commentService, IGenreService genreService, IRecommenderService recommenderService)
        {
            _gameService = gameService;
            _commentService = commentService;
            _genreService = genreService;
            _recommenderService = recommenderService;
            indexViewModel = new();
            indexViewModel.genres = _genreService.GetAllAsync().Result;
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

            var gameModel = await _gameService.GetByIdAsync((int)id);

            ViewBag.gameRating = _gameService.CalculateGameRatingScore((int)id);

            ViewBag.ratingsNumber = _gameService.GetGameRatingsNumber((int)id);

            return View(gameModel);
        }

        public async Task<IActionResult> Create()
        {
            var genres = await _genreService.GetAllAsync();
            ViewBag.genres = genres.OrderBy(g => g.Name).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameDTO gameDTO, List<string> selectedGenres)
        {
            if (selectedGenres != null)
            {
                var genresEntities = await _genreService.GetAllAsync();

                gameDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _gameService.AddGameWithGenreAsync(gameDTO, selectedGenres);
            }
            return RedirectToAction(nameof(Index));


            //var games = await _gameService.GetAllAsync();
            //return View(gameDTO);
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
        public async Task<IActionResult> GetGamesByCollectionId(int CollectionId)
        {
            var collection = await _gameService.GetGamesByCollectionId(CollectionId);

            return View("GameCollection", collection);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var gameDTOToEdit = await _gameService.GetByIdAsync(id);
            return View(gameDTOToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameDTO gameDTO)
        {
            //if (ModelState.IsValid)
            {
                await _gameService.UpdateAsync(gameDTO);
                return RedirectToAction(nameof(Index));
            }

            //var games = await _gameService.GetAllAsync();

            //return View(gameDTO);
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
