using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public class GameLibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserGameLibrary> UserGamesLibrary { get; set; }

        public GameLibraryDbContext(DbContextOptions<GameLibraryDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string adminId = Guid.NewGuid().ToString();
            string roleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "user",
                NormalizedName = "USER"
            });

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminId,
                Nickname = "Admin",
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PasswordHash = hasher.HashPassword(null, "Pass22"),
                SecurityStamp = Guid.NewGuid().ToString()
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = adminId,
            });

            builder.Entity<Game>()
                .HasData(new Game { 
                    Id = 1, Name = "Mafia: Definitive Edition", 
                    HeaderImage = "https://i.ibb.co/C0JT8QG/mafia-definitive-edition.jpg", 
                    Genre = "Action, Adventure, Crime",
                    Description = "An inadvertent brush with the mob thrusts cabdriver Tommy Angelo into the world of organized crime. Initially uneasy about falling in with the Salieri family, the rewards become too big to ignore.",
                    Trailer = "https://www.youtube.com/embed/vfwfA_iTOng",
                    Rating = "18",
                    Developer = "Hangar 13",
                    ReleaseDate = new DateTime(2020, 9, 25)
            });

            builder.Entity<Game>()
                .HasData(new Game
                {
                    Id = 2,
                    Name = "Assassin's Creed Unity",
                    HeaderImage = "https://i.ibb.co/sJ6ZJB8/assassin-s-creed-unity.jpg",
                    Genre = "Action, Adventure",
                    Description = "Assassin’s Creed® Unity is an action/adventure game set in the city of Paris during one of its darkest hours, the French Revolution. Take ownership of the story by customising Arno's equipement to make the experience unique to you, both visually and mechanically. In addition to an epic single-player experience, Assassin’s Creed Unity delivers the excitement of playing with up to three friends through online cooperative gameplay in specific missions. Throughout the game, take part in one of the most pivotal moments of French history in a compelling storyline and a breath-taking playground that brought you the city of lights of today.",
                    Trailer = "https://www.youtube.com/embed/xzCEdSKMkdU",
                    Rating = "18",
                    Developer = "Ubisoft",
                    ReleaseDate = new DateTime(2014, 11, 12)
                });

            builder.Entity<Game>()
                .HasData(new Game
                {
                    Id = 3,
                    Name = "Forza Horizon 4",
                    HeaderImage = "https://i.ibb.co/Hz70tC8/forza-horizon-4.jpg",
                    Genre = "Racing",
                    Description = "Dynamic seasons change everything at the world’s greatest automotive festival. Go it alone or team up with others to explore beautiful and historic Britain in a shared open world. Collect, modify and drive over 450 cars. Race, stunt, create and explore – choose your own path to become a Horizon Superstar.",
                    Trailer = "https://www.youtube.com/embed/5xy4n73WOMM",
                    Rating = "3",
                    Developer = "Playground Games",
                    ReleaseDate = new DateTime(2021, 3, 9)
                });

            //builder.Entity<Comment>()
            //    .HasData(new Comment { Id = 1, ApplicationUserId = "1", GameId = 1, Content = "A" });


            //builder.Entity<Comment>()
            //    .HasData(new Comment { Id = 2, ApplicationUserId = "1", GameId = 1, Content = "A", ReplyToCommentId = 1 });

            //builder.Entity<ApplicationUser>()
            //    .HasData(new ApplicationUser { Id = "1", Nickname = "Sancho"});
        }

    }
}
