using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserGameLibraryService
    {
        Task<IEnumerable<UserGameDTO>> GetAllAsync();
        Task<IEnumerable<UserGameDTO>> GetAllByUserIdAsync(string id);
        Task AddAsync(UserGameDTO userGameLibrary);
        Task AddGameToUserLibrary(int gameId, string userId);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(UserGameDTO userGameLibrary);
        Task<UserGameDTO> GetByIdAsync(int id);
        Task UpdateAsync(UserGameDTO userGameLibrary);
        Task IsPassed(int id, bool isPassed);
    }
}
