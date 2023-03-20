using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DAL.Infrastructure
{
    public class GameLibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public GameLibraryDbContext(DbContextOptions<GameLibraryDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GameGenre>().HasKey(gg => new { gg.GameId, gg.GenreId });
            builder.Entity<UserCollection>().HasKey(uc => new { uc.UserGameId, uc.CollectionId });
            builder.Entity<Rating>().HasKey(r => new { r.ApplicationUserId, r.GameId });

            builder.Entity<Game>()
                .Property(g => g.ReleaseDate)
                .HasConversion(
                 v => v.ToString("yyyy-MM-dd"),
                 v => DateOnly.Parse(v)
        );

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
                .HasData(new Game
                {
                    Id = 1,
                    Name = "Mafia: Definitive Edition",
                    HeaderImage = "https://i.ibb.co/C0JT8QG/mafia-definitive-edition.jpg",
                    //Genre = "Action, Adventure, Crime",
                    Description = "An inadvertent brush with the mob thrusts cabdriver Tommy Angelo into the world of organized crime. Initially uneasy about falling in with the Salieri family, the rewards become too big to ignore.",
                    Trailer = "https://www.youtube.com/embed/vfwfA_iTOng",
                    Rating = "18",
                    Developer = "Hangar 13",
                    ReleaseDate = new DateOnly(2020, 9, 25)
                });

            builder.Entity<Game>()
                .HasData(new Game
                {
                    Id = 2,
                    Name = "Assassin's Creed Unity",
                    HeaderImage = "https://i.ibb.co/sJ6ZJB8/assassin-s-creed-unity.jpg",
                    //Genre = "Action, Adventure",
                    Description = "Assassin’s Creed® Unity is an action/adventure game set in the city of Paris during one of its darkest hours, the French Revolution. Take ownership of the story by customising Arno's equipement to make the experience unique to you, both visually and mechanically. In addition to an epic single-player experience, Assassin’s Creed Unity delivers the excitement of playing with up to three friends through online cooperative gameplay in specific missions. Throughout the game, take part in one of the most pivotal moments of French history in a compelling storyline and a breath-taking playground that brought you the city of lights of today.",
                    Trailer = "https://www.youtube.com/embed/xzCEdSKMkdU",
                    Rating = "18",
                    Developer = "Ubisoft",
                    ReleaseDate = new DateOnly(2014, 11, 12)
                });

            builder.Entity<Game>()
                .HasData(new Game
                {
                    Id = 3,
                    Name = "Forza Horizon 4",
                    HeaderImage = "https://i.ibb.co/Hz70tC8/forza-horizon-4.jpg",
                    //Genre = "Racing",
                    Description = "Dynamic seasons change everything at the world’s greatest automotive festival. Go it alone or team up with others to explore beautiful and historic Britain in a shared open world. Collect, modify and drive over 450 cars. Race, stunt, create and explore – choose your own path to become a Horizon Superstar.",
                    Trailer = "https://www.youtube.com/embed/5xy4n73WOMM",
                    Rating = "3",
                    Developer = "Playground Games",
                    ReleaseDate = new DateOnly(2021, 3, 9)
                });

            builder.Entity<Genre>()
                .HasData(new Genre
                {
                    Id = 1,
                    Name = "Action",
                    Description = "An action game is a video game genre that emphasizes physical challenges, including hand–eye coordination and reaction-time. The genre includes a large variety of sub-genres, such as fighting games, beat 'em ups, shooter games, and platform games."
                });

            builder.Entity<Genre>()
                .HasData(new Genre
                {
                    Id = 2,
                    Name = "Strategy",
                    Description = "There are two major types of electronic strategy games: turn-based strategy (TBS) and real-time strategy (RTS). Although some TBS games have experimented with multiplayer support, the slow pace of waiting for each player to finish managing all of his or her resources and units has limited their appeal."
                });

            builder.Entity<Genre>()
                .HasData(new Genre
                {
                    Id = 3,
                    Name = "Adventure",
                    Description = "An adventure game is a video game in which the player assumes the role of a protagonist in an interactive story driven by exploration and/or puzzle-solving. The genre's focus on story allows it to draw heavily from other narrative-based media, literature and film, encompassing a wide variety of literary genres."
                });

            builder.Entity<Genre>()
                .HasData(new Genre
                {
                    Id = 4,
                    Name = "Crime",
                    Description = "Crime is the genre that fictionalises crimes, their detection, criminals and their motives."
                });

            builder.Entity<Genre>()
                .HasData(new Genre
                {
                    Id = 5,
                    Name = "Racing",
                    Description = "Racing games are a video game genre in which the player participates in a racing competition. They may be based on anything from real-world racing leagues to fantastical settings. They are distributed along a spectrum between more realistic racing simulations and more fantastical arcade-style racing games."
                });

            builder.Entity<GameGenre>()
                .HasData(new GameGenre
                {
                    GameId = 1,
                    GenreId = 1
                });

            builder.Entity<GameGenre>()
                .HasData(new GameGenre
                {
                    GameId = 1,
                    GenreId = 3
                });

            builder.Entity<GameGenre>()
                .HasData(new GameGenre
                {
                    GameId = 1,
                    GenreId = 4
                });

            builder.Entity<GameGenre>()
               .HasData(new GameGenre
               {
                   GameId = 2,
                   GenreId = 1
               });

            builder.Entity<GameGenre>()
               .HasData(new GameGenre
               {
                   GameId = 2,
                   GenreId = 3
               });

            builder.Entity<GameGenre>()
               .HasData(new GameGenre
               {
                   GameId = 3,
                   GenreId = 5
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
