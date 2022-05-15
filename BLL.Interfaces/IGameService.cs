using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Task<IEnumerable<GameDTO>> Search(string searchString);
        Task<IEnumerable<GameDTO>> FilterByGenre(string gameGenre);
    }
}
