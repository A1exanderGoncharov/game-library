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
    public class UserGameLibraryRepository : Repository<UserGameLibrary>, IUserGameLibraryRepository
    {
        GameLibraryDbContext _context;
        DbSet<UserGameLibrary> _dbSet;

        public UserGameLibraryRepository(GameLibraryDbContext context)
            : base(context)
        {
            _context = context;
            _dbSet = context.Set<UserGameLibrary>();
        }
        public IQueryable<UserGameLibrary> GetAllAsync(Expression<Func<UserGameLibrary, bool>> filter)
        {
            IQueryable<UserGameLibrary> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(g => g.Game)
                .Include(au => au.ApplicationUser);

        }
    }
}
