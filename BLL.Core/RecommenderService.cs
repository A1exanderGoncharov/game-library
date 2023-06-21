using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
    public class RecommenderService : IRecommenderService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IGameService _gameService;
        readonly IUserService _userService;

        public RecommenderService(IUnitOfWork unitOfWork, IMapper mapper, IGameService gameService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gameService = gameService;
            _userService = userService;
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

        private static List<ComparedUserModel> GetNearestNeighbors(ApplicationUserDTO targetUserDTO, IEnumerable<ApplicationUserDTO> usersDTO, double minAverageOfUserRatings, int userCount)
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
        
        public async Task<List<RecommendedGameDTO>> GetPersonalizedRecommendationsAsync(string currentUserId, int count, double minAverageOfUserRatings, int userCount)
        {
            ApplicationUserDTO targetUserDTO = _userService.GetUserById(currentUserId);
            IEnumerable<ApplicationUserDTO> usersDTO = _userService.GetAllUsers();

            List<ComparedUserModel> neighbors = GetNearestNeighbors(targetUserDTO, usersDTO, minAverageOfUserRatings, userCount);

            if (neighbors.Count == 0)
            {
                await SupplementRecsByTopRatedGamesAsync(new List<RecommendedGameDTO>(), count, currentUserId);
            }

            List<GameDTO> recommendedGamesDTO = await RetrieveNeighborsGamesForRecommendations(currentUserId, neighbors, 3);

            List<RecommendedGameDTO> recommendationsDTO = SpecifyRecommendationType(recommendedGamesDTO, RecommendationType.ForYou);

            if (recommendedGamesDTO.Count < count)
            {
                await SupplementRecsByTopRatedGamesAsync(recommendationsDTO, count, currentUserId);
            }

            return recommendationsDTO;
        }

        private async Task<List<GameDTO>> RetrieveNeighborsGamesForRecommendations(string currentUserId, List<ComparedUserModel> neighbors, double minAverageRating)
        {
            var ratings = _unitOfWork.RatingRepository.GetAllWithIncludes();

            var gamesRatedByTargetUser = ratings
                .Where(r => r.ApplicationUserId == currentUserId)
                .Select(r => r.Game);

            var gamesRatedByComparedUsers = ratings
                .Where(r => neighbors.Select(cu => cu.ComparedUserId)
                .Contains(r.ApplicationUserId) & r.GameRating > minAverageRating)
                .Select(r => r.Game);

            var gamesToRecommend = await gamesRatedByComparedUsers.Except(gamesRatedByTargetUser).ToListAsync();

            return _mapper.Map<List<Game>, List<GameDTO>>(gamesToRecommend);
        }

        private async Task<List<GameDTO>> GetGamesByIdsAsync(IEnumerable<int> gameIds)
        {
            List<GameDTO> gamesDTO = new();

            foreach (var gameId in gameIds)
            {
                var game = await _gameService.GetByIdAsync(gameId);
                gamesDTO.Add(game);
            }

            return gamesDTO;
        }

        private async Task<List<RecommendedGameDTO>> SupplementRecsByTopRatedGamesAsync(List<RecommendedGameDTO> recommendationsDTO, int minRecommendedGamesCount, string userId)
        {
            const double minGameRatingScore = 4;
            var gamesDTO = await _gameService.GetAllAsync();

            IEnumerable<GameDTO> gamesNotRatedByUser = GetGamesNotRatedByUser(userId, gamesDTO);

            IEnumerable<int> topRatedGamesIdsExclRecs = GetTopRatedGamesIds(minGameRatingScore, gamesNotRatedByUser)
                .Except(recommendationsDTO.Select(g => g.Id));

            List<GameDTO> gamesToSupplement = (await GetGamesByIdsAsync(topRatedGamesIdsExclRecs))
                .OrderBy(g => Guid.NewGuid())
                .Take(minRecommendedGamesCount - recommendationsDTO.Count)
                .ToList();

            List<RecommendedGameDTO> topRatedGamesRecs = SpecifyRecommendationType(gamesToSupplement, RecommendationType.TopRated);

            recommendationsDTO.AddRange(topRatedGamesRecs);

            return recommendationsDTO;
        }

        private static IEnumerable<GameDTO> GetGamesNotRatedByUser(string userId, IEnumerable<GameDTO> gamesDTO)
        {
            var gamesWithoutUserRating = gamesDTO
                .Where(g => !g.Ratings.Any(r => r.ApplicationUserId == userId))
                .Select(g => g)
                .Distinct();

            return gamesWithoutUserRating;
        }

        private static IEnumerable<int> GetTopRatedGamesIds(double minRecommendedGameRating, IEnumerable<GameDTO> gamesDTO)
        {
            var topRatedGameIds = gamesDTO
                .Where(g => g.Ratings
                .Select(r => r.GameRating).DefaultIfEmpty()
                .Average() > minRecommendedGameRating)
                .Select(g => g.Id);

            return topRatedGameIds;
        }

        private List<RecommendedGameDTO> SpecifyRecommendationType(List<GameDTO> gamesDTO, RecommendationType recommendationType)
        {
            var recommendedGamesDTO = _mapper.Map<List<GameDTO>, List<RecommendedGameDTO>>(gamesDTO);

            recommendedGamesDTO.ForEach(g => g.RecommendationType = recommendationType);

            return recommendedGamesDTO;
        }

    }
}
