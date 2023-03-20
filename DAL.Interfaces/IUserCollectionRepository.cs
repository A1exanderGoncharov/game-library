using DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IUserCollectionRepository : IRepository<UserCollection>
    {
        IQueryable<UserCollection> GetAllAsync(Expression<Func<UserCollection, bool>> filter = null);
    }
}
