using BLL.DTO;
using System.Collections.Generic;
using Web.Helpers;

namespace Web.ViewModels
{
	public class SearchingViewModel
	{
		public IEnumerable<GameDTO> Games { get; set; } = new List<GameDTO>();
		public IEnumerable<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
		public PaginatedList<GameDTO> PaginatedGames { get; set; }
	}
}
