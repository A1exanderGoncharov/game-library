namespace BLL.DTO
{
    public class GameGenreDTO
    {
        public int GameId { get; set; }
        public GameDTO Game { get; set; }
        public int GenreId { get; set; }
        public GenreDTO Genre { get; set; }
    }
}
