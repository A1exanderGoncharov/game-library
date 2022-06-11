using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserCollection : BaseEntity
    {
        public int UserGameId { get; set; }
        public UserGame UserGame { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }
    }
}
