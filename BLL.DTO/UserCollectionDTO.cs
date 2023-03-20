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
