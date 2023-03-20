using DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IQueryable<Comment> DbsetWithProperties(Expression<Func<Comment, bool>> filter = null);
    }
}
