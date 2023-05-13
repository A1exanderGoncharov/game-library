using BLL.DTO;
using System.Collections.Generic;
using System;
using Web.ViewModels;
using System.Linq;

namespace Web.Helpers
{
	public class CarouselRangesCreator
	{
		public static void CreateCarouselRanges(List<RecommendedGameDTO> recommendedGames, GameCardsViewModel viewModel)
		{
			List<RecommendedGameDTO> recommendedGamesFirstRange = new();
            viewModel.recommendedGamesFirstRange = new List<RecommendedGameDTO>();

			for (int i = 0; i < recommendedGames.Count; i++)
			{
				recommendedGamesFirstRange.Add(recommendedGames[i]);
			}

            viewModel.recommendedGamesFirstRange = recommendedGamesFirstRange.Take(3);
			recommendedGames.RemoveRange(0, Math.Min(3, recommendedGamesFirstRange.Count));

			List<RecommendedGameDTO> recommendedGamesSecondRange = new();
			viewModel.recommendedGamesSecondRange = new List<RecommendedGameDTO>();

			for (int i = 0; i < recommendedGames.Count; i++)
			{
				recommendedGamesSecondRange.Add(recommendedGames[i]);
			}

			viewModel.recommendedGamesSecondRange = recommendedGamesSecondRange.Take(3);
			recommendedGames.RemoveRange(0, Math.Min(3, recommendedGamesSecondRange.Count));
		}
	}
}
