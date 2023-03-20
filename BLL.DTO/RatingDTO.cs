namespace BLL.DTO
{
    public class RatingDTO
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public int GameId { get; set; }
        public GameDTO Game { get; set; }
        public int GameRating { get; set; }
    }
}
