using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> GetAllAsync(Expression<Func<ApplicationUser, bool>> filter = null);
    }
}
