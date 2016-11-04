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
                yield return new TestCaseData(new CommentDTO {Name = "stub-name", Body = "stub-body", GameId = 1},
                    new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
            }
        }

        public static IEnumerable CommentValidWithParent
        {
            get
            {
                yield return new TestCaseData(
                    new CommentDTO
                    {
                        Name = "stub-name",
                        Body = "stub-body",
                        ParentId = 1,
                        ParrentComment = "stub-name",
                        GameId = 1
                    },
                    new Comment
                    {
                        Id = 1,
                        Name = "stub-name",
                        Body = "stub-body",
                        Game = new Game {Id = 1, Key = "stub-key", Name = "stub-name"}
                    },
                    new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
            }
        }

        public static IEnumerable CommentInvalidNoParrent
        {
            get
            {
                yield return
                    new TestCaseData(new CommentDTO {Name = string.Empty, Body = "stub-body", GameId = 1},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new CommentDTO {Name = "stub-name", Body = string.Empty, GameId = 1},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new CommentDTO {Name = string.Empty, Body = string.Empty, GameId = 1},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new CommentDTO {Name = null, Body = null, GameId = 1},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new CommentDTO {Name = null, Body = "stub-body", GameId = 1},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new CommentDTO {Name = "stub-name", Body = null, GameId = 1},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return new TestCaseData(null, new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new CommentDTO {Name = "stub-name", Body = "stub-body", GameId = 0},
                        new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
            }
        }

        public static IEnumerable CommentInvalidWithParrent
        {
            get
            {
                yield return new TestCaseData(
                    new CommentDTO
                    {
                        Name = "stub-name",
                        Body = "stub-body",
                        ParentId = 2,
                        ParrentComment = "stub-name",
                        GameId = 1
                    },
                    new Comment
                    {
                        Id = 1,
                        Name = "stub-name",
                        Body = "stub-body",
                        Game = new Game {Id = 1, Key = "stub-key", Name = "stub-name"}
                    },
                    new Game {Id = 1, Key = "stub-key", Name = "stub-name"});
            }
        }

        public static IEnumerable GameValidNoLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO {Key = "stub-key", Name = "stub-name"});
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Description = "stub-description"
                    });
            }
        }

        public static IEnumerable GameValidWithLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Genres = new List<GenreDTO>
                        {
                            new GenreDTO {Name = "stub-genre-1"},
                            new GenreDTO {Name = "stub-genre-2"}
                        },
                        PlatformTypes = new List<PlatformTypeDTO>
                        {
                            new PlatformTypeDTO {Type = "stub-platform-1"},
                            new PlatformTypeDTO {Type = "stub-platform-2"}
                        }
                    },
                    new List<Genre>
                    {
                        new Genre {Name = "stub-genre-1"},
                        new Genre {Name = "stub-genre-2"}
                    },
                    new List<PlatformType>
                    {
                        new PlatformType {Type = "stub-platform-1"},
                        new PlatformType {Type = "stub-platform-2"}
                    });
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Description = "stub-descr",
                        Genres = new List<GenreDTO>
                        {
                            new GenreDTO {Name = "stub-genre-1"}
                        }
                    },
                    new List<Genre>
                    {
                        new Genre {Name = "stub-genre-1"}
                    },
                    new List<PlatformType>());
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Description = "stub-descr",
                        PlatformTypes = new List<PlatformTypeDTO>
                        {
                            new PlatformTypeDTO {Type = "stub-platform-1"},
                            new PlatformTypeDTO {Type = "stub-platform-2"}
                        }
                    },
                    new List<Genre>(),
                    new List<PlatformType>
                    {
                        new PlatformType {Type = "stub-platform-1"},
                        new PlatformType {Type = "stub-platform-2"}
                    });
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
                        Key = string.Empty,
                        Name = "stub-name",
                        Description = "stub-description"
                    });
                yield return
                    new TestCaseData(new GameDTO
                    {
                        Key = string.Empty,
                        Name = string.Empty,
                        Description = "stub-description"
                    });
                yield return
                    new TestCaseData(new GameDTO {Key = null, Name = "stub-name", Description = "stub-description"});
                yield return
                    new TestCaseData(new GameDTO {Key = "stub-key", Name = null, Description = "stub-description"});
                yield return new TestCaseData(new GameDTO {Key = null, Name = null, Description = "stub-description"});
            }
        }

        public static IEnumerable GameInvalidWithLists
        {
            get
            {
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Genres = new List<GenreDTO>
                        {
                            new GenreDTO {Name = "in_db_1"},
                            new GenreDTO {Name = "not_in_db"}
                        },
                        PlatformTypes = new List<PlatformTypeDTO>
                        {
                            new PlatformTypeDTO {Type = "in_db_1"},
                            new PlatformTypeDTO {Type = "in_db_2"}
                        }
                    },
                    new List<Genre>
                    {
                        new Genre {Name = "in_db_1"}
                    },
                    new List<PlatformType>
                    {
                        new PlatformType {Type = "in_db_1"},
                        new PlatformType {Type = "in_db_2"}
                    });
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Genres = new List<GenreDTO>
                        {
                            new GenreDTO {Name = "in_db_1"},
                            new GenreDTO {Name = "not_in_db"}
                        },
                        PlatformTypes = new List<PlatformTypeDTO>
                        {
                            new PlatformTypeDTO {Type = "not_in_db"}
                        }
                    },
                    new List<Genre>
                    {
                        new Genre {Name = "in_db_1"}
                    },
                    new List<PlatformType>
                    {
                        new PlatformType {Type = "in_db_1"},
                        new PlatformType {Type = "not_in_db"}
                    });
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Genres = new List<GenreDTO>
                        {
                            new GenreDTO {Name = "in_db_1"}
                        },
                        PlatformTypes = new List<PlatformTypeDTO>
                        {
                            new PlatformTypeDTO {Type = "not_in_db"}
                        }
                    },
                    new List<Genre>
                    {
                        new Genre {Name = "in_db_1"}
                    },
                    new List<PlatformType>());
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Genres = new List<GenreDTO>(),
                        PlatformTypes = new List<PlatformTypeDTO>
                        {
                            new PlatformTypeDTO {Type = "not_in_db"}
                        }
                    },
                    new List<Genre>
                    {
                        new Genre {Name = "in_db_1"}
                    },
                    new List<PlatformType>
                    {
                        new PlatformType {Type = "in_db_1"}
                    });
                yield return new TestCaseData(new GameDTO
                    {
                        Key = "stub-key",
                        Name = "stub-name",
                        Genres = new List<GenreDTO>
                        {
                            new GenreDTO {Name = "not_in_db"}
                        },
                        PlatformTypes = new List<PlatformTypeDTO>()
                    },
                    new List<Genre>(),
                    new List<PlatformType>
                    {
                        new PlatformType {Type = "in_db_1"}
                    });
            }
        }
    }
}