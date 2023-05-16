using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        SignInManager<ApplicationUser> SignInManager { get; }
        UserManager<ApplicationUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        IGameRepository GameRepository { get; }
        IUserGameLibraryRepository UserGameLibraryRepository { get; }
        ICommentRepository CommentRepository { get; }
        IRepository<Genre> GenreRepository { get; }
        IRepository<GameGenre> GameGenresRepository { get; }
        ICollectionRepository CollectionRepository { get; }
        IUserCollectionRepository UserCollectionRepository { get; }
        IRatingRepository RatingRepository { get; }
        IUserRepository UserRepository { get; }

        Task SaveChangesAsync();
    }
}
