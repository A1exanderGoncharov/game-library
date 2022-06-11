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
    public class UserGameLibraryRepository : Repository<UserGame>, IUserGameLibraryRepository
    {
        GameLibraryDbContext _context;
        DbSet<UserGame> _dbSet;

        public UserGameLibraryRepository(GameLibraryDbContext context)
            : base(context)
        {
            _context = context;
            _dbSet = context.Set<UserGame>();
        }
        public IQueryable<UserGame> GetAllAsync(Expression<Func<UserGame, bool>> filter)
        {
            IQueryable<UserGame> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(g => g.Game)
                .Include(au => au.ApplicationUser);

        }
    }
}
