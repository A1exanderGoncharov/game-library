using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserGameLibraryService
    {
        Task<IEnumerable<UserGameDTO>> GetAllAsync();
        Task<IEnumerable<UserGameDTO>> GetAllByUserIdAsync(string id);
        Task AddAsync(UserGameDTO userGameDTO);
        Task AddGameToUserLibraryAsync(int gameId, string userId);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(UserGameDTO userGameDTO);
        Task<UserGameDTO> GetByIdAsync(int id);
        Task UpdateAsync(UserGameDTO userGameDTO);
        Task IsGamePassedAsync(int id, bool isPassed);
    }
}
