using System.Collections;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace GameStore.BLL.Tests.Services
{
    public class TestData
    {
        public static IEnumerable CommentValidNoParent
        {
            get
            {
                yield return new TestCaseData(new CommentDTO { Id=1,  Name = "name", Body = "body", GameId = 1 , GameKey = "key"}, new Game { Id = 1, Key = "key", Name = "name" });
            }
        }

        public static IEnumerable CommentValidWithParent
        {
            get
            {
                yield return new TestCaseData(
                    new CommentDTO { Id=2,  Name = "name", Body = "body", ParentId = 1, ParrentComment = "name", GameId = 1 , GameKey = "key"},
                    new Comment { Id = 1, Name = "name", Body = "body", Game = new Game { Id = 1, Key = "key", Name = "name" }, GameId = 1},
                    new Game { Id = 1, Key = "key", Name = "name" });
            }
        }

        public static IEnumerable CommentInvalidNoParrent
        {
            get
            {
                yield return new TestCaseData(new CommentDTO {Id=1, Name = "", Body = "body", GameId = 1 }, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(new CommentDTO {Id=2, Name = "name", Body = "", GameId = 1 }, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(new CommentDTO { Id=3, Name = "", Body = "", GameId = 1 }, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(new CommentDTO { Id=4, Name = null, Body = null, GameId = 1 }, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(new CommentDTO { Id=5, Name = null, Body = "body", GameId = 1 }, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(new CommentDTO { Id =6, Name = "name", Body = null, GameId = 1 }, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(null, new Game { Id = 1, Key = "key", Name = "name" });
                yield return new TestCaseData(new CommentDTO {Id=7, Name = "name", Body = "body", GameId = 0 }, new Game { Id = 1, Key = "key", Name = "name" });
            }
        }

        public static IEnumerable CommentInvalidWithParrent
        {
            get
            {
                yield return new TestCaseData(
                    new CommentDTO { Id=1, Name = "name", Body = "body", ParentId = 2, ParrentComment = "name", GameId = 1 },
                    new Comment { Id = 2, Name = "name", Body = "body", Game = new Game { Id = 1, Key = "key", Name = "name" } },
                    new Game { Id = 1, Key = "key", Name = "name" });
            }
        }
        public static IEnumerable GameValidNoLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO {Id=1, Key = "key", Name = "name" });
              
            }
        }

        public static IEnumerable GameValidWithLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO
                {
                    Id=1,
                    Key = "key",
                    Name = "name",
                    Genres = new List<GenreDTO>
                    {
                        new GenreDTO {Name = "genre1"},
                        new GenreDTO {Name = "genre2"}
                    },
                    PlatformTypes = new List<PlatformTypeDTO>
                    {
                        new PlatformTypeDTO { Id = 1, Type = "platform1"},
                        new PlatformTypeDTO {Id =2, Type = "platform2"}
                    }
                },
                new List<Genre>{
                        new Genre {Id=1, Name = "genre1"},
                        new Genre {Id=2, Name = "genre2"}
                },
                new List<PlatformType>
                {
                    new PlatformType {Id=1, Type = "platform1"},
                    new PlatformType {Id=2, Type = "platform2"}
                });

                yield return new TestCaseData(new GameDTO
                {
                    Id=2,
                    Key = "key",
                    Name = "name",
                    Description = "descr",
                    Genres = new List<GenreDTO>
                    {
                        new GenreDTO {Id=1, Name = "genre1"},
                    }
                },
                new List<Genre>
                {
                    new Genre {Id=1, Name = "genre1"},
                },
                new List<PlatformType>());

                yield return new TestCaseData(new GameDTO
                {
                    Id=3,
                    Key = "key",
                    Name = "name",
                    Description = "descr",
                    PlatformTypes = new List<PlatformTypeDTO>
                    {
                        new PlatformTypeDTO {Id=1, Type = "platform1"},
                        new PlatformTypeDTO {Id=2, Type = "platform2"}
                    }
                },
                new List<Genre>(),
                new List<PlatformType>
                {
                    new PlatformType {Id=1, Type = "platform1"},
                    new PlatformType {Id=2, Type = "platform2"}
                });
            }
        }

        public static IEnumerable GameInvalidNoLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO());
                yield return new TestCaseData(null);
                yield return new TestCaseData(new GameDTO { Id=1, Key = "", Name = "name", Description = "stub-description" });
                yield return new TestCaseData(new GameDTO { Id=-2, Key = "", Name = "", Description = "stub-description" });
                yield return new TestCaseData(new GameDTO { Id=3, Key = null, Name = "stub-name", Description = "stub-description" });
                yield return new TestCaseData(new GameDTO { Id=4, Key = "stub-key", Name = null, Description = "stub-description" });
                yield return new TestCaseData(new GameDTO { Id=0, Key = null, Name = null, Description = "stub-description" });
            }
        }

        public static IEnumerable GameInvalidWithLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO
                {
                    Id=1,
                    Key = "key",
                    Name = "name",
                    Genres = new List<GenreDTO>
                    {
                        new GenreDTO {Id=1, Name = "in_db_1"},
                        new GenreDTO {Id=2, Name = "not_in_db"}
                    },
                    PlatformTypes = new List<PlatformTypeDTO>
                    {
                        new PlatformTypeDTO {Id=1,  Type = "in_db_1"},
                        new PlatformTypeDTO {Id=2, Type = "in_db_2"}
                    }
                },
                new List<Genre>{
                    new Genre {Id =1, Name = "in_db_1"},
                },
                new List<PlatformType>
                {
                    new PlatformType {Id=1, Type = "in_db_1"},
                    new PlatformType {Id=2, Type = "in_db_2"}
                });

                yield return new TestCaseData(new GameDTO
                {
                    Id=2,
                    Key = "key",
                    Name = "name",
                    Genres = new List<GenreDTO>
                    {
                        new GenreDTO {Id=1, Name = "in_db_1"},
                        new GenreDTO {Id=2, Name = "not_in_db"}
                    },
                    PlatformTypes = new List<PlatformTypeDTO>
                    {
                        new PlatformTypeDTO {Id=2, Type = "not_in_db"}
                    }
                },
                 new List<Genre>{
                    new Genre {Id=1, Name = "in_db_1"},
                 },
                 new List<PlatformType>
                 {
                    new PlatformType {Id=1, Type = "in_db_1"},
                    new PlatformType {Id=2, Type = "not_in_db"}
                 });

                yield return new TestCaseData(new GameDTO
                {
                    Id=3,
                    Key = "key",
                    Name = "name",
                    Genres = new List<GenreDTO>
                    {
                        new GenreDTO {Id=1, Name = "in_db_1"},
                    },
                    PlatformTypes = new List<PlatformTypeDTO>
                    {
                        new PlatformTypeDTO {Id=2, Type = "not_in_db"}
                    }
                },
                new List<Genre>{
                    new Genre {Id=1, Name = "in_db_1"},
                },
                new List<PlatformType>());

                yield return new TestCaseData(new GameDTO
                {
                    Id=4,
                    Key = "key",
                    Name = "name",
                    Genres = new List<GenreDTO>(),
                    PlatformTypes = new List<PlatformTypeDTO>
                    {
                        new PlatformTypeDTO {Id=2, Type = "not_in_db"}
                    }
                },
                new List<Genre>{
                    new Genre {Id=1, Name = "in_db_1"},
                },
                new List<PlatformType>
                {
                    new PlatformType {Id=1, Type = "in_db_1"},
                });

                yield return new TestCaseData(new GameDTO
                {
                    Id=5,
                    Key = "key",
                    Name = "name",
                    Genres = new List<GenreDTO>
                    {
                        new GenreDTO {Id=2, Name = "not_in_db"},
                    },
                    PlatformTypes = new List<PlatformTypeDTO>()
                },
                new List<Genre>(),
                new List<PlatformType>
                {
                    new PlatformType {Id=1, Type = "in_db_1"},
                });
            }
        }
    
    }
}
