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
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        GameLibraryDbContext _context;
        DbSet<Collection> _dbSet;

        public CollectionRepository(GameLibraryDbContext context)
            : base(context)
        {
            _context = context;
            _dbSet = context.Set<Collection>();
        }
        public IQueryable<Collection> GetAllAsync(Expression<Func<Collection, bool>> filter)
        {
            IQueryable<Collection> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                .Include(c => c.UserCollections);
        }
    }
}
