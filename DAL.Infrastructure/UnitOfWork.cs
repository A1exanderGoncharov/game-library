using DAL.Entities;
using DAL.Infrastructure.Repositories;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        GameLibraryDbContext _context;

        public UnitOfWork(GameLibraryDbContext context,
                          UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager,
                          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        IGameRepository _gameRepository;
        public IGameRepository GameRepository =>
            _gameRepository ??= new GameRepository(_context);

        IUserGameLibraryRepository _userGameLibraryRepository;
        public IUserGameLibraryRepository UserGameLibraryRepository =>
            _userGameLibraryRepository ??= new UserGameLibraryRepository(_context);

        ICommentRepository _commentRepository;
        public ICommentRepository CommentRepository =>
            _commentRepository ??= new CommentRepository(_context);

        //IUserRepository _userRepository;
        //public IUserRepository UserRepository =>
        //    _userRepository ??= new UserRepository(_context);

        //IRepository<Comment> _commentRepository;
        //public IRepository<Comment> CommentRepository =>
        //    _commentRepository ??= new Repository<Comment>(_context);

        public SignInManager<ApplicationUser> SignInManager { get; }

        public UserManager<ApplicationUser> UserManager { get; }

        public RoleManager<IdentityRole> RoleManager { get; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
