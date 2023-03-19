using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class GenreController : Controller
    {
        readonly IGenreService _genreService;

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
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _genreService.DeleteByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
