using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        new IQueryable<Collection> GetAllAsync();
    }
}
