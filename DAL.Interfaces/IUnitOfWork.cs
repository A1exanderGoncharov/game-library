using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //IUserRepository UserRepository { get; }

        Task SaveChangesAsync();
    }
}
