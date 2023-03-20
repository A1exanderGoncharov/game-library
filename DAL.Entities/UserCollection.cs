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
