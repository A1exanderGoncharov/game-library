using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserCollectionRepository : IRepository<UserCollection>
    {
        IQueryable<UserCollection> GetAllAsync(Expression<Func<UserCollection, bool>> filter = null);
    }
}
