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
        DbSet<Collection> _dbSet;

        public CollectionRepository(GameLibraryDbContext context)
            : base(context)
        {
            _dbSet = context.Set<Collection>();
        }
        public new IQueryable<Collection> GetAllAsync()
        {
            return _dbSet
                .Include(c => c.UserCollections);
        }
    }
}
