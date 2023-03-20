using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        GameLibraryDbContext _context;
        DbSet<Comment> _dbSet;

        public CommentRepository(GameLibraryDbContext context)
            : base(context)
        {
            _context = context;
            _dbSet = context.Set<Comment>();
        }

        public IQueryable<Comment> DbsetWithProperties(Expression<Func<Comment, bool>> filter)
        {
            IQueryable<Comment> entities = _dbSet;

            if (filter != null) entities = entities.Where(filter);

            return entities
                //.Include(c => c.ReplyToComment)
                .Include(c => c.Replies);

        }

    }
}
