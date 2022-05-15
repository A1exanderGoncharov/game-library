using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserGameLibraryRepository : IRepository<UserGameLibrary>
    {
        IQueryable<UserGameLibrary> GetAllAsync(Expression<Func<UserGameLibrary, bool>> filter = null);
    }
}
