using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Collection : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public List<UserCollection> UserCollections { get; set; }
    }
}
