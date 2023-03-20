using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class UserGameLibraryController : Controller
    {
        IUserGameLibraryService _gameUserLibraryService;
        IGameService _gameService;

        public UserGameLibraryController(IUserGameLibraryService gameUserLibraryService, IGameService gameService)
        {
            _gameUserLibraryService = gameUserLibraryService;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _gameUserLibraryService.GetAllAsync();

            return View(model.Where(x => x.ApplicationUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        public async Task<IActionResult> AddGameToUserLibrary(int Id, string UserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _gameUserLibraryService.AddGameToUserLibrary(Id, userId);

            return RedirectToAction("Index", "Game");
        }

        public async Task<IActionResult> IsPassed(int id, bool isPassed)
        {
            await _gameUserLibraryService.IsPassed(id, isPassed);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _gameUserLibraryService.DeleteByIdAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
