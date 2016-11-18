using System;
using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using NUnit.Framework;

namespace GameStore.BLL.Tests.Services
{
    public class TestData
    {
        public static IEnumerable CommentValidNoParent
        {
            get
            {
                var game = new Game {Id = Guid.NewGuid(), Key = "key", Name = "name"};
                yield return new TestCaseData(new CommentDTO { Name = "name", Body = "body", GameId = game.Id.ToString(), GameKey = game.Key},
                    game);
            }
        }

        public static IEnumerable CommentValidWithParent
        {
            get
            {
                var game = new Game {Id = Guid.NewGuid(), Key = "key", Name = "name"};
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
                var game = new Game {Id = Guid.NewGuid(), Key = "stub-key", Name = "stub-name"};
                yield return
                    new TestCaseData(new CommentDTO { Name = string.Empty, Body = "stub-body", GameId = game.Id.ToString(), GameKey = game.Key},
                       game);
                yield return
                    new TestCaseData(new CommentDTO { Name = "stub-name", Body = string.Empty, GameId = game.Id.ToString(), GameKey = game.Key },
                        game);
                yield return
                    new TestCaseData(new CommentDTO { Name = string.Empty, Body = string.Empty, GameId = game.Id.ToString(), GameKey = game.Key },
                       game);
                yield return
                    new TestCaseData(new CommentDTO { Name = null, Body = null, GameId = game.Id.ToString(), GameKey = game.Key },
                        game);
                yield return
                new TestCaseData(new CommentDTO { Name = null, Body = "stub-body", GameId = game.Id.ToString(), GameKey = game.Key },
                    game);
                yield return
                new TestCaseData(new CommentDTO { Name = "stub-name", Body = null, GameId = game.Id.ToString(), GameKey = game.Key },
                    game);
                yield return new TestCaseData(null, game);
                yield return
                    new TestCaseData(new CommentDTO { Name = "stub-name", Body = "stub-body", GameId = "" },
                       game);
            }
        }

        public static IEnumerable CommentInvalidWithParrent
        {
            get
            {
                var game = new Game {Id = Guid.NewGuid(), Key = "key", Name = "name"};
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
                   parrentComment,game);
            }
        }

        public static IEnumerable GenreValidNoLists
        {
            get
            {
                yield return new TestCaseData(new GenreDTO { Id=Guid.NewGuid().ToString(), Name = "name" });
                yield return
                    new TestCaseData(new GenreDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "AnotherName"
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
                yield return new TestCaseData(new List<OrderDTO> {new OrderDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = "",
                    Date = DateTime.UtcNow,
                    IsConfirmed = false
                },new OrderDTO
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
                    Name = "name"
                };
                yield return
                    new TestCaseData(new GameDTO
                        {
                            Id = "",
                            Key = "key",
                            Name = "name",
                            Description = "stub-description",
                            GenresName = new List<string>() {"name"},
                            PlatformTypesName = new List<string>() {"naame"}

                        }, new List<Genre> { genre},
                        new List<PlatformType> {new PlatformType()
                        {
                            Id = Guid.NewGuid(),
                            Name = "naame"
                        } 
            } );
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
                        Id=Guid.NewGuid().ToString(),
                        Key = string.Empty,
                        Name = "stub-name",
                        Description = "stub-description"
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = string.Empty,
                        Name = string.Empty,
                        Description = "stub-description"
                    });
                yield return
                    new TestCaseData(new GameDTO { Id = Guid.NewGuid().ToString(), Key = null, Name = "stub-name", Description = "stub-description" });
                yield return
                    new TestCaseData(new GameDTO { Id = Guid.NewGuid().ToString(), Key = "stub-key", Name = null, Description = "stub-description" });
                yield return new TestCaseData(new GameDTO { Id = Guid.NewGuid().ToString(), Key = null, Name = null, Description = "stub-description" });
            }
        }
    }
}