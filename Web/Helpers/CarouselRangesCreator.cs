using BLL.DTO;
using System.Collections.Generic;
using Web.ViewModels;
using System.Linq;

namespace Web.Helpers
{
    public class CarouselRangesCreator
	{
		public static void CreateCarouselRanges(List<RecommendedGameDTO> recommendedGames, GameCardsViewModel viewModel, int rangeSize)
		{
            for (int i = 0; i < rangeSize; i++)
			{
				List<RecommendedGameDTO> range = recommendedGames.Skip(i * rangeSize).Take(rangeSize).ToList();

				if (range.Any())
				{
					viewModel.recommendedGames.Add(range);
				}
            }

		}
	}
}
