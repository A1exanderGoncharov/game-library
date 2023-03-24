using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Infrastructure.Repositories
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
		readonly DbSet<Collection> _dbSet;

        public CollectionRepository(GameLibraryDbContext context)
            : base(context)
        {
            _dbSet = context.Set<Collection>();
        }

        public IQueryable<Collection> GetAllWithIncludes()
        {
            return _dbSet
                .Include(c => c.UserCollections);
        }
    }
}
