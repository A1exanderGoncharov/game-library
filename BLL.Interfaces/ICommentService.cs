using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task AddAsync(CommentDTO comment);
        Task DeleteByIdAsync(int id);
        Task RemoveAsync(CommentDTO comment);
        Task<CommentDTO> GetByIdAsync(int id);
        Task UpdateAsync(CommentDTO comment);
    }
}
