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
        Task<IEnumerable<UserGameLibraryDTO>> GetAllAsync();
        Task<IEnumerable<UserGameLibraryDTO>> GetAllByUserIdAsync(string id);
        Task AddAsync(UserGameLibraryDTO userGameLibrary);
        Task AddGameToUserLibrary(int gameId, string userId);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(UserGameLibraryDTO userGameLibrary);
        Task<UserGameLibraryDTO> GetByIdAsync(int id);
        Task UpdateAsync(UserGameLibraryDTO userGameLibrary);
        Task IsPassed(int id, bool isPassed);
    }
}
