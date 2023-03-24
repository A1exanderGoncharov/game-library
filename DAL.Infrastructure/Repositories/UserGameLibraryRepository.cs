using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Infrastructure.Repositories
{
    public class UserGameLibraryRepository : Repository<UserGame>, IUserGameLibraryRepository
    {
		readonly DbSet<UserGame> _dbSet;

        public UserGameLibraryRepository(GameLibraryDbContext context)
            : base(context)
        {
            _dbSet = context.Set<UserGame>();
        }
        public IQueryable<UserGame> GetAllWithIncludes(Expression<Func<UserGame, bool>> filter)
        {
            IQueryable<UserGame> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(g => g.Game)
                .Include(au => au.ApplicationUser);

        }
    }
}
