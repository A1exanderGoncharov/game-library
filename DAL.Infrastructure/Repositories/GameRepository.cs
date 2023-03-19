using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Infrastructure.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly DbSet<Game> _dbSet;

        public GameRepository(GameLibraryDbContext context)
            : base(context)
        {
            _dbSet = context.Set<Game>();
        }

        public async Task<IEnumerable<Game>> GetAllWithIncludesAsync(Expression<Func<Game, bool>> filter)
        {
            IQueryable<Game> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return await entities
                .Include(g => g.Comments).ThenInclude(c => c.ApplicationUser).AsSplitQuery()
                .Include(g => g.Comments).ThenInclude(c => c.Replies).AsSplitQuery()
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre).AsSplitQuery()
                .Include(g => g.Ratings)
                .ToListAsync();
        }

        public async Task<Game> GetByIdWithIncludesAsync(int id)
        {
            return await _dbSet
                .Include(g => g.Comments).ThenInclude(c => c.ApplicationUser).AsSplitQuery()
                .Include(g => g.Comments).ThenInclude(c => c.Replies).AsSplitQuery()
                .Include(g => g.GameGenres).ThenInclude(gg => gg.Genre).AsSplitQuery()
                .Include(g => g.Ratings)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

    }
}
