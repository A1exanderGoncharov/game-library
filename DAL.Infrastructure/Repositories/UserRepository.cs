using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        GameLibraryDbContext _context;
		readonly DbSet<ApplicationUser> _dbSet;

        public UserRepository(GameLibraryDbContext context)
        {
            _context = context;
            _dbSet = context.Set<ApplicationUser>();
        }
        public IQueryable<ApplicationUser> GetAllAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            IQueryable<ApplicationUser> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(p => p.Ratings);

        }
    }
}
