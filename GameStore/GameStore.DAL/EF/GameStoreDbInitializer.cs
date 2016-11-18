using System;
using System.Collections.Generic;
using System.Data.Entity;
using GameStore.DAL.Entities;

namespace GameStore.DAL.EF
{
    public class GameStoreDbInitializer : DropCreateDatabaseAlways<GameStoreContext>
    {
        protected override void Seed(GameStoreContext db)
        {
            var platforms = new List<PlatformType>
            {
                new PlatformType
                {
                    Id = Guid.NewGuid(),
                    Name = "Mobile"
                },
                new PlatformType
                {
                    Id = Guid.NewGuid(),
                    Name = "Desktop"
                },
                new PlatformType
                {
                    Id = Guid.NewGuid(),
                    Name = "Browser"
                },
                new PlatformType
                {
                    Id = Guid.NewGuid(),
                    Name = "Console"
                }
            };
            db.PlatformTypes.AddRange(platforms);
            var genres = new List<Genre>
            {
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Strategy"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "RPG"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Sports"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Races"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Action"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Adventure"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Puzzle&Skill"
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Misc"
                }
            };
            var subgenres = new List<Genre>
            {
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "RTS",
                    ParentGenre = genres[0]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "TBS",
                    ParentGenre = genres[0]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Rally",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Arcade",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Formula",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Off-road",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "FPS",
                    ParentGenre = genres[4]
                },
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "TPS",
                    ParentGenre = genres[4]
                }          
            };
            db.Genres.AddRange(genres);
            db.Genres.AddRange(subgenres);
            var firstLevelComments = new List<Comment>
            {
                new Comment
                {
                    Id = Guid.NewGuid(),
                    Name = "Marina",
                    Body = "First comment"
                },
                new Comment
                {
                    Id = Guid.NewGuid(),
                    Name = "Artur",
                    Body = "Second comment"
                }
            };
            var secondLevelComments = new List<Comment>
            {
                new Comment
                {
                    Id = Guid.NewGuid(),
                    Name = "Anna",
                    Body = "Answer for the first comment",
                    ParentComment = firstLevelComments[0]
                },
                new Comment
                {
                   Id = Guid.NewGuid(),
                    Name = "Ivan",
                    Body = "Some joke",
                    ParentComment = firstLevelComments[0]
                }
            };
            var thirdLevelComments = new List<Comment>
            {
                new Comment
                {
                    Id = Guid.NewGuid(),
                    Name = "Dmitriy",
                    Body = "Anna, i write you responce",
                    ParentComment = secondLevelComments[0]        
                }
            };
            var comments = firstLevelComments;
            comments.AddRange(secondLevelComments);
            comments.AddRange(thirdLevelComments);        
            var publishers = new List<Publisher>
            {
                new Publisher
                {
                    Id = Guid.NewGuid(),
                    Description = "Cool publisher",
                    HomePage = "www.tratratra.com",
                    Name = "GamesCorporation"
                }
            };
            db.Publishers.AddRange(publishers);
            var games = new List<Game>
            {
                new Game
                {
                    Id = Guid.NewGuid(),
                    Key = "Gta6_ThirdEdition",
                    Name = "Gta6",
                    Description = "It's very interesting game",
                    PlatformTypes = new List<PlatformType>
                    {
                        platforms[0],
                        platforms[1]
                    },
                    Genres = new List<Genre>
                    {
                        subgenres[0],
                        subgenres[1]
                    },
                    Comments = comments,
                    Discountinues = false,
                    Price = 15,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddMonths(-4)
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Key = "Sims3_16in1",
                    Name = "Sims3",
                    Description = "You'll spend a lot of time playing in this game",
                    PlatformTypes = new List<PlatformType>
                    {
                        platforms[2],
                        platforms[0]
                    },
                    Genres = new List<Genre>
                    {
                        genres[1],
                        genres[4]
                    },
                    Discountinues = false,
                    Price = 10,
                    UnitsInStock = 20,
                    Publisher = publishers[0],
                    DateOfAdding = DateTime.UtcNow.AddDays(-4)
                }
            };
            db.Games.AddRange(games);
            db.SaveChanges();
        }
    }
}
