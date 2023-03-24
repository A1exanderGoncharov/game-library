using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public class GameService : IGameService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(GameDTO game)
        {
            var gameEntity = _mapper.Map<GameDTO, Game>(game);

            await _unitOfWork.GameRepository.InsertAsync(gameEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);

            _unitOfWork.GameRepository.Delete(game);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(GameDTO game)
        {
            var gameEntity = _mapper.Map<GameDTO, Game>(game);

            _unitOfWork.GameRepository.Delete(gameEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAllWithIncludesAsync();

            return _mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(games);
        }

        public async Task<GameDTO> GetByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdWithIncludesAsync(id);

            return _mapper.Map<Game, GameDTO>(game);
        }      

        public async Task UpdateAsync(GameDTO game)
        {
            var gameEntity = _mapper.Map<GameDTO, Game>(game);
            _unitOfWork.GameRepository.Update(gameEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameDTO>> SearchAsync(string searchString)
        {
            var games = await GetAllAsync();

            return games.Where(g => g.Name.ToUpper().Contains(searchString.ToUpper()));
        }

        public async Task<IEnumerable<GameDTO>> FilterByGenreAsync(int gameGenre)
        {
            var gameGenresEntities = await _unitOfWork.GameGenresRepository.GetAllAsync();
            var gameGenresDTO = _mapper.Map<IEnumerable<GameGenre>, IEnumerable<GameGenreDTO>>(gameGenresEntities);
            ICollection<GameDTO> games = new List<GameDTO>();

            var result = gameGenresDTO.Where(x => x.GenreId == gameGenre);

            foreach (var item in result)
            {
                var game = GetByIdAsync(item.GameId).Result;
                games.Add(game);
            }
            return games;
        }

        public async Task AddGameWithGenreAsync(GameDTO game, string[] selectedGenres)
        {
            var gameEntity = _mapper.Map<GameDTO, Game>(game);

            await _unitOfWork.GameRepository.InsertAsync(gameEntity);
            await _unitOfWork.SaveChangesAsync();

            for (int i = 0; i < selectedGenres.Length; i++)
            {
                int GenreId = int.Parse(selectedGenres[i]);
                await AddGenreToGameAsync(gameEntity.Id, GenreId);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGenreToGameAsync(int GameId, int GenreId)
        {
            GameGenreDTO gameGenre = new()
            {
                GameId = GameId,
                GenreId = GenreId
            };

            var gameGenreEntity = _mapper.Map<GameGenreDTO, GameGenre>(gameGenre);
            await _unitOfWork.GameGenresRepository.InsertAsync(gameGenreEntity);
        }

        public async Task<IEnumerable<UserCollectionDTO>> GetGamesByCollectionIdAsync(int CollectionId)
        {
            var userCollectionsEntities = await _unitOfWork.UserCollectionRepository.GetAllWithIncludes().ToListAsync();
            var userCollectionGames = userCollectionsEntities.Where(g => g.CollectionId == CollectionId).ToList();
            
            return _mapper.Map<IEnumerable<UserCollection>, IEnumerable<UserCollectionDTO>>(userCollectionGames);
        }

        public async Task AddRatingToGameAsync(string UserId, int GameId, int Rating)
        {
			RatingDTO rating = new()
			{
				ApplicationUserId = UserId,
				GameId = GameId,
				GameRating = Rating
			};

			var RatingEntity = _mapper.Map<RatingDTO, Rating>(rating);

            await _unitOfWork.RatingRepository.InsertAsync(RatingEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GetGameRatingsCountAsync(int gameId)
        {
            var ratings = await _unitOfWork.RatingRepository.GetAllAsync();
            var ratingsCount = ratings.Where(r => r.GameId == gameId).Count();

			return ratingsCount;
        }

        public async Task<int> CalculateGameRatingScoreAsync(int gameId)
        {
            var ratings = await _unitOfWork.RatingRepository.GetAllAsync();
            var gameRatings = ratings.Where(r => r.GameId == gameId);

			int ratingSum = 0;
            int ratingScore = 0;

            foreach (var rating in gameRatings)
            {
                ratingSum += rating.GameRating;
            }

            if (gameRatings.Any())
            {
                ratingScore = ratingSum / gameRatings.Count();
            }

            return ratingScore;
        }

        //public IEnumerable<GameDTO> GetTopGames()
        //{
        //    var games = _unitOfWork.RatingRepository.GetAllAsync().Result;
        //    var gamesDTO = _mapper.Map<IEnumerable<Rating>, IEnumerable<RatingDTO>>(games);
        //    gamesDTO.GroupBy(x => x.GameId).Average(x => x.));

        //    return gamesDTO;
        //}
    }
}
