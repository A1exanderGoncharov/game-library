using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class RecommenderService : IRecommenderService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public RecommenderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public double CalculateCosineSimilarity(ApplicationUserDTO targetUser, ApplicationUserDTO user)
        {
            double numeratorSum = 0;
            double firstDenumeratorSum = 0;
            double secondDenumeratorSum = 0;

            List<RatingDTO> vector1 = new();
            List<RatingDTO> vector2 = new();

            for (int i = 0; i < targetUser.Ratings.Count; i++)
            {
                for (int j = 0; j < user.Ratings.Count; j++)
                {
                    if (targetUser.Ratings[i].GameId == user.Ratings[j].GameId)
                    {
                        vector1.Add(targetUser.Ratings[i]);
                        vector2.Add(user.Ratings[j]);
                    }
                }
            }

            for (int i = 0; i < vector1.Count; i++)
            {
                numeratorSum += vector1[i].GameRating * vector2[i].GameRating;
                firstDenumeratorSum += Math.Pow(vector1[i].GameRating, 2);
                secondDenumeratorSum += Math.Pow(vector2[i].GameRating, 2);
            }

            vector1.Clear();
            vector2.Clear();

            return numeratorSum / (Math.Sqrt(firstDenumeratorSum) * Math.Sqrt(secondDenumeratorSum));
        }

        public List<ComparedUserModel> GetNearestNeighbors(string targetUserId)
        {
            var targetUser = _unitOfWork.UserRepository.GetAllAsync().FirstOrDefault(u => u.Id == targetUserId);
            var targetUserDTO = _mapper.Map<ApplicationUser, ApplicationUserDTO>(targetUser);

            var usersEntities = _unitOfWork.UserRepository.GetAllAsync().ToList();
            var usersDTO = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserDTO>>(usersEntities);

            List<ComparedUserModel> neighbors = new();

            foreach (var user in usersDTO)
            {
                if (targetUserId != user.Id)
                {
                    neighbors.Add(new ComparedUserModel
                    {
                        ComparedUserId = user.Id,
                        SimilarityScore = CalculateCosineSimilarity(targetUserDTO, user)
                    });
                }
            }

            var similarUsers = neighbors.OrderByDescending(c => c.SimilarityScore).Take(5).ToList();

            return similarUsers;
        }

        public List<int> GetPersonalizedRecommendations(string currentUserId)
        {
            List<int> recommendations = new();

            var neighbors = GetNearestNeighbors(currentUserId);
            var gamesIdOfTargetUser = _unitOfWork.RatingRepository.GetAllAsync().Result
                .Where(x => x.ApplicationUserId == currentUserId)
                .Select(x => x.GameId)
                .ToList();
            List<int> gamesIdOfComparedUser = new();

            foreach (var neighbor in neighbors)
            {
                gamesIdOfComparedUser = _unitOfWork.RatingRepository.GetAllAsync().Result
                    .Where(x => x.ApplicationUserId == neighbor.ComparedUserId)
                    .Select(x => x.GameId)
                    .ToList();
            }

            recommendations = CompareGamesId(gamesIdOfTargetUser, gamesIdOfComparedUser);

            return recommendations;
        }

        public List<int> CompareGamesId(List<int> gamesIdOfTargetUser, List<int> gamesIdOfComparedUser)
        {
            List<int> pairValuesOnly = new();

            if (gamesIdOfTargetUser.Count > gamesIdOfComparedUser.Count)
            {
                pairValuesOnly = gamesIdOfTargetUser.Except(gamesIdOfComparedUser).ToList();
            }

            if (gamesIdOfTargetUser.Count < gamesIdOfComparedUser.Count)
            {
                pairValuesOnly = gamesIdOfComparedUser.Except(gamesIdOfTargetUser).ToList();
            }

            return pairValuesOnly;

        }
    }
}
