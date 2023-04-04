using BLL.DTO;
using System.Collections.Generic;
using System;
using Web.ViewModels;
using System.Linq;

namespace Web.Helpers
{
	public class CarouselRangesCreator
	{
		public static void CreateCarouselRanges(List<GameDTO> recommendedGames, IndexViewModel indexViewModel)
		{
			List<GameDTO> recommendedGamesFirstRange = new();
			indexViewModel.recommendedGamesFirstRange = new List<GameDTO>();

			for (int i = 0; i < recommendedGames.Count; i++)
			{
				recommendedGamesFirstRange.Add(recommendedGames[i]);
			}

			indexViewModel.recommendedGamesFirstRange = recommendedGamesFirstRange.Take(3);
			recommendedGames.RemoveRange(0, Math.Min(3, recommendedGamesFirstRange.Count));

			List<GameDTO> recommendedGamesSecondRange = new();
			indexViewModel.recommendedGamesSecondRange = new List<GameDTO>();

			for (int i = 0; i < recommendedGames.Count; i++)
			{
				recommendedGamesSecondRange.Add(recommendedGames[i]);
			}

			indexViewModel.recommendedGamesSecondRange = recommendedGamesSecondRange.Take(3);
			recommendedGames.RemoveRange(0, Math.Min(3, recommendedGamesSecondRange.Count));
		}
	}
}
