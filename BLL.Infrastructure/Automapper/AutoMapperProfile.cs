using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Infrastructure.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<UserGame, UserGameDTO>().ReverseMap();
            CreateMap<UserRegisterModel, ApplicationUser>().ReverseMap()
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();

            CreateMap<ApplicationUser, UserRegisterModel>().ReverseMap()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GameGenre, GameGenreDTO>().ReverseMap();
            CreateMap<Collection, CollectionDTO>().ReverseMap();
            CreateMap<UserCollection, UserCollectionDTO>().ReverseMap();
            CreateMap<Rating, RatingDTO>().ReverseMap();
        }
    }
}
