using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.Interfaces;
using GameStore.Web.Providers;
using GameStore.Web.ViewModels.Games;
using GameStore.Web.ViewModels.Genres;
using GameStore.Web.ViewModels.PlatformTypes;
using GameStore.Web.ViewModels.Publishers;
using Moq;
using NUnit.Framework;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.ViewModels.Orders;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTest
    {
        private readonly Mock<IGameService> _mockGame;
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<INamedService<PublisherDTO, PublisherDTOTranslate>> _mockPublisher;
        private readonly Mock<INamedService<GenreDTO, GenreDTOTranslate>> _mockGenre;
        private readonly Mock<INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate>> _mockType;
        private readonly UserModel _user;
        private readonly GamesController _controller;

        public GamesControllerTest()
        {
            _mockGame = new Mock<IGameService>();
            _mockAuth = new Mock<IAuthenticationManager>();
            _mockPublisher = new Mock<INamedService<PublisherDTO, PublisherDTOTranslate>>();
            _mockGenre = new Mock<INamedService<GenreDTO, GenreDTOTranslate>>();
      _orderService= new Mock<IOrderService>();
            _mockType = new Mock<INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate>>();
            AutoMapperConfiguration.Configure();
            _user = new UserModel { Username = "User", IsBanned = false,
                Roles =
                    new List<UserRole>
                    {
                        UserRole.User,
                        UserRole.Administrator,
                        UserRole.Guest,
                        UserRole.Manager,
                        UserRole.Moderator
                    }
            };
            _mockAuth.Setup(x => x.CurrentUser).Returns(new UserProvider(_user));

            _mockGame.Setup(a => a.GetAll(false)).Returns(new List<GameDTO>());
            _mockGenre.Setup(a => a.GetAll(false)).Returns(new List<GenreDTO>());
            _mockPublisher.Setup(a => a.GetAll(false)).Returns(new List<PublisherDTO>());
            _mockType.Setup(a => a.GetAll(false)).Returns(new List<PlatformTypeDTO>());
            _mockGame.Setup(a => a.GetAllByFilter(It.IsAny<FilterDTO>(), false, It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new FilterResultDTO());
            _controller = new GamesController(_mockGame.Object, _mockType.Object, _mockGenre.Object, _orderService.Object, _mockPublisher.Object, _mockAuth.Object);
        }

        [Test]
        public void GetGames_BLLReturnsSomeData()
        {
            // Arrange       
            _mockGame.Setup(a => a.GetAllByFilter(It.IsAny<FilterDTO>(), false, It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new FilterResultDTO());
            // Act
            var res = _controller.Index(new FilterViewModel());

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetGames_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Act
            var res = _controller.Index(new FilterViewModel() {ListGenres = new List<CheckBox>(), ListTypes = new List<CheckBox>(), ListPublishers = new List<CheckBox>()});

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(_controller.ControllerContext));
        }

        [Test]
        public void AddToBasket_BLLChangeData()
        {
            // Arrange
            _orderService.Setup(a => a.GetOrderDetail(It.IsAny<Guid>().ToString(),1, true));

            // Act
            var res = _controller.AddToBasket(new BasketViewModel() { CustomerId = "", GameKey = "key", Quantity = "3", UnitInStock = 1 });

            // Assert
            Assert.AreEqual(res.GetType(), typeof(PartialViewResult));
        }

        [Test]
        public void AddGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _mockGame.Setup(a => a.AddEntity(It.IsAny<GameDTO>())).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();

            // Act
            var res = _controller.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void UpdateGame_GetValidItem_ReturneViewResult()
        {
            _mockGame.Setup(a => a.AddEntity(It.IsAny<GameDTO>()));
            var res = _controller.Update(new CreateGameViewModel() { AllGenres = new List<GenreViewModel>(), AllPublishers = new List<PublisherViewModel>(), AllTypes = new List<PlatformTypeViewModel>(), Id = Guid.NewGuid().ToString(), DateOfAdding = DateTime.UtcNow }, null);

            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
