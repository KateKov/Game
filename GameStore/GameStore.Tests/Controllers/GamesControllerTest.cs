﻿using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;

        public GamesControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
        }
        
        [Test]
        public void GetGames_BLLReturnsSomeData_ReturnsGamesJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGames()).Returns(new List<GameDTO> { new GameDTO(), new GameDTO() });
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Index();

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(List<GameViewModel>)));
            Assert.That(res.Data as List<GameViewModel>, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetGames_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGames()).Returns(new List<GameDTO>());
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Index();

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Empty);
        }

        [Test]
        public void AddGame_GetsValidItem_ReturnsStatusCodeCreated()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.AddGame(It.IsAny<GameDTO>())).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.New(new GameViewModel
            {
                Name = "valid-game",
                Genres = new List<GenreViewModel> { new GenreViewModel() },
                PlatformTypes = new List<PlatformTypeViewModel> { new PlatformTypeViewModel() }
            });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void AddGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.AddGame(It.IsAny<GameDTO>())).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.New(new GameViewModel { Name = "invalid-game" });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void EditGame_GetsValidItem_ReturnsStatusCodeOk()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.EditGame(It.IsAny<GameDTO>())).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Update(new GameViewModel
            {
                Name = "valid-game",
                Genres = new List<GenreViewModel> { new GenreViewModel() },
                PlatformTypes = new List<PlatformTypeViewModel> { new PlatformTypeViewModel() }
            });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void EditGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.EditGame(It.IsAny<GameDTO>())).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Update(new GameViewModel { Name = "invalid-game" });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void DeleteGame_GetsValidItem_ReturnsStatusCodeOk()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.DeleteGame(It.IsAny<int>())).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Remove(new GameViewModel
            {
                Name = "valid-game",
                Genres = new List<GenreViewModel> { new GenreViewModel() },
                PlatformTypes = new List<PlatformTypeViewModel> { new PlatformTypeViewModel() }
            });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void DeleteGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.DeleteGame(It.IsAny<int>())).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Remove(new GameViewModel { Name = "invalid-game" });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void DownloadGame_GetsInvalidKey_ReturnsNull()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Throws(new ValidationException("", ""));
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Download("invalid-key");

            // Assert
            Assert.That(res, Is.Null);
        }
    }
}
