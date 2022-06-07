using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public ICollection<Game> Games { get; set; } = new List<Game>();
        public List<GameGenre> GameGenres { get; set; }
    }
}
