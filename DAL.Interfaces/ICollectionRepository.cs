using DAL.Entities;
using System.Linq;

namespace DAL.Interfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        IQueryable<Collection> GetAllWithIncludes();
    }
}
