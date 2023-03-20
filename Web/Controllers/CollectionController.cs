using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CollectionController : Controller
    {
        readonly ICollectionService _collectionService;
        readonly IUserGameLibraryService _userGameService;

        public CollectionController(ICollectionService collectionService, IUserGameLibraryService userGameService)
        {
            _collectionService = collectionService;
            _userGameService = userGameService;
        }

        public async Task<IActionResult> Index()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            var models = await _collectionService.GetAllByUserId(UserId);

            return View(models);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CollectionDTO collectionDTO)
        {
            collectionDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                await _collectionService.AddAsync(collectionDTO);

                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> SelectGamesToCollection(int collectionId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userGames = await _userGameService.GetAllAsync();
            var model = userGames.Where(x => x.ApplicationUserId == UserId);

            ViewBag.CollectionId = collectionId;

            return View(model);
        }

        public async Task<IActionResult> AddGamesToCollection(int collectionId, List<string> selectedGames)
        {
            await _collectionService.AddGamesToCollection(collectionId, selectedGames);
            return RedirectToAction("GetGamesByCollectionId", "Game", new { CollectionId = collectionId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _collectionService.DeleteByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
