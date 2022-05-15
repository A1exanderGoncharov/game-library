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
    public class GameRepository : Repository<Game>, IGameRepository
    {
        GameLibraryDbContext _context;
        DbSet<Game> _dbSet;

        public GameRepository(GameLibraryDbContext context)
            : base(context)
        {
            _context = context;
            _dbSet = context.Set<Game>();
        }
        public IQueryable<Game> GetAllAsync(Expression<Func<Game, bool>> filter)
        {
            IQueryable<Game> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(c => c.Comments).ThenInclude(u => u.ApplicationUser).AsSplitQuery()
                .Include(c => c.Comments).ThenInclude(r => r.Replies).AsSplitQuery();

        }

    }
}
