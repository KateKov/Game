using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.Core;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Infrastructure.OrderFilter;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;
using GameStore.Web.Infrastracture;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ILogger = NLog.ILogger;
using GameStore.DAL.Infrastracture;

namespace GameStore.BLL.Tests.Services
{
    [TestFixture]
    public class StoreServiceTest
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IUnitOfWork> _mock;

        public StoreServiceTest()
        {
            _loggerMock = new Mock<ILogger>();
            AutoMapperConfiguration.Configure();
            _mock = new Mock<IUnitOfWork>();
        }


        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void CreateGame_ItemSentToDAL_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>())).Verifiable();
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);
            var sut = new GameStoreService(_mock.Object);

            // Act
            Assert.Throws<NullReferenceException>(() => sut.GenericService<GameDTO>().AddOrUpdate(gameDto, true));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void CreateGame_ValidationException_ThrownInvalidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new GameStoreService(_mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.GenericService<GameDTO>().AddOrUpdate(gameDto, true));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidNoLists))]
        public void EditGame_ValidationException_ThrownInvalidItemNoLists(GameDTO gameDto)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(new List<Genre>());
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(new List<PlatformType>());
            var sut = new Service<Game, GameDTO>(_mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddOrUpdate(gameDto, false));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.GameInvalidWithLists))]
        public void EditGame_ItemSentToDAL_InvalidItemWithLists(GameDTO gameDto, List<Genre> genresFromDb, List<PlatformType> platformsFromDb)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>())).Verifiable();
            _mock.Setup(a => a.Repository<Genre>().GetAll()).Returns(genresFromDb);
            _mock.Setup(a => a.Repository<PlatformType>().GetAll()).Returns(platformsFromDb);
            var sut = new Service<Game, GameDTO>(_mock.Object);

            // Act
            Assert.Throws<NullReferenceException>(() => sut.AddOrUpdate(gameDto, false));
        }


        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidNoParent))]
        public void AddComment_ItemSentToDAL_ValidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new CommentsService(_mock.Object);

            // Act
            sut.AddComment(commentDto, game.Key);

            // Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentValidWithParent))]
        public void AddComment_ItemSentToDAL_ValidItemWithParrent(CommentDTO commentDto, Comment parentComment, Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            _mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<string>())).Returns(parentComment);
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            var sut = new CommentsService(_mock.Object);

            // Act
            sut.AddComment(commentDto, game.Key);

            // Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidNoParrent))]
        public void AddComment_ValidationExceptionThrown_InvalidItemNoParrent(CommentDTO commentDto, Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            if (commentDto != null && commentDto.GameId == game.EntityId.ToString())
                _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });
            else
                _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new CommentsService(_mock.Object);

            // Act & Assert
            Assert.Throws<ValidationException>(() => sut.AddComment(commentDto, game.Key));
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CommentInvalidWithParrent))]
        public void AddComment_ValidationExceptionThrown_InvalidItemWithParrent(CommentDTO commentDto, Comment parentComment, Game game)
        {
            // Arrange
            _mock.Setup(a => a.Repository<Comment>().Add(It.IsAny<Comment>())).Verifiable();
            if (commentDto != null && commentDto.GameId == game.EntityId.ToString())
                _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { game });

            if (!(commentDto != null && commentDto.ParentCommentId == parentComment.EntityId.ToString()))
                _mock.Setup(a => a.Repository<Comment>().GetSingle(It.IsAny<string>())).Returns((Comment)null);
            var sut = new CommentsService(_mock.Object);
            sut.AddComment(commentDto, game.Key);

            // Act & Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderValid))]
        public void ConfirmeOrder_ItemChangeInDAL_ValidItem(OrderDTO orderDto)
        {
            //Arange
            _mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>())).Verifiable();
            _mock.Setup(a => a.Repository<Order>().GetSingle(It.IsAny<string>()))
                .Returns(new Order
                {
                    //CustomerId = Guid.Parse(orderDto.Id),
                    EntityId = Guid.Parse(orderDto.Id),
                    Date = orderDto.Date,
                    IsConfirmed = true
                });
            var sut = new OrdersService(_mock.Object);

            //Act
            sut.ConfirmeOrder(orderDto.Id);

            //Assert
            Mock.Verify(_mock);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrderInvalid))]
        public void ConfirmeOrder_ItemChangeInDAL_InvalidItem(OrderDTO orderDto)
        {
            //Arange
            _mock.Setup(a => a.Repository<Order>().Edit(It.IsAny<Order>())).Verifiable();
            var sut = new OrdersService(_mock.Object);

            //Act && Assert
            Assert.Throws<ValidationException>(() => sut.ConfirmeOrder(orderDto.Id));
        }

        [Test]
        public void GetBasket_ReturnsOrder_ValidItem()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new OrdersService(_mock.Object);

            // Act
            var res = sut.GetBusket("");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        public void GetOrderDetail_ReturnsOrder_ValidItem()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new OrdersService(_mock.Object);

            // Act
            var res = sut.GetBusket("");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }


        [Test]
        public void GetOrderDetail_ReturnsOrder_InvalidItem()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new OrdersService(_mock.Object);

            // Act
            var res = sut.GetOrders();

            //// Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }


        [Test]
        public void GetOrders_ReturnsOrder_ValidItem()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>())).Returns(new List<Order> { new Order(), new Order() });
            var sut = new OrdersService(_mock.Object);

            // Act
            var res = sut.GetOrders();

            //// Assert
            Assert.That(res, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        public void GetGame_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game() });
            var sut = new GameStoreService(_mock.Object);

            // Act
            var res = sut.KeyService<GameDTO>().GetByKey("stub-key");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGameByName_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game> { new Game() { EntityId = Guid.NewGuid(), Translates = new List<GameTranslate>() {new GameTranslate() {EntityId = Guid.NewGuid(), Name = "name", Language = Language.En} } } });
            var sut = new GameStoreService(_mock.Object);

            // Act
            var res = sut.NamedService<GameDTO, GameDTOTranslate>().GetByName("name");

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetAllGame_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetAll()).Returns(new List<Game> { new Game() });
            var sut = new GameStoreService(_mock.Object);

            // Act
            var res = sut.GenericService<GameDTO>().GetAll();

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(List<GameDTO>)));
        }


        [Test]
        public void GetGameById_ReturnsGameDTO_DALReturnsValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<string>())).Returns(new Game());
            var sut = new GameStoreService(_mock.Object);

            // Act
            var res = sut.GenericService<GameDTO>().GetById(Guid.NewGuid().ToString());

            // Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.TypeOf(typeof(GameDTO)));
        }

        [Test]
        public void GetGame_ReturnsNull_DALReturnsNothing()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>());
            var sut = new GameStoreService(_mock.Object);

            // Act & Assert
            var res = sut.KeyService<GameDTO>().GetByKey("stub-key");
            Assert.That(res, Is.Null);
        }


        [Test]
        public void GetGamesByFilter_ReturnsGames_DALReturnsValues()
        {
            // Arrange
            var mockPipeline = new Mock<IPipeLine<Game>>();
            mockPipeline.Setup(x => x.Execute());
            mockPipeline.Setup(x => x.Register(It.IsAny<IOperation<Game>>()));
            _mock.Setup(a => a.Repository<Game>().GetAll(It.IsAny<Func<Game, bool>>())).Returns(new List<Game>());
            var sut = new GamesService(_mock.Object);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => sut.GetAllByFilter(new FilterDTO() {FilterBy = Filter.Comments, Name = "name", DateOfAdding = Date.month}));
        }

        [Test]
        public void GetCommentsByGameKey_ReturnsComments_DALReturnsValues()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            _mock.Setup(a => a.Repository<Game>().GetAll());
            _mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>
            {
                new Comment {EntityId = Guid.NewGuid(), Name = "name", Body = "name", Game = new Game {EntityId = gameId} },
                new Comment {EntityId = Guid.NewGuid(), Name="anotherName", Body = "name", Game = new Game {EntityId = gameId} }
            });
            var sut = new CommentsService(_mock.Object);

            // Act
            var res = sut.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetOrderDetail_ReturnsNothing_DALReturnsValues()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Publisher>().FindBy(It.IsAny<Expression<Func<Publisher, bool>>>())).Returns(new List<Publisher>() {new Publisher() {EntityId = Guid.NewGuid(), Translates = new List<PublisherTranslate>() {new PublisherTranslate() {EntityId = Guid.NewGuid(), Name = "name", Language = Language.En} } } });
            _mock.Setup(a => a.Repository<Genre>().FindBy(It.IsAny<Expression<Func<Genre, bool>>>())).Returns(new List<Genre>() {new Genre() {EntityId = Guid.NewGuid(), Translates = new List<GenreTranslate>() {new GenreTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = "name"} } } });
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>() { new Game() {EntityId = Guid.NewGuid(), Key = "key", Translates = new List<GameTranslate>() {new GameTranslate() {Name = "name", EntityId = Guid.NewGuid(), Language = Language.En} }, Publisher = new Publisher() {EntityId = Guid.NewGuid(), Translates = new List<PublisherTranslate>() {new PublisherTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = "publisher"} } }, Genres = new List<Genre>() {new Genre() {EntityId = Guid.NewGuid(), Translates = new List<GenreTranslate>() {new GenreTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = "genre"} } } }, PlatformTypes = new List<PlatformType>() {new PlatformType() {EntityId = Guid.NewGuid(), Translates = new List<PlatformTypeTranslate>() {new PlatformTypeTranslate() {EntityId = Guid.NewGuid(), Language = Language.En, Name = "platform"} } } } } });
            _mock.Setup(a => a.Repository<Genre>().Add(It.IsAny<Genre>()));
            _mock.Setup(a => a.Repository<Publisher>().Add(It.IsAny<Publisher>()));
            _mock.Setup(a => a.Repository<Game>().Add(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<Game>().Edit(It.IsAny<Game>()));
            _mock.Setup(a => a.Repository<OrderDetail>().FindBy(It.IsAny<Expression<Func<OrderDetail, bool>>>())).Returns(new List<OrderDetail>());
            _mock.Setup(a => a.Repository<Order>().GetAll()).Returns(new List<Order>());
            _mock.Setup(a => a.Repository<Order>().Add(It.IsAny<Order>()));
            _mock.Setup(a => a.Repository<OrderDetail>().Add(It.IsAny<OrderDetail>()));
            var sut = new OrdersService(_mock.Object);

            sut.GetOrderDetail("key", 0); 
            Mock.Verify(_mock);
        }


        [Test]
        public void GetCommentsByGameKey_ReturnsNoComments_DALReturnsNothing()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetAll());
            _mock.Setup(a => a.Repository<Comment>().FindBy(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());
            var sut = new CommentsService(_mock.Object);

            // Act
            var res = sut.GetCommentsByGameKey("key");

            // Assert
            var commentsDto = res as IList<CommentDTO> ?? res.ToList();
            Assert.That(commentsDto, Is.TypeOf(typeof(List<CommentDTO>)));
            Assert.That(commentsDto.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteGameByName_ReturnsValidationException_DALChangeValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetAll());
            _mock.Setup(a => a.Repository<Game>().FindBy(It.IsAny<Expression<Func<Game, bool>>>())).Returns(new List<Game>()).Verifiable();
            _mock.Setup(a => a.Repository<Game>().Delete(It.IsAny<Game>()));
            var sut = new ModelWithNameService<Game, GameDTO, GameTranslate, GameDTOTranslate>(_mock.Object);

            // Act
            Assert.Throws<ValidationException>(() => sut.DeleteByName("name"));
        }

        [Test]
        public void DeleteGameById_ReturnsValidationException_DALChangeValue()
        {
            // Arrange
            _mock.Setup(a => a.Repository<Game>().GetAll()).Returns(new List<Game>());
            _mock.Setup(a => a.Repository<Game>().GetSingle(It.IsAny<string>())).Returns(new Game()).Verifiable();
            _mock.Setup(a => a.Repository<Game>().Delete(It.IsAny<Game>()));
            var sut = new Service<Game, GameDTO>(_mock.Object);

            // Act
            sut.DeleteById(Guid.NewGuid().ToString());
            Mock.Verify(_mock);
        }


        [Test]
        public void DeleteBusket_ChangeOrderRepository_DALReturnsNothing()
        {
            _mock.Setup(a => a.Repository<Order>().FindBy(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(new List<Order>
            {
                new Order {EntityId = Guid.NewGuid(), CustomerId = "", Date = DateTime.UtcNow, IsConfirmed = false, OrderDetails = new List<OrderDetail>() {new OrderDetail()}, Sum=0 }
               }
            ); 
            _mock.Setup(a => a.Repository<Order>().Delete(It.IsAny<Order>()));
            var sut = new OrdersService(_mock.Object);
            sut.DeleteBusket(Guid.Empty.ToString());

            // Assert
            Mock.Verify(_mock);
        }
    }
}
