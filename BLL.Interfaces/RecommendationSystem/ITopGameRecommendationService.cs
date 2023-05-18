using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces.RecommendationSystem
{
    public interface ITopGameRecommendationService
    {
        public List<GameDTO> GetRandomTopRatedGames(double minRecommendedGameRating, IEnumerable<GameDTO> gamesDTO, int? count);
        public IEnumerable<GameDTO> GetGamesNotRatedByUser(string userId, IEnumerable<GameDTO> gamesDTO);
    }
}
