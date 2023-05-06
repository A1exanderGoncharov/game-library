using DAL.Entities;
using DAL.Infrastructure.Repositories;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.Infrastructure.Services
{
    public static class DALServices
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, string connectionString)
        {
            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 1;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "";
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<GameLibraryDbContext>()
            .AddSignInManager();

            services.AddDbContext<GameLibraryDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUserGameLibraryRepository, UserGameLibraryRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IUserCollectionRepository, UserCollectionRepository>();

            return services;
        }
    }
}
