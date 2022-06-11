using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infrastructure.Repositories
{
    public class UserCollectionRepository : Repository<UserCollection>, IUserCollectionRepository
    {
        GameLibraryDbContext _context;
        DbSet<UserCollection> _dbSet;

        public UserCollectionRepository(GameLibraryDbContext context)
            : base(context)
        {
            _context = context;
            _dbSet = context.Set<UserCollection>();
        }

        public IQueryable<UserCollection> GetAllAsync(Expression<Func<UserCollection, bool>> filter)
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
