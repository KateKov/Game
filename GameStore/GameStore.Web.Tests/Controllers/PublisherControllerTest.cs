using GameStore.Web.Infrastracture;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.Web.Controllers;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Publishers;

namespace GameStore.Web.Tests.Controllers
{
    public class PublisherControllerTest
    {
        private readonly Mock<INamedService<PublisherDTO, PublisherDTOTranslate>> _mockPublisher;
        private readonly Mock<INamedService<GameDTO, GameDTOTranslate>> _mockGame;
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly UserModel _user;
        private readonly PublishersController _controller;

        public PublisherControllerTest()
        {
            AutoMapperConfiguration.Configure();
            _mockPublisher = new Mock<INamedService<PublisherDTO, PublisherDTOTranslate>>();
            _mockGame = new Mock<INamedService<GameDTO, GameDTOTranslate>>();
            _mockAuth = new Mock<IAuthenticationManager>();
            _user = new UserModel { Username = "User", IsBanned = false,
                Roles =
                    new List<UserRole>
                    {
                        UserRole.Administrator,
                        UserRole.Guest,
                        UserRole.Manager,
                    }
             };
            _mockAuth.Setup(x => x.CurrentUser).Returns(new UserProvider(_user));
            _controller = new PublishersController(_mockPublisher.Object, _mockAuth.Object);
        }

        [Test]
        public void GetPublishers_BLLReturnsSomeData()
        {
            // Arrange
            _mockPublisher.Setup(a => a.GetAll(false)).Returns(new List<PublisherDTO>());

            // Act
            var res = _controller.Index();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetPublishers_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _mockPublisher.Setup(a => a.GetAll(false));

            // Act
            var res = _controller.Index();

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(_controller.ControllerContext));
        }


        [Test]
        public void AddPublisher_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _mockPublisher.Setup(a => a.AddEntity(It.IsAny<PublisherDTO>())).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();

            // Act
            var res = _controller.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void DetailPublisher_GetValidItems_ReturnViewResult()
        {
            //Arrange
            _mockPublisher.Setup(a => a.GetByName(It.IsAny<string>())).Returns(new PublisherDTO()).Verifiable();

            //Act
            var res = _controller.Details("name");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void UpdatePublisher_GetValidItem_ReturneViewResult()
        {
            _mockGame.Setup(a => a.AddEntity(It.IsAny<GameDTO>())).Verifiable();
            _mockPublisher.Setup(a => a.GetAll(true)).Returns(new List<PublisherDTO>()).Verifiable();
            _mockPublisher.Setup(a => a.GetByName("name"));

            var res = _controller.Update(new CreatePublisherViewModel());

            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
