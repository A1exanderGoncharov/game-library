using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserGame : BaseEntity
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsPassed { get; set; }
        public List<UserCollection> UserCollections { get; set; }
    }
}
