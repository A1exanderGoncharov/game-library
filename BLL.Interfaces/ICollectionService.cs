using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICollectionService
    {
        Task<IEnumerable<CollectionDTO>> GetAllAsync();
        Task AddAsync(CollectionDTO collection);
        Task DeleteByIdAsync(int id);
        Task<CollectionDTO> GetByIdAsync(int id);
        Task UpdateAsync(CollectionDTO collection);
        Task AddGamesToCollection(int CollectionId, List<string> SelectedGames);
        Task<IEnumerable<CollectionDTO>> GetAllByUserId(string UserId);
    }
}
