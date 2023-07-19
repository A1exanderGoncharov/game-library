namespace BLL.DTO
{
    public class RecommendationOptions
    {
        public int UsersCount { get; set; } = 5;
        public double MinAverageOfUserRatings { get; set; } = 3;
        public double MinAverageRating { get; set; } = 3.9;
        public bool EnableRandomOrder { get; set; } = true;
    }
}
