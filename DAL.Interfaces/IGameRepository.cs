using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<IEnumerable<Game>> GetAllWithIncludesAsync(Expression<Func<Game, bool>> filter = null);
        Task<Game> GetByIdWithIncludesAsync(int id);
    }
}
