using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.Providers;
using GameStore.Web.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;
using Filter = GameStore.DAL.Enums.Filter;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IGameStoreService> _mock;
        private readonly Mock<IGameService> _mockGame;

        public GamesControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
            _mock = new Mock<IGameStoreService>();
            _mockGame = new Mock<IGameService>();
            AutoMapperConfiguration.Configure();
        }

        [Test]
        public void GetGames_BLLReturnsSomeData()
        {
            // Arrange       
            _mock.Setup(a => a.GenericService<GameDTO>().GetAll()).Returns(new List<GameDTO>());
            _mock.Setup(a => a.GenericService<GenreDTO>().GetAll()).Returns(new List<GenreDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<PublisherDTO>().GetAll()).Returns(new List<PublisherDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<PlatformTypeDTO>().GetAll()).Returns(new List<PlatformTypeDTO>()).Verifiable();
            _mockGame.Setup(a => a.GetAllByFilter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new FilterResultDTO());
            var sut = new GamesController(_mock.Object, _mockGame.Object);
            // Act
            var res = sut.Index(new FilterViewModel());

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetGames_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _mock.Setup(a => a.GenericService<GameDTO>().GetAll()).Returns(new List<GameDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<GenreDTO>().GetAll()).Returns(new List<GenreDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<PublisherDTO>().GetAll()).Returns(new List<PublisherDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<PlatformTypeDTO>().GetAll()).Returns(new List<PlatformTypeDTO>()).Verifiable();
            _mockGame.Setup(a => a.GetAllByFilter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new FilterResultDTO());
            var sut = new GamesController(_mock.Object, _mockGame.Object);

            // Act
            var res = sut.Index(new FilterViewModel() {ListGenres = new List<CheckBox>(), ListTypes = new List<CheckBox>(), ListPublishers = new List<CheckBox>()});

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(sut.ControllerContext));
        }


        [Test]
        public void AddGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _mock.Setup(a => a.GenericService<GameDTO>().AddOrUpdate(It.IsAny<GameDTO>(), true)).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();
            _mock.Setup(a => a.GenericService<GenreDTO>().GetAll()).Returns(new List<GenreDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<PublisherDTO>().GetAll()).Returns(new List<PublisherDTO>()).Verifiable();
            _mock.Setup(a => a.GenericService<PlatformTypeDTO>().GetAll()).Returns(new List<PlatformTypeDTO>()).Verifiable();
            var sut = new GamesController(_mock.Object, _mockGame.Object);

            // Act
            var res = sut.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void UpdateGame_GetValidItem_ReturneViewResult()
        {
            _mock.Setup(a => a.GenericService<GameDTO>().AddOrUpdate(It.IsAny<GameDTO>(), false)).Verifiable();
            var sut = new GamesController(_mock.Object, _mockGame.Object);

            var res = sut.Update(new UpdateGameViewModel() { AllGenres = new List<GenreViewModel>(), AllPublishers = new List<PublisherViewModel>(), AllTypes = new List<PlatformTypeViewModel>(), Id = Guid.NewGuid().ToString(), DateOfAdding = DateTime.UtcNow });

            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
