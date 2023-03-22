using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class UserLibraryController : Controller
    {
        readonly IUserGameLibraryService _gameUserLibraryService;
        readonly IGameService _gameService;

        public UserLibraryController(IUserGameLibraryService gameUserLibraryService, IGameService gameService)
        {
            _gameUserLibraryService = gameUserLibraryService;
            _gameService = gameService;
        }

        public async Task<IActionResult> IndexUserLibrary()
        {
            var userLibrary = await _gameUserLibraryService.GetAllByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(userLibrary);
        }

        public async Task<IActionResult> AddGameToUserLibrary(int Id, string UserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _gameUserLibraryService.AddGameToUserLibrary(Id, userId);

            return RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> IsGamePassed(int id, bool isPassed)
        {
            await _gameUserLibraryService.IsPassed(id, isPassed);

            return RedirectToAction(nameof(IndexUserLibrary));
        }

        public async Task<IActionResult> RemoveGame(int id)
        {
            await _gameUserLibraryService.DeleteByIdAsync(id);

            return RedirectToAction(nameof(IndexUserLibrary));
        }
    }
}
