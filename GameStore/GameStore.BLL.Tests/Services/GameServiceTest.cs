using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.Web.Infrastracture;
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
            AutoMapperConfiguration.Configure();
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidNoLists))]
        public void CreateGame_ItemSentToDAL_ValidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object);

            // Act
            sut.AddGame(gameDto);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidWithLists))]
        public void CreateGame_ItemSentToDAL_ValidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);
            var sut = new GameService(mock.Object);

            // Act
            sut.AddGame(gameDto);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void CreateGame_ValidationException_ThrownInvalidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>()));
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddGame(gameDto));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void CreateGame_ValidationExceptionThrown_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>()));
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddGame(gameDto));
        }

        [Test]
        public void CreateGame_ValidationExceptionThrown_KeyExists()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddGame(new GameDTO { Key = "stub-key", Name = "stub-name" }));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidNoParent))]
        public void CreateComment_ItemSentToDAL_ValidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new GameService(mock.Object);

            // Act
            sut.AddComment(commentDto, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidWithParent))]
        public void CreateComment_ItemSentToDAL_ValidItemWithParrent(CommentDTO commentDto, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<int>())).Returns(parentComment);
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new GameService(mock.Object);

            // Act
            sut.AddComment(commentDto, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidNoParrent))]
        public void CreateComment_ValidationExceptionThrown_InvalidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            if (commentDto != null && commentDto.GameId == game.Id)
                mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            else
                mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidWithParrent))]
        public void CreateComment_ValidationExceptionThrown_InvalidItemWithParrent(CommentDTO commentDto, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            if (commentDto != null && commentDto.GameId == game.Id)
                mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
           
            if (!(commentDto != null && commentDto.ParentId == parentComment.Id))
                mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<int>())).Returns((Comment)null);
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidNoLists))]
        public void UpdateGame_ItemSentToDAL_ValidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<int>())).Returns(new Game { Key = gameDto.Key, Name = gameDto.Name });
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object);

            // Act
            sut.EditGame(gameDto);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameValidWithLists))]
        public void UpdateGame_ItemSentToDAL_ValidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<int>())).Returns(new Game { Key = gameDto.Key, Name = gameDto.Name });
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);
            var sut = new GameService(mock.Object);

            // Act
            sut.EditGame(gameDto);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void UpdateGame_ValidationExceptionThrown_InvalidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>()));
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.EditGame(gameDto));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void UpdateGame_ValidationExceptionThrown_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>()));
            mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<int>())).Returns(new Game { Key = gameDto.Key, Name = gameDto.Name });
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.EditGame(gameDto));
        }

        [Test]
        public void UpdateGame_ValidationExceptionThrown_KeyExists()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<int>())).Returns(new Game());
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.EditGame(new GameDTO { Key = "another-key-that-exists-in-db", Name = "stub-name" }));
        }

        //[Test]
        //public void DeleteGame_DeletingFromDALCalled_ValidId()
        //{
        //    const int validId = 1;

        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(a => a.Repository<Game>().Delete(It.IsAny<Game>())).Verifiable();
        //    mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<int>())).Returns(new Game());
        //    var sut = new GameService(mock.Object);

        //    // Act
        //    sut.DeleteGame(validId);

        //    // Assert
        //    Mock.Verify(mock);
        //}

        //[Test]
        //public void DeleteGame_ValidationExceptionThrown_InvalidId()
        //{
        //    const int invalidId = 0;

        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(a => a.Repository<Game>().Delete(It.IsAny<Game>())).Verifiable();
        //    mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<int>())).Returns((Game)null);
        //    var sut = new GameService(mock.Object);

        //    // Act & Assert
        //    Assert.Throws<ValidationException>(() => sut.DeleteGame(invalidId));
        //}

        //[Test]
        //public void GetGames_ReturnsListOfGames_DALReturnsValues()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(a => a.Repository<Game>().GetAll()).Returns(new List<Game> { new Game(), new Game() });
        //    var sut = new GameService(mock.Object);

        //    // Act
        //    var res = sut.GetGames();

        //    // Assert
        //    var gameDtos = res as IList<GameDTO> ?? res.ToList();
        //    Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
        //    Assert.That(gameDtos.Count, Is.EqualTo(2));
        //}

        //[Test]
        //public void GetGames_ReturnsEmptyList_DALReturnsNothing()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(a => a.Repository<Game>().GetAll()).Returns(new List<Game>());
        //    var sut = new GameService(mock.Object);

        //    // Act
        //    var res = sut.GetGames();

        //    // Assert
        //    var gameDtos = res as IList<GameDTO> ?? res.ToList();
        //    Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
        //    Assert.That(gameDtos.Count, Is.EqualTo(0));
        //}

        [Test]
        public void GetGamesByGenre_ReturnsListOfGames_DALReturnsValues()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetGamesByGenreId(1);

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetGamesByGenre_ReturnsEmptyList_DALReturnsNothing()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetGamesByGenreId(1);

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetGamesByGenreName_ReturnsListOfGames_DALReturnsValues()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetGamesByGenreName("genre");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetGamesByGenreName_ReturnsEmptyList_DALReturnsNothing()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetGamesByGenreName("genre");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetGamesByPlatform_ReturnsListOfGames_DALReturnsValues()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game(), new Game() });
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetGamesByPlatformTypeName("type");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetGamesByPlatform_ReturnsEmptyList_DALReturnsNothing()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetGamesByPlatformTypeName("type");

            // Assert
            var gameDtos = res as IList<GameDTO> ?? res.ToList();
            Assert.That(gameDtos, Is.TypeOf(typeof(List<GameDTO>)));
            Assert.That(gameDtos.Count, Is.EqualTo(0));
        }

        //[Test]
        //public void GetGame_ReturnsGameDTO_DALReturnsValue()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game() });
        //    var sut = new GameService(mock.Object);

        //    // Act
        //    var res = sut.GetKey("stub-key");

        //    // Assert
        //    Assert.That(res, Is.Not.Null);
        //    Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        //}

        //[Test]
        //public void GetGame_ReturnsNull_DALReturnsNothing()
        //{
        //    // Arrange
        //    var mock = new Mock<IUnitOfWork>();
        //    mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
        //    var sut = new GameService(mock.Object);

        //    // Act & Assert
        //    Assert.Throws<ValidationException>(() => sut.GetGameByKey("stub-key"));
        //}

        [Test]
        public void GetCommentsByGame_ReturnsListOfComments_DALReturnsValues()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>
            {
                new Comment {Id = 1, Name = "stub-name", Body = "stub-name", Game = new Game {Id = 1} },
                new Comment {Id = 2, Name = "stub-name", Body = "stub-name", Game = new Game {Id = 1} }
            });
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetCommentsByGame_ReturnsEmptyList_DALReturnsNothing()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());
            var sut = new GameService(mock.Object);

            // Act
            var res = sut.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(0));
        }
    }
}
