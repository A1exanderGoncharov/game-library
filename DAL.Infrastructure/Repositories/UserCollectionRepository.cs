using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Infrastructure.Repositories
{
    public class UserCollectionRepository : Repository<UserCollection>, IUserCollectionRepository
    {
		readonly DbSet<UserCollection> _dbSet;

        public UserCollectionRepository(GameLibraryDbContext context)
            : base(context)
        {
            _dbSet = context.Set<UserCollection>();
        }

        public IQueryable<UserCollection> GetAllWithIncludes(Expression<Func<UserCollection, bool>> filter)
        {
            IQueryable<UserCollection> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(uc => uc.UserGame).ThenInclude(au => au.ApplicationUser)
                .Include(uc => uc.UserGame).ThenInclude(g => g.Game)
                .Include(uc => uc.Collection);

        }
    }
}
