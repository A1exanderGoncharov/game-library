using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameDTO>> GetAllAsync();
        Task AddAsync(GameDTO game);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(GameDTO game);
        Task<GameDTO> GetByIdAsync(int id);
        Task UpdateAsync(GameDTO game);
        Task<IEnumerable<GameDTO>> SearchAsync(string searchString);
        Task<IEnumerable<GameDTO>> FilterByGenreAsync(int gameGenre);
        Task AddGenreToGameAsync(int GameId, int GenreId);
        Task AddGameWithGenreAsync(GameDTO game, string[] selectedGenres);
        Task<IEnumerable<UserCollectionDTO>> GetGamesByCollectionIdAsync(int CollectionId);
        Task AddRatingToGameAsync(string UserId, int GameId, int Rating);
        Task<int> CalculateGameRatingScoreAsync(int gameId);
        Task<int> GetGameRatingsCountAsync(int gameId);
        //public IEnumerable<GameDTO> GetTopGames();
    }
}
