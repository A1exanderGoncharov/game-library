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
        [HttpGet]
        public IActionResult CreateComment(int GameId, int? ReplyToCommentId, string NicknameToReply)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(int? ReplyToCommentId, int GameId, string NicknameToReply, string Content)
        {
            CommentDTO commentDTO = new();

            if (ReplyToCommentId != null)
            {
                commentDTO.ReplyToCommentId = ReplyToCommentId;
                commentDTO.NicknameToReply = NicknameToReply;
            }

            commentDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            commentDTO.GameId = GameId;
            commentDTO.NicknameToReply = NicknameToReply;
            commentDTO.Content = Content;

            await _commentService.AddAsync(commentDTO);
            return RedirectToAction("GameDetails", "Game", new { id = GameId });
        }

        [Authorize]
        public IActionResult ReplyToParentComment(int Id, int GameId, string Nickname)
        {
            int ReplyToCommentId = Id;

            return RedirectToAction(nameof(CreateComment), new { ReplyToCommentId, GameId, NicknameToReply = Nickname });
        }

        [Authorize]
        public IActionResult ReplyToChildComment(int ReplyToCommentId, int GameId, string Nickname)
        {
            return RedirectToAction(nameof(CreateComment), new { ReplyToCommentId, GameId, NicknameToReply = Nickname });
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
