using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CommentController : Controller
    {
        ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult Create(int GameId, int? ReplyToCommentId, string NicknameToReply)
        {
            ViewBag.gameId = GameId;
            ViewBag.nickname = NicknameToReply;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentDTO commentDTO, int? ReplyToCommentId, int GameId, string NicknameToReply)
        {
            //if (ModelState.IsValid)
            {
                commentDTO.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _commentService.AddAsync(commentDTO);
                return RedirectToAction("GameDetails", "Game", new { id = GameId });
            }

            var comments = await _commentService.GetAllAsync();
            return RedirectToAction("GameDetails", "Game");
        }

        public IActionResult Reply(int Id, int GameId, string Nickname)
        {
            int ReplyToCommentId = Id;

            return RedirectToAction("Create", new { ReplyToCommentId, GameId, NicknameToReply = Nickname });
        }

        public IActionResult ReplyToSecondTier(int ReplyToCommentId, int GameId, string Nickname)
        {
            return RedirectToAction("Create", new { ReplyToCommentId, GameId, NicknameToReply = Nickname });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var comments = await _commentService.GetAllAsync();

            var commentDTOToEdit = await _commentService.GetByIdAsync(id);
            return View(commentDTOToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentDTO commentDTO)
        {
            if (ModelState.IsValid)
            {
                await _commentService.UpdateAsync(commentDTO);
                return RedirectToAction("GameDetails", "Game", new { id = commentDTO.GameId });
            }

            var comments = await _commentService.GetAllAsync();

            return RedirectToAction("GameDetails", "Game", new { id = commentDTO.GameId });
        }

        public async Task<IActionResult> Delete(int id, int gameId)
        {
            await _commentService.DeleteByIdAsync(id);

            return RedirectToAction("GameDetails", "Game", new { id = gameId });
        }
    }
}
