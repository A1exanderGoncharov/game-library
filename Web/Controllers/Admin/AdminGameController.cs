using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Controllers.Admin
{
    [Authorize(Policy = "RequireAdministratorRole")]
    public class AdminGameController : Controller
    {
        readonly IGameService _gameService;
        readonly IGenreService _genreService;

        public AdminGameController(IGameService gameService, IGenreService genreService)
        {
            _gameService = gameService;
            _genreService = genreService;
        }


        [HttpGet]
        public async Task<IActionResult> CreateGame()
        {
            ViewBag.genres = await _genreService.GetAllGenresOrderedByAsync();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateGame(GameDTO gameDTO, string[] selectedGenres)
        {
            if (selectedGenres != null)
            {
                gameDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _gameService.AddGameWithGenreAsync(gameDTO, selectedGenres);
            }
            return RedirectToAction("Index", "Game");
        }


        [HttpGet]
        public async Task<IActionResult> EditGame(int id, int? pageNumber, string actionName, int? gameGenreId, string searchString)
        {
            var game = await _gameService.GetByIdAsync(id);
            var genres = await _genreService.GetAllGenresOrderedByAsync();

            GameToEditViewModel gameToEdit = new()
            {
                GameDto = game,
                PageNumber = pageNumber,
                ActionName = actionName,
                GameGenreId = gameGenreId,
                SearchString = searchString
            };

            ViewBag.genres = genres;
            ViewBag.gameGenres = game.GameGenres;

            return View(gameToEdit);
        }


        [HttpPost]
        public async Task<IActionResult> EditGame(GameToEditViewModel model, string[] selectedGenres)
        {
            await _gameService.UpdateAsync(model.GameDto);
            await _gameService.UpdateGameGenresAsync(model.GameDto.Id, selectedGenres);

            return RedirectToAction(
                model.ActionName,
                "Game",
                new
                {
                    pageNumber = model.PageNumber,
                    gameGenreId = model.GameGenreId,
                    searchString = model.SearchString
                });
        }


        public async Task<IActionResult> DeleteGame(int id, int? pageNumber, string actionName, int? gameGenreId, string searchString)
        {
            await _gameService.DeleteByIdAsync(id);
            return RedirectToAction(actionName, "Game", new { pageNumber, gameGenreId, searchString });
        }
    }
}
