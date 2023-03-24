using DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IUserGameLibraryRepository : IRepository<UserGame>
    {
        IQueryable<UserGame> GetAllWithIncludes(Expression<Func<UserGame, bool>> filter = null);
    }
}
