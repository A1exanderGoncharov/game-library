using DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IUserGameLibraryRepository : IRepository<UserGame>
    {
        IQueryable<UserGame> GetAllAsync(Expression<Func<UserGame, bool>> filter = null);
    }
}
