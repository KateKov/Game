using System;
using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;
using NUnit.Framework;

namespace GameStore.BLL.Tests.Services
{
    public class TestData
    {
        public static IEnumerable CommentValidNoParent
        {
            get
            {
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Key = "key",
                    Translates =
                        new List<GameTranslate>()
                        {
                            new GameTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                        }
                };
                yield return
                    new TestCaseData(
                        new CommentDTO
                        {
                            Name = "name",
                            Body = "body",
                            GameId = game.Id.ToString(),
                            GameKey = game.Key
                        },
                        game);
            }
        }

        public static IEnumerable CommentValidWithParent
        {
            get
            {
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Key = "key",
                    Translates =
                        new List<GameTranslate>()
                        {
                            new GameTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                        }
                };
                var parrentComment = new Comment
                {
                    Id = Guid.NewGuid(),
                    Name = "name",
                    Body = "body",
                    Game = game
                };
                yield return new TestCaseData(
                    new CommentDTO
                    {
                        Name = "stub-name",
                        Body = "stub-body",
                        ParentCommentId = parrentComment.Id.ToString(),
                        ParrentCommentName = "name",
                        GameId = game.Id.ToString(),
                        GameKey = game.Key
                    },
                    parrentComment, game);
            }
        }

        public static IEnumerable CommentInvalidNoParrent
        {
            get
            {
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Key = "stub-key",
                    Translates =
                        new List<GameTranslate>()
                        {
                            new GameTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                        }
                };
                yield return
                    new TestCaseData(
                        new CommentDTO
                        {
                            Name = string.Empty,
                            Body = "stub-body",
                            GameId = game.Id.ToString(),
                            GameKey = game.Key
                        },
                        game);
                yield return
                    new TestCaseData(
                        new CommentDTO
                        {
                            Name = "stub-name",
                            Body = string.Empty,
                            GameId = game.Id.ToString(),
                            GameKey = game.Key
                        },
                        game);
                yield return
                    new TestCaseData(
                        new CommentDTO
                        {
                            Name = string.Empty,
                            Body = string.Empty,
                            GameId = game.Id.ToString(),
                            GameKey = game.Key
                        },
                        game);
                yield return
                    new TestCaseData(
                        new CommentDTO {Name = null, Body = null, GameId = game.Id.ToString(), GameKey = game.Key},
                        game);
                yield return
                    new TestCaseData(
                        new CommentDTO
                        {
                            Name = null,
                            Body = "stub-body",
                            GameId = game.Id.ToString(),
                            GameKey = game.Key
                        },
                        game);
                yield return
                    new TestCaseData(
                        new CommentDTO
                        {
                            Name = "stub-name",
                            Body = null,
                            GameId = game.Id.ToString(),
                            GameKey = game.Key
                        },
                        game);
                yield return new TestCaseData(null, game);
                yield return
                    new TestCaseData(new CommentDTO {Name = "stub-name", Body = "stub-body", GameId = ""},
                        game);
            }
        }

        public static IEnumerable CommentInvalidWithParrent
        {
            get
            {
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Key = "key",
                    Translates =
                        new List<GameTranslate>()
                        {
                            new GameTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                        }
                };
                var parrentComment = new Comment
                {
                    Id = Guid.NewGuid(),
                    Name = "name",
                    Body = "body",
                    Game = game
                };
                yield return new TestCaseData(
                    new CommentDTO
                    {
                        Name = "stub-name",
                        Body = "stub-body",
                        ParentCommentId = parrentComment.Id.ToString(),
                        ParrentCommentName = "name",
                        GameId = game.Id.ToString()
                    },
                    parrentComment, game);
            }
        }

        public static IEnumerable GenreValidNoLists
        {
            get
            {
                yield return
                    new TestCaseData(new GenreDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Translates =
                            new List<GenreDTOTranslate>()
                            {
                                new GenreDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name"
                                }
                            }
                    });
                yield return
                    new TestCaseData(new GenreDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Translates =
                            new List<GenreDTOTranslate>()
                            {
                                new GenreDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name"
                                }
                            }
                    });
            }
        }

        public static IEnumerable OrderValid
        {
            get
            {
                yield return new TestCaseData(new OrderDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = "",
                    Date = DateTime.UtcNow,
                    IsConfirmed = false
                });
            }
        }

        public static IEnumerable BasketValid
        {
            get
            {
                yield return new TestCaseData(new List<OrderDTO>
                    {
                        new OrderDTO
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerId = "",
                            Date = DateTime.UtcNow,
                            IsConfirmed = false
                        },
                        new OrderDTO
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerId = "",
                            Date = DateTime.UtcNow,
                            IsConfirmed = true
                        }
                    }
                );
            }
        }

        public static IEnumerable GameForOrderDetails
        {
            get
            {
                yield return
                    new TestCaseData(
                        new Game()
                        {
                            Id = Guid.NewGuid(),
                            Key = "key",
                            Translates =
                                new List<GameTranslate>()
                                {
                                    new GameTranslate()
                                    {
                                        Name = "name",
                                        Id = Guid.NewGuid(),
                                        Language = Language.en
                                    }
                                },
                            Publisher = new Publisher()
                            {
                                Id = Guid.NewGuid(),
                                Translates =
                                    new List<PublisherTranslate>()
                                    {
                                        new PublisherTranslate()
                                        {
                                            Id = Guid.NewGuid(),
                                            Language = Language.en,
                                            Name = "publisher"
                                        }
                                    }
                            },
                            Genres =
                                new List<Genre>()
                                {
                                    new Genre()
                                    {
                                        Id = Guid.NewGuid(),
                                        Translates =
                                            new List<GenreTranslate>()
                                            {
                                                new GenreTranslate()
                                                {
                                                    Id = Guid.NewGuid(),
                                                    Language = Language.en,
                                                    Name = "genre"
                                                }
                                            }
                                    }
                                },
                            PlatformTypes =
                                new List<PlatformType>()
                                {
                                    new PlatformType()
                                    {
                                        Id = Guid.NewGuid(),
                                        Translates =
                                            new List<PlatformTypeTranslate>()
                                            {
                                                new PlatformTypeTranslate()
                                                {
                                                    Id = Guid.NewGuid(),
                                                    Language = Language.en,
                                                    Name = "platform"
                                                }
                                            }
                                    }
                                }
                        });
            }
        }

        public static IEnumerable OrderInvalid
        {
            get
            {
                yield return new TestCaseData(new OrderDTO
                {
                    Id = "",
                    CustomerId = "",
                    Date = DateTime.UtcNow,
                    IsConfirmed = false,
                });
            }
        }

        public static IEnumerable GameInvalidWithLists
        {
            get
            {
                var genre = new Genre()
                {
                    Translates =
                        new List<GenreTranslate>()
                        {
                            new GenreTranslate() {Id = Guid.NewGuid(), Language = Language.en, Name = "name"}
                        }
                };
                yield return
                    new TestCaseData(new GameDTO
                        {
                            Id = "",
                            Key = "key",
                            Translates = new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name",
                                    Description = "stub-description",
                                    GenresName = new List<string>() {"name"},
                                    PlatformTypesName = new List<string>() {"naame"}
                                }
                            }
                        }, new List<Genre> {genre},
                        new List<PlatformType>
                        {
                            new PlatformType()
                            {
                                Id = Guid.NewGuid(),
                                Translates =
                                    new List<PlatformTypeTranslate>()
                                    {
                                        new PlatformTypeTranslate()
                                        {
                                            Id = Guid.NewGuid(),
                                            Language = Language.en,
                                            Name = "naame"
                                        }
                                    }
                            }
                        });
            }
        }

        public static IEnumerable OrderValidItems
        {
            get
            {
                var publisherTranslate = new PublisherTranslate
                {
                    Id = Guid.NewGuid(),
                    Name = "Name"
                };
                var publisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    Translates = new List<PublisherTranslate>
                {
                    publisherTranslate
                }
                };
                var platformTypeTranslate = new PlatformTypeTranslate
                {
                    Id = Guid.NewGuid(),
                    Name = "name"
                };
                var platformType = new PlatformType
                {
                    Id = Guid.NewGuid(),
                    Translates = new List<PlatformTypeTranslate>
                {
                    platformTypeTranslate
                }
                };
                var genreTranslate = new GenreTranslate
                {
                    Id = Guid.NewGuid(),
                    Name = "name"
                };
                var genre = new Genre
                {
                    Id = Guid.NewGuid(),
                    Translates = new List<GenreTranslate>
                {
                    genreTranslate
                }
                };
                var gameId = Guid.NewGuid();
                var gameTranslate = new GameTranslate
                {
                    Id = Guid.NewGuid(),
                    Language = Language.en,
                    Name = "name"
                };
                var game = new Game
                {
                    Id = gameId,
                    DateOfAdding = DateTime.UtcNow,
                    Key = "key",
                    PlatformTypes = new List<PlatformType> { platformType },
                    Genres = new List<Genre> { genre },
                    Translates = new List<GameTranslate>
                {
                    gameTranslate
                }
                };
                yield return new TestCaseData(new Order
                {
                    Id = Guid.NewGuid(),
                    User = new User { Id = Guid.NewGuid(), Username = "name" },
                    Date = DateTime.UtcNow,
                    IsConfirmed = false,
                    OrderDetails = new List<OrderDetail>()
                {
                    new OrderDetail()
                        {Id = Guid.NewGuid(), GameId = gameId, Game = game}
                },
                    Sum = 0,
                    IsPayed = false,
                    IsDeleted = false,
                    IsShipped = false
                },
                game, gameId,
                platformType, platformTypeTranslate,
                gameTranslate, genre, genreTranslate,
                publisher, publisherTranslate
                );
            }
        }


        public static IEnumerable GameInvalidNoLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO());
                yield return new TestCaseData(null);
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = string.Empty,
                        Translates =
                            new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name",
                                    Description = "description"
                                }
                            }
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = string.Empty,
                        Translates =
                            new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = string.Empty,
                                    Description = "description"
                                }
                            }
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = null,
                        Translates =
                            new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = "name",
                                    Description = "description"
                                }
                            }
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = "stub-key",
                        Translates =
                            new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = null,
                                    Description = "description"
                                }
                            }
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = null,
                        Translates =
                            new List<GameDTOTranslate>()
                            {
                                new GameDTOTranslate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Language = Language.en,
                                    Name = null,
                                    Description = "description"
                                }
                            }
                    });
            }
        }
    }
}