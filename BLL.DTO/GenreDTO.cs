using System.Collections.Generic;

namespace BLL.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GameGenreDTO> GameGenres { get; set; }
    }
}
