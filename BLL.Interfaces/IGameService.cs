using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameDTO>> GetAllAsync();
        Task AddAsync(GameDTO gameDTO);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(GameDTO gameDTO);
        Task<GameDTO> GetByIdAsync(int id);
        Task UpdateAsync(GameDTO gameDTO);
        Task<IEnumerable<GameDTO>> SearchAsync(string searchString);
        Task<IEnumerable<GameDTO>> FilterByGenreAsync(int gameGenreId);
        Task AddGenreToGameAsync(int gameId, int genreId);
        Task AddGameWithGenreAsync(GameDTO gameDTO, string[] selectedGenres);
        Task<IEnumerable<UserCollectionDTO>> GetGamesByCollectionIdAsync(int collectionId);
        Task AddRatingToGameAsync(string userId, int gameId, int ratingScore);
        Task<double> CalculateGameRatingScoreAsync(int gameId);
        Task<int> GetGameRatingsCountAsync(int gameId);
        Task UpdateGameGenresAsync(int gameId, string[] selectedGenres);
        Task<bool> HasUserRatedGame(int gameId, string userId);
    }
}
