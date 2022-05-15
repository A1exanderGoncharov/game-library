using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IQueryable<Comment> DbsetWithProperties(Expression<Func<Comment, bool>> filter = null);
    }
}
