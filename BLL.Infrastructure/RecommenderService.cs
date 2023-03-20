﻿using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Infrastructure
{
    public class RecommenderService : IRecommenderService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        IGameService _gameService;

        public RecommenderService(IUnitOfWork unitOfWork, IMapper mapper, IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gameService = gameService;
        }

        public double CalculateCosineSimilarity(ApplicationUserDTO targetUser, ApplicationUserDTO user)
        {
            double numeratorSum = 0;
            double firstDenumeratorSum = 0;
            double secondDenumeratorSum = 0;

            List<RatingDTO> targetUserRatings = new();
            List<RatingDTO> userRatingsToCompare = new();

            for (int i = 0; i < targetUser.Ratings.Count; i++)
            {
                for (int j = 0; j < user.Ratings.Count; j++)
                {
                    if (targetUser.Ratings[i].GameId == user.Ratings[j].GameId)
                    {
                        targetUserRatings.Add(targetUser.Ratings[i]);
                        userRatingsToCompare.Add(user.Ratings[j]);
                    }
                }
            }

            for (int i = 0; i < targetUserRatings.Count; i++)
            {
                numeratorSum += targetUserRatings[i].GameRating * userRatingsToCompare[i].GameRating;
                firstDenumeratorSum += Math.Pow(targetUserRatings[i].GameRating, 2);
                secondDenumeratorSum += Math.Pow(userRatingsToCompare[i].GameRating, 2);
            }

            targetUserRatings.Clear();
            userRatingsToCompare.Clear();

            return numeratorSum / (Math.Sqrt(firstDenumeratorSum) * Math.Sqrt(secondDenumeratorSum));
        }

        public List<ComparedUserModel> GetNearestNeighbors(string targetUserId)
        {
            var targetUser = _unitOfWork.UserRepository.GetAllAsync().FirstOrDefault(u => u.Id == targetUserId);
            var targetUserDTO = _mapper.Map<ApplicationUser, ApplicationUserDTO>(targetUser);

            var usersEntities = _unitOfWork.UserRepository.GetAllAsync().ToList();
            var usersDTO = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserDTO>>(usersEntities);

            //if (targetUserDTO == null)
            //{
            //    return new List<ComparedUserModel>();
            //}

            const int minAvgRating = 3;
            const int minNumberOfRatedGames = 2;

            List<ComparedUserModel> neighbors = new();

            if (targetUserDTO.Ratings.Count >= minNumberOfRatedGames && targetUserDTO.Ratings.Select(x => x.GameRating).DefaultIfEmpty().Average() >= minAvgRating)
            {
                foreach (var user in usersDTO)
                {
                    if (targetUserId != user.Id && user.Ratings.Select(x => x.GameRating).DefaultIfEmpty().Average() >= minAvgRating)
                    {
                        neighbors.Add(new ComparedUserModel
                        {
                            ComparedUserId = user.Id,
                            SimilarityScore = CalculateCosineSimilarity(targetUserDTO, user)
                        });
                    }
                }
            }

            var similarUsers = neighbors.OrderByDescending(c => c.SimilarityScore).Take(5).ToList();

            return similarUsers;
        }

        public List<GameDTO> GetPersonalizedRecommendations(string currentUserId)
        {
            var neighbors = GetNearestNeighbors(currentUserId);

            List<GameDTO> recommendedGames = new();

            const int minRecommendedGamesNumber = 6;

            if (neighbors.Count != 0)
            {
                var gameIdsOfTargetUser = _unitOfWork.RatingRepository.GetAllAsync().Result
                    .Where(x => x.ApplicationUserId == currentUserId)
                    .Select(x => x.GameId);

                var gameIdsOfComparedUser = _unitOfWork.RatingRepository.GetAllAsync().Result
                    .Where(r => neighbors.Select(cu => cu.ComparedUserId)
                    .Contains(r.ApplicationUserId) & r.GameRating > 3)
                    .Select(x => x.GameId);

                List<int> recommendedGameIds = gameIdsOfComparedUser.Except(gameIdsOfTargetUser).ToList();

                recommendedGames = GetRecommendedGamesByIds(recommendedGameIds);
            }

            if (recommendedGames.Count < minRecommendedGamesNumber)
            {
                SupplementRecommendationsByTopRatedGames(recommendedGames, minRecommendedGamesNumber);
            }

            return recommendedGames;
        }

        private List<GameDTO> GetRecommendedGamesByIds(List<int> recommendations)
        {
            List<GameDTO> games = new();

            foreach (var gameId in recommendations)
            {
                var game = _gameService.GetByIdAsync(gameId);
                games.Add(game.Result);
            }

            return games;
        }

        private List<GameDTO> SupplementRecommendationsByTopRatedGames(List<GameDTO> recommendations, int minRecommendedGamesNumber)
        {
            const int minRecommendedGameRating = 4;

            if (recommendations.Count < minRecommendedGamesNumber)
            {
                var topRatedGameIds = _gameService.GetAllAsync().Result
                    .Where(g => g.Ratings
                    .Select(r => r.GameRating).DefaultIfEmpty()
                    .Average() > minRecommendedGameRating)
                    .Select(g => g.Id)
                    .Except(recommendations.Select(g => g.Id))
                    .ToList();

                var gamesToSupplement = GetRecommendedGamesByIds(topRatedGameIds)
                    .OrderBy(g => Guid.NewGuid())
                    .Take(minRecommendedGamesNumber - recommendations.Count);

                recommendations.AddRange(gamesToSupplement);
            }

            return recommendations;
        }

    }
}
