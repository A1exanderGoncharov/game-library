using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Collection : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<UserCollection> UserCollections { get; set; }
    }
}
