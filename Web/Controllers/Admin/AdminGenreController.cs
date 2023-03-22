using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Admin
{
    [Authorize(Policy = "RequireAdministratorRole")]
    public class AdminGenreController : Controller
    {
        readonly IGenreService _genreService;

        public AdminGenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

		[HttpGet]
		public async Task<IActionResult> IndexGenres()
        {
            var genres = await _genreService.GetAllAsync();

            return View(genres);
        }

        [HttpGet]
        public IActionResult CreateGenre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreDTO genreDTO)
        {
            if (ModelState.IsValid)
            {
                await _genreService.AddAsync(genreDTO);

                return RedirectToAction(nameof(IndexGenres));
            }
            return BadRequest();
        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            await _genreService.DeleteByIdAsync(id);
            return RedirectToAction(nameof(IndexGenres));
        }

    }
}
