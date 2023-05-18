using AutoMapper;
using BLL.Infrastructure.Automapper;
using BLL.Infrastructure.RecommendationSystem;
using BLL.Interfaces;
using BLL.Interfaces.RecommendationSystem;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Infrastructure.Services
{
    public static class BLLServices
    {
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(c => c.AddProfile(new AutoMapperProfile()));
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IUserGameLibraryService, UserGameLibraryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<ICollectionService, CollectionService>();
            services.AddTransient<IRecommenderService, RecommenderService>();
            services.AddTransient<IRecommendationService, RecommendationService>();
            services.AddTransient<IUserBasedRecommendationService, UserBasedRecommendationService>();
            services.AddTransient<ITopGameRecommendationService, TopGameRecommendationService>();

            return services;
        }
    }
}
