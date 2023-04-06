using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class CollectionController : Controller
    {
        readonly ICollectionService _collectionService;
        readonly IUserGameLibraryService _userGameService;

        public CollectionController(ICollectionService collectionService, IUserGameLibraryService userGameService)
        {
            _collectionService = collectionService;
            _userGameService = userGameService;
        }

        public async Task<IActionResult> IndexCollections()
        {        
            var collections = await _collectionService.GetAllByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(collections);
        }

        [HttpGet]
        public IActionResult CreateCollection()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCollection(CollectionDTO collectionDTO)
        {
            collectionDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                await _collectionService.AddAsync(collectionDTO);

                return RedirectToAction(nameof(IndexCollections));
            }
            return BadRequest();
        }

        public async Task<IActionResult> SelectGamesToCollection(int collectionId)
        {
            var userGames = await _userGameService.GetAllByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewBag.CollectionId = collectionId;

            return View(userGames);
        }

        public async Task<IActionResult> AddGamesToCollection(int collectionId, List<string> selectedGames)
        {
            await _collectionService.AddGamesToCollectionAsync(collectionId, selectedGames);

            return RedirectToAction("GetGamesByCollectionId", "Game", new { CollectionId = collectionId });
        }

        public async Task<IActionResult> DeleteCollection(int id)
        {
            await _collectionService.DeleteByIdAsync(id);

            return RedirectToAction(nameof(IndexCollections));
        }

        [HttpGet]
        public async Task<IActionResult> EditCollection(int id)
        {
            var collection = await _collectionService.GetByIdAsync(id);

            return View(collection);
        }

        [HttpPost]
        public async Task<IActionResult> EditCollection(CollectionDTO collectionDTO)
        {
            await _collectionService.UpdateAsync(collectionDTO);

            return RedirectToAction(nameof(IndexCollections));
        }
    }
}
