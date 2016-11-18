using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.Core;
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


        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void CreateGame_ItemSentToDAL_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>())).Verifiable();
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);
            var sut = new GameStoreService(mock.Object);

            // Act
            Assert.Throws<TargetInvocationException>(() => sut.AddOrUpdate(gameDto, true));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void CreateGame_ValidationException_ThrownInvalidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>()));
            mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameStoreService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddOrUpdate(gameDto, true));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidNoParent))]
        public void AddComment_ItemSentToDAL_ValidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new GameStoreService(mock.Object);

            // Act
            sut.AddComment(commentDto, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidWithParent))]
        public void AddComment_ItemSentToDAL_ValidItemWithParrent(CommentDTO commentDto, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<Guid>())).Returns(parentComment);
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new GameStoreService(mock.Object);

            // Act
            sut.AddComment(commentDto, game.Key);

            // Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidNoParrent))]
        public void AddComment_ValidationExceptionThrown_InvalidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            if (commentDto != null && commentDto.GameId == game.Id.ToString())
                mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            else
                mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameStoreService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidWithParrent))]
        public void AddComment_ValidationExceptionThrown_InvalidItemWithParrent(CommentDTO commentDto, Comment parentComment, Game game)
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            if (commentDto != null && commentDto.GameId == game.Id.ToString())
                mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });

            if (!(commentDto != null && commentDto.ParentCommentId == parentComment.Id.ToString()))
                mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<Guid>())).Returns((Comment)null);
            var sut = new GameStoreService(mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderValid))]
        public void ConfirmeOrder_ItemChangeInDAL_ValidItem(OrderDTO orderDto)
        {
            //Arange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>())).Verifiable();
            mock.Setup(a => a.Repository<Order>().GetSingle(It.IsAny<Guid>()))
                .Returns(new Order
                {
                    CustomerId = Guid.Parse(orderDto.Id),
                    Id = Guid.Parse(orderDto.Id),
                    Date = orderDto.Date,
                    IsConfirmed = true
                });
            var sut = new GameStoreService(mock.Object);

            //Act
            sut.ConfirmeOrder(orderDto.Id);

            //Assert
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderInvalid))]
        public void ConfirmeOrder_ItemChangeInDAL_InvalidItem(OrderDTO orderDto)
        {
            //Arange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>())).Verifiable();
            var sut = new GameStoreService(mock.Object);

            //Act && Assert
            Assert.Throws<ValidationException>(() => sut.ConfirmeOrder(orderDto.Id));
        }

        [Test]
        public void GetBasket_ReturnsOrder_ValidItem()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetBusket("");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        public void GetOrderDetail_ReturnsOrder_ValidItem()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetBusket("");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }


        [Test]
        public void GetOrderDetail_ReturnsOrder_InvalidItem()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetOrders("");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }


        [Test]
        public void GetOrders_ReturnsOrder_ValidItem()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetOrders("");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        public void GetGame_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game() });
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetByKey<GameDTO>("stub-key");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGame_ReturnsNull_DALReturnsNothing()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameStoreService(mock.Object);

            // Act & Assert
            Assert.Throws<TargetInvocationException>(() => sut.GetByKey<GameDTO>("stub-key"));
        }

        [Test]
        public void GetCommentsByGameKey_ReturnsComments_DALReturnsValues()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            var gameId = Guid.NewGuid();
            mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>
            {
                new Comment {Id = Guid.NewGuid(), Name = "name", Body = "name", Game = new Game {Id = gameId} },
                new Comment {Id = Guid.NewGuid(), Name="anotherName", Body = "name", Game = new Game {Id = gameId} }
            });
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetCommentsByGameKey_ReturnsNoComments_DALReturnsNothing()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());
            var sut = new GameStoreService(mock.Object);

            // Act
            var res = sut.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(0));
        }
    }
}
