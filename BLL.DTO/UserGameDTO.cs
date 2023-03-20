using System.Collections.Generic;

namespace BLL.DTO
{
    public class UserGameDTO
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public GameDTO Game { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public bool IsPassed { get; set; }
        public List<UserCollectionDTO> UserCollections { get; set; }
    }
}
