using DAL.Entities;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        public IQueryable<Rating> GetAllWithIncludes();
    }
}
