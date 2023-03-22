using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Admin
{
    [Authorize(Policy = "RequireAdministratorRole")]
    public class AdminCommentController : Controller
    {
        readonly ICommentService _commentService;

        public AdminCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var commentDTOToEdit = await _commentService.GetByIdAsync(id);
            return View(commentDTOToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
            {
                await _commentService.UpdateAsync(commentDTO);
                return RedirectToAction("GameDetails", "Game", new { id = commentDTO.GameId });
            }
            return RedirectToAction("GameDetails", "Game", new { id = commentDTO.GameId });
        }
    }
}
