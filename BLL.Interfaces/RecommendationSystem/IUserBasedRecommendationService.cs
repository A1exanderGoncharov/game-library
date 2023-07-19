using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces.RecommendationSystem
{
    public interface IUserBasedRecommendationService
    {
        public Task<List<GameDTO>> RetrieveNeighborsGamesForRecommendations(string currentUserId, List<ComparedUserModel> neighbors, double minAverageRating, bool useRandomOrder = false);
        public List<ComparedUserModel> GetNearestNeighbors(ApplicationUserDTO targetUserDTO, IEnumerable<ApplicationUserDTO> usersDTO, double minAverageOfUserRatings, int userCount);
    }
}
