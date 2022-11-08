using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRecommenderService
    {
        public double CalculateCosineSimilarity(ApplicationUserDTO targetUser, ApplicationUserDTO user);
        public List<ComparedUserModel> GetNearestNeighbors(string targetUserId);
        public List<int> GetPersonalizedRecommendations(string currentUserId); 
    }
}
