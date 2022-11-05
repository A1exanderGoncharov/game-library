using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Rating : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int GameRating{ get; set; }
    }
}
