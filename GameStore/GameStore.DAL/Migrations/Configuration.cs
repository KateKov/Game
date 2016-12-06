namespace GameStore.DAL.Migrations
{
    using Entities;
    using Entities.Translation;
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GameStore.DAL.EF.GameStoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GameStore.DAL.EF.GameStoreContext context)
        {
            var platforms = new List<PlatformType>
            {
                new PlatformType
                {
                    EntityId = Guid.NewGuid(),
                    Translates = new List<PlatformTypeTranslate>()
                    {
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Mobile"
                        },
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Мобильные"
                        }
                    }
                },
                new PlatformType
                {
                    EntityId = Guid.NewGuid(),
                   Translates = new List<PlatformTypeTranslate>()
                    {
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Desktop"
                        },
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Настольные"
                        }
                    }
                },
                new PlatformType
                {
                    EntityId = Guid.NewGuid(),
                    Translates = new List<PlatformTypeTranslate>()
                    {
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Browser"
                        },
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Браузерные"
                        }
                    }
                },
                new PlatformType
                {
                    EntityId = Guid.NewGuid(),
                   Translates = new List<PlatformTypeTranslate>()
                    {
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Console"
                        },
                        new PlatformTypeTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Консольные"
                        }
                    }
                }
            };
            context.PlatformTypes.AddOrUpdate(x => x.EntityId, platforms.ToArray());
            var genres = new List<Genre>
            {
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                   Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Strategy"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Стратегия"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                     Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "RPG"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Ролевая игра"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                     Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Sports"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Спорт"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                     Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Races"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Гонки"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                     Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Action"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Экшн"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                     Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Adventure"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Приключение"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                     Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Puzzle&Skill"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Головоломки"
                        }
                    }
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                      Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Misc"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Разное"
                        }
                    }
                }
            };
            var subgenres = new List<Genre>
            {
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                      Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "RTS"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Стратегия в реальном времени"
                        }
                    },
                    ParentGenre = genres[0]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                       Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "TBS"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Пошаговая стратегия"
                        }
                    },
                    ParentGenre = genres[0]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                       Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Rally"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Ралли"
                        }
                    },
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                       Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Arcade"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Аркада"
                        }
                    },
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                       Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Formula"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Формула"
                        }
                    },
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                       Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "Off-road"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Внедорожние"
                        }
                    },
                    ParentGenre = genres[3]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                       Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "FPS"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Шутер от первого лица"
                        }
                    },
                    ParentGenre = genres[4]
                },
                new Genre
                {
                    EntityId = Guid.NewGuid(),
                      Translates = new List<GenreTranslate>()
                    {
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.En,
                            Name = "TPS"
                        },
                        new GenreTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Language = Language.Ru,
                            Name = "Шутер от третьего лица"
                        }
                    },
                    ParentGenre = genres[4]
                }
            };
            context.Genres.AddOrUpdate(x => x.EntityId, genres.ToArray());
            context.Genres.AddOrUpdate(x => x.EntityId, subgenres.ToArray());
            var firstLevelComments = new List<Comment>
            {
                new Comment
                {
                    EntityId = Guid.NewGuid(),
                    Name = "Marina",
                    Body = "First comment"
                },
                new Comment
                {
                    EntityId = Guid.NewGuid(),
                    Name = "Artur",
                    Body = "Second comment"
                }
            };
            var secondLevelComments = new List<Comment>
            {
                new Comment
                {
                    EntityId = Guid.NewGuid(),
                    Name = "Anna",
                    Body = "Answer for the first comment",
                    ParentComment = firstLevelComments[0]
                },
                new Comment
                {
                   EntityId = Guid.NewGuid(),
                    Name = "Ivan",
                    Body = "Some joke",
                    ParentComment = firstLevelComments[0]
                }
            };
            var thirdLevelComments = new List<Comment>
            {
                new Comment
                {
                    EntityId = Guid.NewGuid(),
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
                    EntityId = Guid.NewGuid(),
                    Translates = new List<PublisherTranslate>()
                    {
                        new PublisherTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Cool publisher",
                            Name = "GamesCorporation",
                            Language = Language.En
                        },
                        new PublisherTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Классный",
                            Name = "Корпорация игр",
                            Language = Language.Ru
                        }
                    },
                    HomePage = "www.tratratra.com",
                }
            };
            context.Publishers.AddOrUpdate(x => x.EntityId, publishers.ToArray());
            var games = new List<Game>
            {
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Gta6_Third_Edition",
                   Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "It's very interesting game",
                            Name = "Gta6",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Это очень интересная игра",
                            Name = "ГТА 6",
                            Language = Language.Ru
                        }
                    },
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
                    EntityId = Guid.NewGuid(),
                    Key = "Sims3_16_in_1",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Sims3",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Симс 3",
                            Language = Language.Ru
                        }
                    },
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
            var additionalGames = new List<Game>
            {
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game3_Game3",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game3",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Game3",
                            Language = Language.Ru
                        }
                    },
                    PlatformTypes = new List<PlatformType>
                    {
                        platforms[0],
                        platforms[2]
                    },
                    Genres = new List<Genre>
                    {
                        subgenres[0],
                        subgenres[2]
                    },
                    Discountinues = false,
                    Price = 150,
                    Publisher = publishers[0],
                    UnitsInStock = 10,
                    DateOfAdding = DateTime.UtcNow.AddMonths(-9)
                },
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game4",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game4",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра4",
                            Language = Language.Ru
                        }
                    },
                    PlatformTypes = new List<PlatformType>
                    {
                        platforms[0],
                        platforms[2]
                    },
                    Genres = new List<Genre>
                    {
                        subgenres[0],
                        subgenres[3]
                    },
                    Discountinues = false,
                    Price = 12,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddMonths(-1)
                },
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game5",
                   Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game5",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра5",
                            Language = Language.Ru
                        }
                    },
                    PlatformTypes = new List<PlatformType>
                    {
                        platforms[0],
                        platforms[2]
                    },
                    Genres = new List<Genre>
                    {
                        subgenres[1],
                        subgenres[2]
                    },

                    Discountinues = false,
                    Price = 220,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddYears(-3)
                },

                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game7",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game7",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра7",
                            Language = Language.Ru
                        }
                    },
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

                    Discountinues = false,
                    Price = 15,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddYears(-2)
                },
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game8",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game8",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра8",
                            Language = Language.Ru
                        }
                    },
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

                    Discountinues = false,
                    Price = 15,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddYears(-1)
                },
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game9",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game9",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра9",
                            Language = Language.Ru
                        }
                    },
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

                    Discountinues = false,
                    Price = 15,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddMonths(-4)
                },
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game10",
                    Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game10",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра10",
                            Language = Language.Ru
                        }
                    },
                    PlatformTypes = new List<PlatformType>
                    {
                        platforms[0],
                        platforms[2]
                    },
                    Genres = new List<Genre>
                    {
                        subgenres[1],
                        subgenres[5]
                    },

                    Discountinues = false,
                    Price = 15,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddMonths(-4)
                },
                new Game
                {
                    EntityId = Guid.NewGuid(),
                    Key = "Game11",
                   Translates = new List<GameTranslate>()
                    {
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Name = "Game11",
                            Description = "You'll spend a lot of time playing in this game",
                            Language = Language.En
                        },
                        new GameTranslate()
                        {
                            EntityId = Guid.NewGuid(),
                            Description = "Вы потратите на эту игру очень много времени",
                            Name = "Игра11",
                            Language = Language.Ru
                        }
                    },
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

                    Discountinues = false,
                    Price = 15,
                    UnitsInStock = 20,
                    DateOfAdding = DateTime.UtcNow.AddMonths(-4)
                }


            };
            context.Games.AddOrUpdate(x => x.EntityId, games.ToArray());
            context.Games.AddOrUpdate(x => x.EntityId, additionalGames.ToArray());
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
