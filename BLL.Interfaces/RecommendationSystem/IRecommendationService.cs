using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces.RecommendationSystem
{
    public interface IRecommendationService
    {
        public Task<IEnumerable<RecommendedGameDTO>> GetAgregatedRecommendationsAsync(int totalCount, double minAverageRating, string currentUserId);
        public Task<IEnumerable<RecommendedGameDTO>> GetUserBasedRecommendationsAsync(string currentUserId, RecommendationOptions options);
        public Task<IEnumerable<RecommendedGameDTO>> GetTopRatedGamesRecommendationsAsync(double minAverageRating, string userId, int? count);
    }
}
