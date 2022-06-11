using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class UserCollectionDTO
    {
        public int UserGameId { get; set; }
        public UserGameDTO UserGame { get; set; }
        public int CollectionId { get; set; }
        public CollectionDTO Collection { get; set; }
    }
}
