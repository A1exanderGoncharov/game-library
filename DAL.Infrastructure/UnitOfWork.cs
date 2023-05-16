using DAL.Entities;
using DAL.Infrastructure.Repositories;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly GameLibraryDbContext _context;

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

        IRepository<Genre> _genreRepository;
        public IRepository<Genre> GenreRepository =>
            _genreRepository ??= new Repository<Genre>(_context);

        IRepository<GameGenre> _gameGenresRepository;
        public IRepository<GameGenre> GameGenresRepository =>
            _gameGenresRepository ??= new Repository<GameGenre>(_context);

        ICollectionRepository _collectionRepository;
        public ICollectionRepository CollectionRepository =>
            _collectionRepository ??= new CollectionRepository(_context);

        IUserCollectionRepository _userCollectionRepository;
        public IUserCollectionRepository UserCollectionRepository =>
            _userCollectionRepository ??= new UserCollectionRepository(_context);

        IRatingRepository _ratingRepository;
        public IRatingRepository RatingRepository =>
            _ratingRepository ??= new RatingRepository(_context);

        IUserRepository _userRepository;
        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context);

        public SignInManager<ApplicationUser> SignInManager { get; }

        public UserManager<ApplicationUser> UserManager { get; }

        public RoleManager<IdentityRole> RoleManager { get; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
