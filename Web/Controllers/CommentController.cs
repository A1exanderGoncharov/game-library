using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CommentController : Controller
    {
        readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(int? replyToCommentId, int gameId, string nicknameToReply, string content)
        {
            CommentDTO commentDTO = new()
            {
                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                GameId = gameId,
                NicknameToReply = nicknameToReply,
                Content = content,
                ReplyToCommentId = replyToCommentId
            };

            await _commentService.AddAsync(commentDTO);

            return RedirectToAction("GameDetails", "Game", new { id = gameId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var commentDTOToEdit = await _commentService.GetByIdAsync(id);

            if (commentDTOToEdit == null)
            {
                return NotFound();
            }

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (commentDTOToEdit.ApplicationUserId != currentUserId)
            {
                return View("../Account/AccessDenied");
            }

            return View(commentDTOToEdit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditComment(CommentDTO commentDTO)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (commentDTO.ApplicationUserId == currentUserId && ModelState.IsValid)
            {
                await _commentService.UpdateAsync(commentDTO);
                return RedirectToAction("GameDetails", "Game", new { id = commentDTO.GameId });
            }
            return View("../Account/AccessDenied");
        }

        [Authorize]
        public async Task<IActionResult> DeleteComment(int id, int gameId)
        {
            await _commentService.DeleteByIdAsync(id);

            return RedirectToAction("GameDetails", "Game", new { id = gameId });
        }
    }
}
