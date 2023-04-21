using BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task AddAsync(CommentDTO commentDTO);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(CommentDTO commentDTO);
        Task<CommentDTO> GetByIdAsync(int id);
        Task UpdateAsync(CommentDTO commentDTO);
    }
}
