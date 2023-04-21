using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class GameService : IGameService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(GameDTO gameDTO)
        {
            var game = _mapper.Map<GameDTO, Game>(gameDTO);

            await _unitOfWork.GameRepository.InsertAsync(game);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);

            _unitOfWork.GameRepository.Delete(game);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(GameDTO gameDTO)
        {
            var game = _mapper.Map<GameDTO, Game>(gameDTO);

            _unitOfWork.GameRepository.Delete(game);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAllWithIncludesAsync();

            return _mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(games);
        }

        public async Task<GameDTO> GetByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdWithIncludesAsync(id) 
                ?? throw new ElementNotFoundException(nameof(Game), id); 

            return _mapper.Map<Game, GameDTO>(game);
        }

        public async Task UpdateAsync(GameDTO gameDTO)
        {
            var game = _mapper.Map<GameDTO, Game>(gameDTO);

            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameDTO>> SearchAsync(string searchString)
        {
            var games = await GetAllAsync();

            return games.Where(g => g.Name.ToUpper().Contains(searchString.ToUpper()));
        }

        public async Task<IEnumerable<GameDTO>> FilterByGenreAsync(int gameGenreId)
        {
            var gameGenres = await _unitOfWork.GameGenresRepository.GetAllAsync();
            var gameGenresDTO = _mapper.Map<IEnumerable<GameGenre>, IEnumerable<GameGenreDTO>>(gameGenres);

            ICollection<GameDTO> gamesDTO = new List<GameDTO>();

            var result = gameGenresDTO.Where(x => x.GenreId == gameGenreId);

            foreach (var gameGenreDTO in result)
            {
                var game = await GetByIdAsync(gameGenreDTO.GameId);
                gamesDTO.Add(game);
            }
            return gamesDTO;
        }

        public async Task AddGameWithGenreAsync(GameDTO gameDTO, string[] selectedGenres)
        {
            var game = _mapper.Map<GameDTO, Game>(gameDTO);

            await _unitOfWork.GameRepository.InsertAsync(game);
            await _unitOfWork.SaveChangesAsync();

            for (int i = 0; i < selectedGenres.Length; i++)
            {
                int genreId = int.Parse(selectedGenres[i]);
                await AddGenreToGameAsync(game.Id, genreId);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGenreToGameAsync(int gameId, int genreId)
        {
            GameGenreDTO gameGenreDTO = new()
            {
                GameId = gameId,
                GenreId = genreId
            };

            var gameGenre = _mapper.Map<GameGenreDTO, GameGenre>(gameGenreDTO);
            await _unitOfWork.GameGenresRepository.InsertAsync(gameGenre);
        }

        public async Task<IEnumerable<UserCollectionDTO>> GetGamesByCollectionIdAsync(int collectionId)
        {
            var userCollections = await _unitOfWork.UserCollectionRepository.GetAllWithIncludes().ToListAsync();

            var userCollectionGames = userCollections.Where(g => g.CollectionId == collectionId).ToList();

            return _mapper.Map<IEnumerable<UserCollection>, IEnumerable<UserCollectionDTO>>(userCollectionGames);
        }

        public async Task AddRatingToGameAsync(string userId, int gameId, int ratingScore)
        {
            if (ratingScore < 1 || ratingScore > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(ratingScore));
            }

            bool isRatingExist = await HasUserRatedGame(gameId, userId);

            if (!isRatingExist)
            {
                RatingDTO ratingDTO = new()
                {
                    ApplicationUserId = userId,
                    GameId = gameId,
                    GameRating = ratingScore
                };

                await _unitOfWork.RatingRepository.InsertAsync(_mapper.Map<RatingDTO, Rating>(ratingDTO));
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<int> GetGameRatingsCountAsync(int gameId)
        {
            var ratings = await _unitOfWork.RatingRepository.GetAllAsync();
            var ratingsCount = ratings.Where(r => r.GameId == gameId).Count();

            return ratingsCount;
        }

        public async Task<double> CalculateGameRatingScoreAsync(int gameId)
        {
            var ratings = await _unitOfWork.RatingRepository.GetAllAsync();
            var gameRatings = ratings.Where(r => r.GameId == gameId);

            if (!gameRatings.Any())
            {
                return 0;
            }

            return gameRatings.Average(r => r.GameRating);
        }

        public async Task UpdateGameGenresAsync(int gameId, string[] selectedGenres)
        {
            var game = await _unitOfWork.GameRepository.GetByIdWithIncludesAsync(gameId)
                ?? throw new ElementNotFoundException(nameof(Game), gameId);

            foreach (var gameGenre in game.GameGenres)
            {
                _unitOfWork.GameGenresRepository.Delete(gameGenre);
            }

            foreach (string selectedGenre in selectedGenres)
            {
                int genreId = int.Parse(selectedGenre);

                await AddGenreToGameAsync(game.Id, genreId);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> HasUserRatedGame(int gameId, string userId)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);

            if (game.Ratings == null)
            {
                return false;
            }

            var hasUserRated = game.Ratings
                .Where(r => r.ApplicationUserId == userId && r.GameId == gameId)
                .Any();

            return hasUserRated;
        }

    }
}
