using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDTO>> GetAllAsync();
        Task<IEnumerable<GenreDTO>> GetAllGenresOrderedByAsync();
        Task AddAsync(GenreDTO genre);
        Task DeleteByIdAsync(int id);
        Task<GenreDTO> GetByIdAsync(int id);
        Task UpdateAsync(GenreDTO genre);
    }
}
