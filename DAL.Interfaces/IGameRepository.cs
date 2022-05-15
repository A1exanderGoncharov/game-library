using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        IQueryable<Game> GetAllAsync(Expression<Func<Game, bool>> filter = null);
    }
}
