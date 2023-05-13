namespace BLL.DTO
{
    public class RecommendedGameDTO : GameDTO
    {
        public RecommendationType RecommendationType { get; set; }
    }

    public enum RecommendationType
    {
        TopRated,
        ForYou
    }
}
