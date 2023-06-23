using BLL.DTO;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.RecommendationSystem;
using System;

namespace BLL.Core.RecommendationSystem
{
    public class TopGameRecommendationService : ITopGameRecommendationService
    {
        public IEnumerable<GameDTO> GetGamesNotRatedByUser(string userId, IEnumerable<GameDTO> gamesDTO)
        {
            var gamesWithoutUserRating = gamesDTO
                .Where(g => !g.Ratings.Any(r => r.ApplicationUserId == userId))
                .Select(g => g)
                .Distinct();

            return gamesWithoutUserRating;
        }

        public List<GameDTO> GetRandomTopRatedGames(double minRecommendedGameRating, IEnumerable<GameDTO> gamesDTO, int? count = null)
        {
            var topRatedGames = gamesDTO
                .Where(g => g.Ratings
                .Select(r => r.GameRating).DefaultIfEmpty()
                .Average() >= minRecommendedGameRating)
                .Select(g => g)
                .OrderBy(g => Guid.NewGuid());

            if (count.HasValue)
            {
                return topRatedGames.Take(count.Value).ToList();
            }
            else
            {
                return topRatedGames.ToList();
            }
        }
    }
}
