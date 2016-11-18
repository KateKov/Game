using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
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
            AutoMapperConfiguration.Configure();
        }

        //[Test]
        //public void GetGames_BLLReturnsSomeData()
        //{
        //    // Arrange
        //    var mock = new Mock<IService>();
        //    mock.Setup(a => a.GetAll<GameDTO>()).Returns(new List<GameDTO>());
        //    var sut = new GamesController(mock.Object);

        //    // Act
        //    var res = sut.Index();

        //    // Assert
        //    Assert.AreEqual(res.GetType(), typeof(ViewResult));
        //}

        //[Test]
        //public void GetGames_BLLReturnsNothing_ReturnsEmptyJson()
        //{
        //    // Arrange
        //    var mock = new Mock<IService>();
        //    mock.Setup(a => a.GetAll<GameDTO>()).Returns(new List<GameDTO>());
        //    var sut = new GamesController(mock.Object);

        //    // Act
        //    var res = sut.Index();

        //    // Assert
        //    Assert.Throws(NullReferenceException => res.ExecuteResult());
        //}


        [Test]
        public void AddGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a =>  a.AddOrUpdate(It.IsAny<GameDTO>(), true)).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();
            var sut = new GamesController(mock.Object);

            // Act
            var res = sut.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }
    }
}
