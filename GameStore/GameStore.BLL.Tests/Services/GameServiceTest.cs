
using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class StoreServiceTest
    {
        private readonly Mock<ILogger> _loggerMock;

        public StoreServiceTest()
        {
            _loggerMock = new Mock<ILogger>();
        }

        #region CreateGame

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameValidNoLists))]
        public void CreateGame_ValidItemNoLists_ItemSentToDAL(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Create(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Genres.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Platforms.GetAll()).Returns(new List<Platform>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.CreateGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameValidWithLists))]
        public void CreateGame_ValidItemWithLists_ItemSentToDAL(GameDTO gameDTO, List<Genre> genresFromDb, List<Platform> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Create(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Genres.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Platforms.GetAll()).Returns(platformsFromDb);
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.CreateGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameInvalidNoLists))]
        public void CreateGame_InvalidItemNoLists_ValidationExceptionThrown(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Create(It.IsAny<Game>()));
            mock.Setup(a => a.Genres.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Platforms.GetAll()).Returns(new List<Platform>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.CreateGame(gameDTO));
        }

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameInvalidWithLists))]
        public void CreateGame_InvalidItemWithLists_ValidationExceptionThrown(GameDTO gameDTO, List<Genre> genresFromDb, List<Platform> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Create(It.IsAny<Game>()));
            mock.Setup(a => a.Genres.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Platforms.GetAll()).Returns(platformsFromDb);
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.CreateGame(gameDTO));
        }

        [Test]
        public void CreateGame_KeyExists_ValidationExceptionThrown()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.CreateGame(new GameDTO { Key = "stub-key", Name = "stub-name" }));
        }

        #endregion

        #region CreateComment

        [Test, TestCaseSource(typeof(CommentDataClass), nameof(CommentDataClass.CommentValidNoParent))]
        public void CreateComment_ValidItemNoParrent_ItemSentToDAL(CommentDTO commentDTO, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Comments.Create(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { game });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.CreateComment(commentDTO, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(CommentDataClass), nameof(CommentDataClass.CommentValidWithParent))]
        public void CreateComment_ValidItemWithParrent_ItemSentToDAL(CommentDTO commentDTO, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Comments.Create(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.Comments.Get(It.IsAny<int>())).Returns(parentComment);
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { game });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.CreateComment(commentDTO, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(CommentDataClass), nameof(CommentDataClass.CommentInvalidNoParrent))]
        public void CreateComment_InvalidItemNoParrent_ValidationExceptionThrown(CommentDTO commentDTO, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Comments.Create(It.IsAny<Comment>())).Verifiable();
            if (commentDTO != null && commentDTO.GameId == game.Id)
                mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { game });
            else
                mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.CreateComment(commentDTO, game.Key));
        }

        [Test, TestCaseSource(typeof(CommentDataClass), nameof(CommentDataClass.CommentInvalidWithParrent))]
        public void CreateComment_InvalidItemWithParrent_ValidationExceptionThrown(CommentDTO commentDTO, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Comments.Create(It.IsAny<Comment>())).Verifiable();
            if (commentDTO != null && commentDTO.GameId == game.Id)
                mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { game });
            else
                mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());
            if (commentDTO != null && commentDTO.ParentCommentId == parentComment.Id)
                mock.Setup(a => a.Comments.Get(It.IsAny<int>())).Returns(parentComment);
            else
                mock.Setup(a => a.Comments.Get(It.IsAny<int>())).Returns((Comment)null);
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.CreateComment(commentDTO, game.Key));
        }

        #endregion

        #region UpdateGame

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameValidNoLists))]
        public void UpdateGame_ValidItemNoLists_ItemSentToDAL(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Update(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Games.Get(It.IsAny<int>())).Returns(new Game { Key = gameDTO.Key, Name = gameDTO.Name });
            mock.Setup(a => a.Genres.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Platforms.GetAll()).Returns(new List<Platform>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.UpdateGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameValidWithLists))]
        public void UpdateGame_ValidItemWithLists_ItemSentToDAL(GameDTO gameDTO, List<Genre> genresFromDb, List<Platform> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Update(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Games.Get(It.IsAny<int>())).Returns(new Game { Key = gameDTO.Key, Name = gameDTO.Name });
            mock.Setup(a => a.Genres.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Platforms.GetAll()).Returns(platformsFromDb);
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.UpdateGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameInvalidNoLists))]
        public void UpdateGame_InvalidItemNoLists_ValidationExceptionThrown(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Update(It.IsAny<Game>()));
            mock.Setup(a => a.Genres.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Platforms.GetAll()).Returns(new List<Platform>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.UpdateGame(gameDTO));
        }

        [Test, TestCaseSource(typeof(GameDataClass), nameof(GameDataClass.GameInvalidWithLists))]
        public void UpdateGame_InvalidItemWithLists_ValidationExceptionThrown(GameDTO gameDTO, List<Genre> genresFromDb, List<Platform> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Update(It.IsAny<Game>()));
            mock.Setup(a => a.Games.Get(It.IsAny<int>())).Returns(new Game { Key = gameDTO.Key, Name = gameDTO.Name });
            mock.Setup(a => a.Genres.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Platforms.GetAll()).Returns(platformsFromDb);
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.UpdateGame(gameDTO));
        }

        [Test]
        public void UpdateGame_KeyExists_ValidationExceptionThrown()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Get(It.IsAny<int>())).Returns(new Game());
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.UpdateGame(new GameDTO { Key = "another-key-that-exists-in-db", Name = "stub-name" }));
        }


        #endregion

        #region DeleteGame

        [Test]
        public void DeleteGame_ValidId_DeletingFromDALCalled()
        {
            const int validId = 1;

            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Delete(It.IsAny<int>())).Verifiable();
            mock.Setup(a => a.Games.Get(It.IsAny<int>())).Returns(new Game());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            sut.DeleteGame(validId);

            // Assert
            Mock.Verify(mock);
        }

        [Test]
        public void DeleteGame_InvalidId_ValidationExceptionThrown()
        {
            const int invalidId = 0;

            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Delete(It.IsAny<int>())).Verifiable();
            mock.Setup(a => a.Games.Get(It.IsAny<int>())).Returns((Game)null);
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.DeleteGame(invalidId));
        }

        #endregion

        #region GetGames

        [Test]
        public void GetGames_DALReturnsValues_ReturnsListOfGames()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.GetAll()).Returns(new List<Game> { new Game(), new Game() });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGames();

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetGames_DALReturnsNothing_ReturnsEmptyList()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.GetAll()).Returns(new List<Game>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGames();

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(0));
        }

        #endregion

        #region GetGamesByGenre

        [Test]
        public void GetGamesByGenre_DALReturnsValues_ReturnsListOfGames()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByGenre("valid genre");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetGamesByGenre_DALReturnsNothing_ReturnsEmptyList()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByGenre("invalid genre");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(0));
        }

        #endregion

        #region GetGamesByPlatform

        [Test]
        public void GetGamesByPlatform_DALReturnsValues_ReturnsListOfGames()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByPlatform("valid platform");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetGamesByPlatform_DALReturnsNothing_ReturnsEmptyList()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByPlatform("invalid platform");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(0));
        }

        #endregion

        #region GetGame

        [Test]
        public void GetGame_DALReturnsValue_ReturnsGameDTO()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game> { new Game() });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGame("valid key");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGame_DALReturnsNothing_ReturnsNull()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Games.Find(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.GetGame("invalid key"));
        }

        #endregion

        #region GetCommentsByGame

        [Test]
        public void GetCommentsByGame_DALReturnsValues_ReturnsListOfComments()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Comments.Find(It.IsAny<Func<Comment, bool>>())).Returns(new List<Comment>
            {
                new Comment {Id = 1, Name = "stub-name", Body = "stub-name", Game = new Game {Id = 1} },
                new Comment {Id = 2, Name = "stub-name", Body = "stub-name", Game = new Game {Id = 1} }
            });
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetCommentsByGame("valid game key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetCommentsByGame_DALReturnsNothing_ReturnsEmptyList()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Comments.Find(It.IsAny<Func<Comment, bool>>())).Returns(new List<Comment>());
            var sut = new StoreService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetCommentsByGame("invalid game key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(0));
        }

        #endregion
    }
}
