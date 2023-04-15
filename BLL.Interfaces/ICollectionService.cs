using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICollectionService
    {
        Task<IEnumerable<CollectionDTO>> GetAllAsync();
        Task AddAsync(CollectionDTO collectionDTO);
        Task DeleteByIdAsync(int id);
        Task<CollectionDTO> GetByIdAsync(int id);
        Task UpdateAsync(CollectionDTO collectionDTO);
        Task AddGamesToCollectionAsync(int collectionId, int[] selectedGames);
        Task<IEnumerable<CollectionDTO>> GetAllByUserIdAsync(string userId);
        Task RemoveGameFromCollectionAsync(UserCollectionDTO userCollectionDTO);
    }
}
