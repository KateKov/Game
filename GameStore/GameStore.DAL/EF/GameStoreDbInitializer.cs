using System.Data.Entity;

namespace GameStore.DAL.EF
{
    public class GameStoreDbInitializer : DropCreateDatabaseAlways<GameStoreContext>
    {
        protected override void Seed(GameStoreContext context)
        {
            //var platforms = new List<PlatformType>
            //{
            //    new PlatformType
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<PlatformTypeTranslate>
            //        {
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Mobile"
            //            },
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Мобильные"
            //            }
            //        }
            //    },
            //    new PlatformType
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<PlatformTypeTranslate>
            //        {
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Desktop"
            //            },
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Настольные"
            //            }
            //        }
            //    },
            //    new PlatformType
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<PlatformTypeTranslate>
            //        {
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Browser"
            //            },
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Браузерные"
            //            }
            //        }
            //    },
            //    new PlatformType
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<PlatformTypeTranslate>
            //        {
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Console"
            //            },
            //            new PlatformTypeTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Консольные"
            //            }
            //        }
            //    }
            //};
            //context.PlatformTypes.AddRange(platforms);
            //var genres = new List<Genre>
            //{
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Strategy"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Стратегия"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "RPG"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Ролевая игра"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Sports"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Спорт"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Races"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Гонки"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Action"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Экшн"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Adventure"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Приключение"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Puzzle&Skill"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Головоломки"
            //            }
            //        }
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Misc"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Разное"
            //            }
            //        }
            //    }
            //};
            //var subgenres = new List<Genre>
            //{
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "RTS"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Стратегия в реальном времени"
            //            }
            //        },
            //        ParentGenre = genres[0]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "TBS"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Пошаговая стратегия"
            //            }
            //        },
            //        ParentGenre = genres[0]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Rally"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Ралли"
            //            }
            //        },
            //        ParentGenre = genres[3]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Arcade"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Аркада"
            //            }
            //        },
            //        ParentGenre = genres[3]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Formula"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Формула"
            //            }
            //        },
            //        ParentGenre = genres[3]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "Off-road"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Внедорожние"
            //            }
            //        },
            //        ParentGenre = genres[3]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "FPS"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Шутер от первого лица"
            //            }
            //        },
            //        ParentGenre = genres[4]
            //    },
            //    new Genre
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<GenreTranslate>
            //        {
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.en,
            //                Name = "TPS"
            //            },
            //            new GenreTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Language = Language.ru,
            //                Name = "Шутер от третьего лица"
            //            }
            //        },
            //        ParentGenre = genres[4]
            //    }
            //};
            //context.Genres.AddRange(genres);
            //context.Genres.AddRange(subgenres);
            ////var firstLevelComments = new List<Comment>
            ////{
            ////    new Comment
            ////    {
            ////        Id = Guid.NewGuid(),
            ////        Name = "Marina",
            ////        Body = "First comment"
            ////    },
            ////    new Comment
            ////    {
            ////        Id = Guid.NewGuid(),
            ////        Name = "Artur",
            ////        Body = "Second comment"
            ////    }
            ////};
            ////var secondLevelComments = new List<Comment>
            ////{
            ////    new Comment
            ////    {
            ////        Id = Guid.NewGuid(),
            ////        Name = "Anna",
            ////        Body = "Answer for the first comment",
            ////        ParentComment = firstLevelComments[0]
            ////    },
            ////    new Comment
            ////    {
            ////        Id = Guid.NewGuid(),
            ////        Name = "Ivan",
            ////        Body = "Some joke",
            ////        ParentComment = firstLevelComments[0]
            ////    }
            ////};
            ////var thirdLevelComments = new List<Comment>
            ////{
            ////    new Comment
            ////    {
            ////        Id = Guid.NewGuid(),
            ////        Name = "Dmitriy",
            ////        Body = "Anna, i write you responce",
            ////        ParentComment = secondLevelComments[0]
            ////    }
            ////};
            ////var comments = firstLevelComments;
            ////comments.AddRange(secondLevelComments);
            ////comments.AddRange(thirdLevelComments);
            //var publishers = new List<Publisher>
            //{
            //    new Publisher
            //    {
            //        Id = Guid.NewGuid(),
            //        Translates = new List<PublisherTranslate>
            //        {
            //            new PublisherTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Cool publisher",
            //                Name = "GamesCorporation",
            //                Language = Language.en
            //            },
            //            new PublisherTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Классный",
            //                Name = "Корпорация игр",
            //                Language = Language.ru
            //            }
            //        },
            //        HomePage = "www.tratratra.com",
            //    }
            //};
            //context.Publishers.AddRange(publishers);
            //var games = new List<Game>
            //{
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Gta6_Third_Edition",
            //        FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "It's very interesting game",
            //                Name = "Gta6",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Это очень интересная игра",
            //                Name = "ГТА 6",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[1]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[1]
            //        },
            //        //Comments = comments,
            //        Discountinues = false,
            //        Price = 15,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Sims3_16_in_1",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Sims3",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Симс 3",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[2],
            //            platforms[0]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            genres[1],
            //            genres[4]
            //        },
            //        Discountinues = false,
            //        Price = 10,
            //        UnitsInStock = 20,
            //        Publisher = publishers[0],
            //        DateOfAdding = DateTime.UtcNow.AddDays(-4)
            //    }
            //};
            //var additionalGames = new List<Game>
            //{
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game3_Game3",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game3",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра3",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[2]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[2]
            //        },
            //        Discountinues = false,
            //        Price = 150,
            //        Publisher = publishers[0],
            //        UnitsInStock = 10,
            //        DateOfAdding = DateTime.UtcNow.AddMonths(-9)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game4",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game4",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра4",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[2]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[3]
            //        },
            //        Discountinues = false,
            //        Price = 12,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddMonths(-1)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game5",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game5",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра5",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[2]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[1],
            //            subgenres[2]
            //        },
            //        Discountinues = false,
            //        Price = 220,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddYears(-3)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game7",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game7",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра7",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[1]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[1]
            //        },
            //        Discountinues = false,
            //        Price = 15,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddYears(-2)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game8",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game8",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра8",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[1]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[1]
            //        },
            //        Discountinues = false,
            //        Price = 15,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddYears(-1)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game9",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game9",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра9",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[1]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[1]
            //        },
            //        Discountinues = false,
            //        Price = 15,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game10",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game10",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра10",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[2]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[1],
            //            subgenres[5]
            //        },
            //        Discountinues = false,
            //        Price = 15,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddMonths(-4)
            //    },
            //    new Game
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = "Game11",
            //         FilePath = "default.jpg",
            //        Translates = new List<GameTranslate>
            //        {
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Name = "Game11",
            //                Description = "You'll spend a lot of time playing in this game",
            //                Language = Language.en
            //            },
            //            new GameTranslate
            //            {
            //                Id = Guid.NewGuid(),
            //                Description = "Вы потратите на эту игру очень много времени",
            //                Name = "Игра11",
            //                Language = Language.ru
            //            }
            //        },
            //        PlatformTypes = new List<PlatformType>
            //        {
            //            platforms[0],
            //            platforms[1]
            //        },
            //        Genres = new List<Genre>
            //        {
            //            subgenres[0],
            //            subgenres[1]
            //        },
            //        Discountinues = false,
            //        Price = 15,
            //        UnitsInStock = 20,
            //        DateOfAdding = DateTime.UtcNow.AddMonths(-4)
            //    }
            //};
            //context.Games.AddRange(games);
            //context.Games.AddRange(additionalGames);
            //var roles = new List<Role>()
            //{
            //    new Role()
            //    {
            //        Id = Guid.NewGuid(),
            //        IsDefault = false,
            //        Translates = new List<RoleTranslate>()
            //        {
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "User"},
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.ru, Name = "Пользователь"}
            //        }
            //    },
            //      new Role()
            //    {
            //        Id = Guid.NewGuid(),
            //        IsDefault = false,
            //        Translates = new List<RoleTranslate>()
            //        {
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "Administrator"},
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.ru, Name = "Администратор"}
            //        }
            //    },
            //        new Role()
            //    {
            //        Id = Guid.NewGuid(),
            //        IsDefault = false,
            //        Translates = new List<RoleTranslate>()
            //        {
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "Moderator"},
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.ru, Name = "Модератор"}
            //        }
            //    },
            //          new Role()
            //    {
            //        Id = Guid.NewGuid(),
            //        IsDefault = true,
            //        Translates = new List<RoleTranslate>()
            //        {
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "Guest"},
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.ru, Name = "Гость"}
            //        }
            //    },
            //            new Role()
            //    {
            //        Id = Guid.NewGuid(),
            //        IsDefault = false,
            //        Translates = new List<RoleTranslate>()
            //        {
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "Manager"},
            //            new RoleTranslate() {Id = Guid.NewGuid(), Language = Language.ru, Name = "Менеджер"}
            //        }
            //    }
            //};
            //context.Roles.AddRange(roles);

            //var users = new List<User>
            //{
            //    new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Username = "Admin",
            //        CreateDate = DateTime.UtcNow,
            //        Email = "admin@admin.com",
            //        Roles = new List<Role> {roles[1]},
            //        PasswordHash = "qWj1V546L1BLMRkik+5zSuJXCPlFoSh7TKZg+4FACqk=",
            //        PasswordSalt = "zPJTssg3NzqrQhVqj0Nh0A=="
            //    },
            //    new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Username = "manager",
            //        CreateDate = DateTime.UtcNow,
            //        Email = "manager@manager.com",
            //        Roles = new List<Role> {roles[4]},
            //        PasswordHash = "qWj1V546L1BLMRkik+5zSuJXCPlFoSh7TKZg+4FACqk=",
            //        PasswordSalt = "zPJTssg3NzqrQhVqj0Nh0A=="
            //    },
            //    new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Username = "moderator",
            //        CreateDate = DateTime.UtcNow,
            //        Email = "moderator@moderator.com",
            //        Roles = new List<Role> {roles[2]},
            //        PasswordHash = "qWj1V546L1BLMRkik+5zSuJXCPlFoSh7TKZg+4FACqk=",
            //        PasswordSalt = "zPJTssg3NzqrQhVqj0Nh0A=="
            //    },
            //      new User
            //    {
            //        Id = Guid.NewGuid(),
            //        Username = "user",
            //        CreateDate = DateTime.UtcNow,
            //        Email = "user@user.com",
            //        Roles = new List<Role> {roles[0]},
            //        PasswordHash = "qWj1V546L1BLMRkik+5zSuJXCPlFoSh7TKZg+4FACqk=",
            //        PasswordSalt = "zPJTssg3NzqrQhVqj0Nh0A=="
            //    }
            //};

            //context.Users.AddRange(users);

            //var order = new Order
            //{
            //    Id = Guid.NewGuid(),
            //    Date = DateTime.UtcNow,
            //    IsConfirmed = true,
            //    IsDeleted = false,
            //    IsPayed = false,
            //    IsShipped = false,
            //    OrderDetails = new List<OrderDetail>
            //    {
            //        new OrderDetail
            //        {
            //            Discount = 2,
            //            Game = games[1],
            //            Id = Guid.NewGuid(),
            //            IsDeleted = false,
            //            IsPayed = false,
            //            Quantity = 4,
            //            Price = games[1].Price
            //        }
            //    },
            //    User = users[3]
            //};
            //context.Orders.Add(order);
            //context.SaveChanges();
        }
    }
}