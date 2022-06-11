using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CollectionController : Controller
    {
        ICollectionService _collectionService;
        IUserGameLibraryService _userGameService;

        public CollectionController(ICollectionService collectionService, IUserGameLibraryService userGameService)
        {
            _collectionService = collectionService;
            _userGameService = userGameService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _collectionService.GetAllAsync();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CollectionDTO collectionDTO)
        {
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
            var userGames = await _userGameService.GetAllAsync();
            ViewBag.CollectionId = collectionId;

            return View(userGames);
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
