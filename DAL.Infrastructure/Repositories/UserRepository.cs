//using DAL.Entities;
//using DAL.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL.Infrastructure.Repositories
//{
//    public class UserRepository : Repository<UserProfile>, IUserRepository
//    {
//        GameLibraryDbContext _context;
//        DbSet<UserProfile> _dbSet;

//        public UserRepository(GameLibraryDbContext context)
//            : base(context)
//        {
//            _context = context;
//            _dbSet = context.Set<UserProfile>();
//        }
//        public IQueryable<UserProfile> GetAllAsync(Expression<Func<UserProfile, bool>> filter)
//        {
//            IQueryable<UserProfile> entities = _dbSet;

//            if (filter != null) entities = entities.Where(filter);

//            return entities
//                .Include(p => p.Games)
//                .Include(p => p.UserGameLibraries)
//                .Include(p => p.ApplicationUser).AsNoTracking();

//        }
//    }
//}
