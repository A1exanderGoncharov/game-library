using DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> GetAllAsync(Expression<Func<ApplicationUser, bool>> filter = null);
    }
}
