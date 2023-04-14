using BLL.DTO;

namespace Web.ViewModels
{
    public class GameToEditViewModel
    {
        public GameDTO GameDto {  get; set; }
        public int? PageNumber { get; set; }
        public string ActionName { get; set; }
        public int? GameGenreId { get; set; }
        public string SearchString { get; set; }
    }
}
