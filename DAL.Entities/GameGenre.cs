namespace DAL.Entities
{
    public class GameGenre : BaseEntity
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        //public bool IsChanged { get; set; }
    }
}
