using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Infrastructure.Repositories
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        readonly DbSet<Rating> _dbSet;

        public RatingRepository(GameLibraryDbContext context)
            : base(context)
        {
            _dbSet = context.Set<Rating>();
        }

        public IQueryable<Rating> GetAllWithIncludes()
        {
            return _dbSet
                .Include(r => r.Game);
        }
    }
}
