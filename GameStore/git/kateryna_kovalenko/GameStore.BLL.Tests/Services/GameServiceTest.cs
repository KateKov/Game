
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidNoLists))]
        public void AddGame_ValidItemNoLists_ItemSentToDAL(GameDTO gameDto)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Add(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            sut.AddGame(gameDto);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidWithLists))]
        public void CreateGame_ValidItemWithLists_ItemSentToDAL(GameDTO gameDTO, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Add(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(platformsFromDb);
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            sut.AddGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void CreateGame_InvalidItemNoLists_ValidationExceptionThrown(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Add(It.IsAny<Game>()));
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddGame(gameDTO));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void CreateGame_InvalidItemWithLists_ValidationExceptionThrown(GameDTO gameDTO, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Add(It.IsAny<Game>()));
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddGame(gameDTO));
        }

        [Test]
        public void CreateGame_KeyExists_ValidationExceptionThrown()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddGame(new GameDTO { Key = "stub-key", Name = "stub-name" }));
        }

       

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidNoParent))]
        public void CreateComment_ValidItemNoParrent_ItemSentToDAL(CommentDTO commentDTO, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.CommentRepository.Add(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            sut.AddComment(commentDTO, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidWithParent))]
        public void AddComment_ValidItemWithParrent_ItemSentToDAL(CommentDTO commentDTO, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.CommentRepository.Add(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.CommentRepository.GetSingle(It.IsAny<int>())).Returns(parentComment);
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            sut.AddComment(commentDTO, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidNoParrent))]
        public void AddComment_InvalidItemNoParrent_ValidationExceptionThrown(CommentDTO commentDTO, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.CommentRepository.Add(It.IsAny<Comment>())).Verifiable();
            if (commentDTO != null && commentDTO.GameId == game.Id)
                mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            else
                mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDTO, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidWithParrent))]
        public void CreateComment_InvalidItemWithParrent_ValidationExceptionThrown(CommentDTO commentDTO, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.CommentRepository.Add(It.IsAny<Comment>())).Verifiable();
            if (commentDTO != null && commentDTO.GameId == game.Id)
                mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            else
                mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            if (commentDTO != null && commentDTO.ParentId == parentComment.Id)
                mock.Setup(a => a.CommentRepository.GetSingle(It.IsAny<int>())).Returns(parentComment);
            else
                mock.Setup(a => a.CommentRepository.GetSingle(It.IsAny<int>())).Returns((Comment)null);
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDTO, game.Key));
        }

       

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidNoLists))]
        public void UpdateGame_ValidItemNoLists_ItemSentToDAL(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Edit(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.GameRepository.GetSingle(It.IsAny<int>())).Returns(new Game { Key = gameDTO.Key, Name = gameDTO.Name });
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            sut.EditGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidWithLists))]
        public void UpdateGame_ValidItemWithLists_ItemSentToDAL(GameDTO gameDTO, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Edit(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.GameRepository.GetSingle(It.IsAny<int>())).Returns(new Game { Key = gameDTO.Key, Name = gameDTO.Name });
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(platformsFromDb);
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            sut.EditGame(gameDTO);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void UpdateGame_InvalidItemNoLists_ValidationExceptionThrown(GameDTO gameDTO)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Edit(It.IsAny<Game>()));
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.EditGame(gameDTO));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void UpdateGame_InvalidItemWithLists_ValidationExceptionThrown(GameDTO gameDTO, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Edit(It.IsAny<Game>()));
            mock.Setup(a => a.GameRepository.GetSingle(It.IsAny<int>())).Returns(new Game { Key = gameDTO.Key, Name = gameDTO.Name });
            mock.Setup(a => a.GenreRepository.GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.PlatformTypeRepository.GetAll()).Returns(platformsFromDb);
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.EditGame(gameDTO));
        }

        [Test]
        public void UpdateGame_KeyExists_ValidationExceptionThrown()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.GetSingle(It.IsAny<int>())).Returns(new Game());
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.EditGame(new GameDTO { Key = "another-key-that-exists-in-db", Name = "stub-name" }));
        }


      
        #region DeleteGame

        [Test]
        public void DeleteGame_ValidId_DeletingFromDALCalled()
        {
            const int validId = 1;

            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.Delete(a.GameRepository.GetSingle((It.IsAny<int>())))).Verifiable();
            mock.Setup(a => a.GameRepository.GetSingle(It.IsAny<int>())).Returns(new Game());
            var sut = new GameService(mock.Object, _loggerMock.Object);

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
            mock.Setup(a => a.GameRepository.Delete(a.GameRepository.GetSingle((It.IsAny<int>())))).Verifiable();
            mock.Setup(a => a.GameRepository.GetSingle(It.IsAny<int>())).Returns((Game)null);
            var sut = new GameService(mock.Object, _loggerMock.Object);

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
            mock.Setup(a => a.GameRepository.GetAll()).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object, _loggerMock.Object);

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
            mock.Setup(a => a.GameRepository.GetAll()).Returns(new List<Game>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

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
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByGenres(1);

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
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByGenres(1);

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
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByPlatformType(1);

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
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGamesByPlatformType(1);

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
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game() });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGameByKey("valid key");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGame_DALReturnsNothing_ReturnsNull()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.GameRepository.FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.GetGameByKey("invalid key"));
        }

        #endregion

        #region GetCommentsByGame

        [Test]
        public void GetCommentsByGame_DALReturnsValues_ReturnsListOfComments()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.CommentRepository.FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>
            {
                new Comment {Id = 1, Name = "stub-name", Body = "stub-name", Game = new Game {Id = 1} },
                new Comment {Id = 2, Name = "stub-name", Body = "stub-name", Game = new Game {Id = 1} }
            });
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetCommentsByGame(-5);

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
            mock.Setup(a => a.CommentRepository.FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());
            var sut = new GameService(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetCommentsByGame(-1);

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(0));
        }

        #endregion
    }
}
