using BLL.DTO;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IRecommenderService
    {
        public double CalculateCosineSimilarity(ApplicationUserDTO targetUser, ApplicationUserDTO user);
        public List<ComparedUserModel> GetNearestNeighbors(string targetUserId);
        public List<GameDTO> GetPersonalizedRecommendations(string currentUserId);
    }
}
