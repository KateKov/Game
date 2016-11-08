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
                    Name = "Mobile"
                },
                new PlatformType
                {
                    Name = "Desktop"
                },
                new PlatformType
                {
                    Name = "Browser"
                },
                new PlatformType
                {
                    Name = "Console"
                }
            };
            db.PlatformTypes.AddRange(platforms);
            var genres = new List<Genre>
            {
                new Genre
                {
                    Name = "Strategy"
                },
                new Genre
                {
                    Name = "RPG"
                },
                new Genre
                {
                    Name = "Sports"
                },
                new Genre
                {
                    Name = "Races"
                },
                new Genre
                {
                    Name = "Action"
                },
                new Genre
                {
                    Name = "Adventure"
                },
                new Genre
                {
                    Name = "Puzzle&Skill"
                },
                new Genre
                {
                    Name = "Misc"
                }
            };
            var subgenres = new List<Genre>
            {
                new Genre
                {
                    Name = "RTS",
                    ParentGenre = genres[0]
                },
                new Genre
                {
                    Name = "TBS",
                    ParentGenre = genres[0]
                },
                new Genre
                {
                    Name = "Rally",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Name = "Arcade",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Name = "Formula",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Name = "Off-road",
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    Name = "FPS",
                    ParentGenre = genres[4]
                },
                new Genre
                {
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
                    Id = 1,
                    Name = "Marina",
                    Body = "First comment"
                },
                new Comment
                {
                    Id = 2,
                    Name = "Artur",
                    Body = "Second comment"
                }
            };
            var secondLevelComments = new List<Comment>
            {
                new Comment
                {
                    Id = 3,
                    Name = "Anna",
                    Body = "Answer for the first comment",
                    ParentComment = firstLevelComments[0]
                },
                new Comment
                {
                    Id = 4,
                    Name = "Ivan",
                    Body = "Some joke",
                    ParentComment = firstLevelComments[0]
                }
            };
            var thirdLevelComments = new List<Comment>
            {
                new Comment
                {
                    Id = 5,
                    Name = "Dmitriy",
                    Body = "Anna, i write you responce",
                    ParentComment = secondLevelComments[0]
                }
            };
            var comments = firstLevelComments;
            comments.AddRange(secondLevelComments);
            comments.AddRange(thirdLevelComments);
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    Discount = 15,
                    Quality = 4,
                    Price = 15
                },
                new OrderDetail
                {
                    Discount = 20,
                    Quality = 18,
                    Price = 20
                }
            };
            db.OrderDetails.AddRange(orderDetails);
            var games = new List<Game>
            {
                new Game
                {
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
                    Price=15,
                    UnitsInStock = 20,
                    OrderDetails = new List<OrderDetail> {orderDetails[0]}
                },
                new Game
                {
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
                    Price=15,
                    UnitsInStock = 20,
                    OrderDetails = new List<OrderDetail> {orderDetails[1]}
                }
            };
            db.Games.AddRange(games);
            var orders = new List<Order>
            {
                new Order
                {
                    CustomerId = 1,
                    Date = DateTime.UtcNow,
                    OrderDetails = new List<OrderDetail> {orderDetails[0], orderDetails[1]}
                }
            };
            db.Orders.AddRange(orders);
            db.SaveChanges();
        }
    }
}
