using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class GenreController : Controller
    {
        IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _genreService.GetAllAsync();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenreDTO genreDTO)
        {
            if (ModelState.IsValid)
            {
                await _genreService.AddAsync(genreDTO);

                return RedirectToAction("Index");
            }
            return BadRequest();
            //var comments = await _genreService.GetAllAsync();
            //return RedirectToAction("GameDetails", "Game");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.DeleteByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
