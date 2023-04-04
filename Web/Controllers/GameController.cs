﻿using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Helpers;
using Web.ViewModels;

namespace Web.Controllers
{
	public class GameController : Controller
	{
		readonly IGameService _gameService;
		readonly ICommentService _commentService;
		readonly IGenreService _genreService;
		readonly IRecommenderService _recommenderService;

		public GameController(IGameService gameService, ICommentService commentService, IGenreService genreService, IRecommenderService recommenderService)
		{
			_gameService = gameService;
			_commentService = commentService;
			_genreService = genreService;
			_recommenderService = recommenderService;
		}

		public async Task<IActionResult> Index(int? pageNumber)
		{
			var games = await _gameService.GetAllAsync();
			var genres = await _genreService.GetAllAsync();

			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			int pageSize = 8;
			var paginatedGames = PaginatedList<GameDTO>.CreateAsync(games.ToList(), pageNumber ?? 1, pageSize);

			IndexViewModel indexViewModel = new()
			{
				games = games,
				genres = genres,
				PaginatedGames = paginatedGames,
			};

			if (currentUserId != null)
			{
				var recommendedGames = await _recommenderService.GetPersonalizedRecommendationsAsync(currentUserId);
				CarouselRangesCreator.CreateCarouselRanges(recommendedGames, indexViewModel);
			}

			return View(indexViewModel);
		}

		public async Task<IActionResult> GameDetails(int? id)
		{
			if (id == null)
				return BadRequest();

			var game = await _gameService.GetByIdAsync((int)id);

			ViewBag.gameRating = await _gameService.CalculateGameRatingScoreAsync((int)id);
			ViewBag.ratingsNumber = await _gameService.GetGameRatingsCountAsync((int)id);

			return View(game);
		}

		public async Task<IActionResult> SearchGame(string searchString)
		{
			if (!String.IsNullOrEmpty(searchString))
			{
				SearchingViewModel searchingViewModel = new()
				{
					Games = await _gameService.SearchAsync(searchString),
					Genres = await _genreService.GetAllAsync()
				};

				return View(searchingViewModel);
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> FilterGamesByGenre(int? gameGenreId)
		{
			if (gameGenreId != null)
			{
				SearchingViewModel searchingViewModel = new()
				{
					Games = await _gameService.FilterByGenreAsync((int)gameGenreId),
					Genres = await _genreService.GetAllAsync()
				};

				return View(nameof(SearchGame), searchingViewModel);
			}
			return BadRequest();
		}

		public async Task<IActionResult> GetGamesByCollectionId(int collectionId)
		{
			var collection = await _gameService.GetGamesByCollectionIdAsync(collectionId);

			return View("GameCollection", collection);
		}

		[Authorize]
		public async Task<IActionResult> RateGame(int gameId, int ratingScore)
		{
			string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _gameService.AddRatingToGameAsync(currentUserId, gameId, ratingScore);

			return RedirectToAction(nameof(GameDetails), new { id = gameId });
		}

	}
}
