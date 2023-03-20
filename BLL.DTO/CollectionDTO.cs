using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class CollectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public List<UserCollectionDTO> UserCollections { get; set; }
    }
}
