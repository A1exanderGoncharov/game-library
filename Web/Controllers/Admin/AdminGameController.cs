using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            return View(game);
        }


        [HttpPost]
        public async Task<IActionResult> EditGame(GameDTO gameDTO)
        {
            await _gameService.UpdateAsync(gameDTO);
            return RedirectToAction("Index", "Game");
        }


        public async Task<IActionResult> DeleteGame(int id)
        {
            await _gameService.DeleteByIdAsync(id);
            return RedirectToAction("Index", "Game");
        }
    }
}
