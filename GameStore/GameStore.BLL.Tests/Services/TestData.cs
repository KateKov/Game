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
                var game = new Game {EntityId = Guid.NewGuid(), Key = "key", Translates = new List<GameTranslate>() {new GameTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = "name"} } };
                yield return new TestCaseData(new CommentDTO { Name = "name", Body = "body", GameId = game.EntityId.ToString(), GameKey = game.Key},
                    game);
            }
        }

        public static IEnumerable CommentValidWithParent
        {
            get
            {
                var game = new Game {EntityId = Guid.NewGuid(), Key = "key", Translates = new List<GameTranslate>() { new GameTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = "name" } } };
                var parrentComment = new Comment
                {
                    EntityId = Guid.NewGuid(),
                    Name = "name",
                    Body = "body",
                    Game = game
                };
                yield return new TestCaseData(
                    new CommentDTO
                    {
                        Name = "stub-name",
                        Body = "stub-body",
                        ParentCommentId = parrentComment.EntityId.ToString(),
                        ParrentCommentName = "name",
                        GameId = game.EntityId.ToString(),
                        GameKey = game.Key
                    },
                    parrentComment, game);
            }
        }

        public static IEnumerable CommentInvalidNoParrent
        {
            get
            {
                var game = new Game {EntityId = Guid.NewGuid(), Key = "stub-key", Translates = new List<GameTranslate>() { new GameTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = "name" } } };
                yield return
                    new TestCaseData(new CommentDTO { Name = string.Empty, Body = "stub-body", GameId = game.EntityId.ToString(), GameKey = game.Key},
                       game);
                yield return
                    new TestCaseData(new CommentDTO { Name = "stub-name", Body = string.Empty, GameId = game.EntityId.ToString(), GameKey = game.Key },
                        game);
                yield return
                    new TestCaseData(new CommentDTO { Name = string.Empty, Body = string.Empty, GameId = game.EntityId.ToString(), GameKey = game.Key },
                       game);
                yield return
                    new TestCaseData(new CommentDTO { Name = null, Body = null, GameId = game.EntityId.ToString(), GameKey = game.Key },
                        game);
                yield return
                new TestCaseData(new CommentDTO { Name = null, Body = "stub-body", GameId = game.EntityId.ToString(), GameKey = game.Key },
                    game);
                yield return
                new TestCaseData(new CommentDTO { Name = "stub-name", Body = null, GameId = game.EntityId.ToString(), GameKey = game.Key },
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
                var game = new Game {EntityId = Guid.NewGuid(), Key = "key", Translates = new List<GameTranslate>() { new GameTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = "name" } } };
                var parrentComment = new Comment
                {
                    EntityId = Guid.NewGuid(),
                    Name = "name",
                    Body = "body",
                    Game = game
                };
                yield return new TestCaseData(
                    new CommentDTO
                    {
                        Name = "stub-name",
                        Body = "stub-body",
                        ParentCommentId = parrentComment.EntityId.ToString(),
                        ParrentCommentName = "name",
                        GameId = game.EntityId.ToString()
                    },
                   parrentComment,game);
            }
        }

        public static IEnumerable GenreValidNoLists
        {
            get
            {
                yield return new TestCaseData(new GenreDTO { Id=Guid.NewGuid().ToString(), Translates = new List<GenreDTOTranslate>() { new GenreDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = "name" } } });
                yield return
                    new TestCaseData(new GenreDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Translates = new List<GenreDTOTranslate>() { new GenreDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = "name" } }
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
                    Translates = new List<GenreTranslate>() { new GenreTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = "name" } }
                };
                yield return
                    new TestCaseData(new GameDTO
                        {
                            Id = "",
                            Key = "key",
                            Translates = new List<GameDTOTranslate>() { new GameDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = "name" ,  Description = "stub-description",
                            GenresName = new List<string>() {"name"},
                            PlatformTypesName = new List<string>() {"naame"}} }
                        }, new List<Genre> { genre},
                        new List<PlatformType> {new PlatformType()
                        {
                            EntityId = Guid.NewGuid(),
                           Translates = new List<PlatformTypeTranslate>() { new PlatformTypeTranslate() { EntityId = Guid.NewGuid(), Language = Language.En, Name = "naame" } }
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
                        Translates = new List<GameDTOTranslate>() { new GameDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = "name", Description = "description"} }
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Id = Guid.NewGuid().ToString(),
                        Key = string.Empty,
                        Translates = new List<GameDTOTranslate>() { new GameDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = string.Empty, Description = "description" } }
                    });
                yield return
                    new TestCaseData(new GameDTO { Id = Guid.NewGuid().ToString(), Key = null, Translates = new List<GameDTOTranslate>() { new GameDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = "name", Description = "description" } }});
                yield return
                    new TestCaseData(new GameDTO { Id = Guid.NewGuid().ToString(), Key = "stub-key", Translates = new List<GameDTOTranslate>() { new GameDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = null, Description = "description" } }});
                yield return new TestCaseData(new GameDTO { Id = Guid.NewGuid().ToString(), Key = null, Translates = new List<GameDTOTranslate>() { new GameDTOTranslate() { Id = Guid.NewGuid().ToString(), Language = Language.En, Name = null, Description = "description" } }});
            }
        }
    }
}