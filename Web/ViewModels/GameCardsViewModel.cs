using BLL.DTO;
using System.Collections.Generic;
using Web.Helpers;

namespace Web.ViewModels
{
    public class GameCardsViewModel
    {
        public IEnumerable<GameDTO> Games = new List<GameDTO>();
        public IEnumerable<GenreDTO> Genres = new List<GenreDTO>();
        public PaginatedList<GameDTO> PaginatedGames { get; set; }
        public List<List<RecommendedGameDTO>> recommendedGames = new();
        public string ActionName { get; set; }
        public int? GameGenreId { get; set; }
        public string SearchString { get; set; }
    }
}
