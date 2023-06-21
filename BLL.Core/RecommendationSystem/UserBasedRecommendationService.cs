using AutoMapper;
using BLL.DTO;
using BLL.Interfaces.RecommendationSystem;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core.RecommendationSystem
{
    public class UserBasedRecommendationService : IUserBasedRecommendationService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public UserBasedRecommendationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private static double CalculateCosineSimilarity(ApplicationUserDTO targetUserDTO, ApplicationUserDTO userDTO)
        {
            double numeratorSum = 0;
            double firstDenumeratorSum = 0;
            double secondDenumeratorSum = 0;

            GetUserRatingsOnEqualGames(targetUserDTO, userDTO, out List<RatingDTO> targetUserRatingsDTO, out List<RatingDTO> userRatingsToCompareDTO);

            for (int i = 0; i < targetUserRatingsDTO.Count; i++)
            {
                numeratorSum += targetUserRatingsDTO[i].GameRating * userRatingsToCompareDTO[i].GameRating;
                firstDenumeratorSum += Math.Pow(targetUserRatingsDTO[i].GameRating, 2);
                secondDenumeratorSum += Math.Pow(userRatingsToCompareDTO[i].GameRating, 2);
            }

            return numeratorSum / (Math.Sqrt(firstDenumeratorSum) * Math.Sqrt(secondDenumeratorSum));
        }

        private static void GetUserRatingsOnEqualGames(ApplicationUserDTO targetUserDTO, ApplicationUserDTO userDTO, out List<RatingDTO> targetUserRatingsDTO, out List<RatingDTO> userRatingsToCompareDTO)
        {
            targetUserRatingsDTO = new();
            userRatingsToCompareDTO = new();

            for (int i = 0; i < targetUserDTO.Ratings.Count; i++)
            {
                for (int j = 0; j < userDTO.Ratings.Count; j++)
                {
                    if (targetUserDTO.Ratings[i].GameId == userDTO.Ratings[j].GameId)
                    {
                        targetUserRatingsDTO.Add(targetUserDTO.Ratings[i]);
                        userRatingsToCompareDTO.Add(userDTO.Ratings[j]);
                    }
                }
            }
        }

        public List<ComparedUserModel> GetNearestNeighbors(ApplicationUserDTO targetUserDTO, IEnumerable<ApplicationUserDTO> usersDTO, double minAverageOfUserRatings, int userCount)
        {
            List<ComparedUserModel> neighbors = new();

            if (GetAverageOfUserRatings(targetUserDTO) <= minAverageOfUserRatings)
            {
                return neighbors;
            }

            foreach (var user in usersDTO)
            {
                if (targetUserDTO.Id != user.Id && GetAverageOfUserRatings(user) >= minAverageOfUserRatings)
                {
                    neighbors.Add(new ComparedUserModel
                    {
                        ComparedUserId = user.Id,
                        SimilarityScore = CalculateCosineSimilarity(targetUserDTO, user)
                    });
                }
            }

            var similarUsers = neighbors.OrderByDescending(c => c.SimilarityScore).Take(userCount).Where(c => !double.IsNaN(c.SimilarityScore)).ToList();

            return similarUsers;
        }

        private static double GetAverageOfUserRatings(ApplicationUserDTO userDTO)
        {
            return userDTO.Ratings.Select(x => x.GameRating).DefaultIfEmpty().Average();
        }

        public async Task<List<GameDTO>> RetrieveNeighborsGamesForRecommendations(string currentUserId, List<ComparedUserModel> neighbors, double minAverageRating)
        {
            var ratings = _unitOfWork.RatingRepository.GetAllWithIncludes();

            var gamesRatedByTargetUser = ratings
                .Where(r => r.ApplicationUserId == currentUserId)
                .Select(r => r.Game);

            var gamesRatedByComparedUsers = ratings
                .Where(r => neighbors.Select(cu => cu.ComparedUserId)
                .Contains(r.ApplicationUserId) & r.GameRating >= minAverageRating)
                .Select(r => r.Game);

            var gamesToRecommend = await gamesRatedByComparedUsers.Except(gamesRatedByTargetUser).ToListAsync();

            return _mapper.Map<List<Game>, List<GameDTO>>(gamesToRecommend);
        }
    }
}
