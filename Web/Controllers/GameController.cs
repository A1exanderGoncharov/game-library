using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Web.Controllers
{
    public class GameController : Controller
    {
        IGameService _gameService;
        ICommentService _commentService;
        IGenreService _genreService;

        public GameController(IGameService gameService, ICommentService commentService, IGenreService genreService)
        {
            _gameService = gameService;
            _commentService = commentService;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _gameService.GetAllAsync();

            return View(model);
        }

        public async Task<IActionResult> GameDetails(int? id)
        {
            var games = await _gameService.GetAllAsync();
            var model = games.FirstOrDefault(g => g.Id == id);

            GetGameRatingScore((int)id);

            ViewBag.ratingsNumber = _gameService.GetGameRatingsNumber((int)id);
            ViewBag.game = model;
            ViewBag.comment = new CommentDTO();

            return View(model);
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

        public IActionResult Comment(int? Id)
        {
            return RedirectToAction("Create", "Comment", new { gameId = Id });
        }

        public async Task<IActionResult> Search(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var games = await _gameService.Search(searchString);
                return View("Index", games);
            }
            return BadRequest();
        }

        public async Task<IActionResult> FilterByGenre(string gameGenre)
        {
            if (!String.IsNullOrEmpty(gameGenre))
            {
                var games = await _gameService.FilterByGenre(gameGenre);

                return View("Index", games);
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
            var games = await _gameService.GetAllAsync();

            var gameDTOToEdit = await _gameService.GetByIdAsync(id);
            return View(gameDTOToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameDTO gameDTO)
        {
            if (ModelState.IsValid)
            {
                await _gameService.UpdateAsync(gameDTO);
                return RedirectToAction(nameof(Index));
            }

            var games = await _gameService.GetAllAsync();

            return View(gameDTO);
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

        public void GetGameRatingScore(int gameId)
        {
            ViewBag.gameRating = _gameService.CalculateGameRatingScore(gameId);
        }

    }
}
