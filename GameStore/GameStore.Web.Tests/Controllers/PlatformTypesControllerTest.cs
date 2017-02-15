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

namespace GameStore.Web.Tests.Controllers
{
    public class PlatformTypesControllerTest
    {
        private readonly Mock<INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate>> _mockPlatformType;
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly UserModel _user;
        private readonly PlatformTypesController _controller;

        public PlatformTypesControllerTest()
        {
            AutoMapperConfiguration.Configure();
            _mockPlatformType = new Mock<INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate>>();
            _mockAuth = new Mock<IAuthenticationManager>();
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
            _controller = new PlatformTypesController(_mockPlatformType.Object, _mockAuth.Object);
        }

        [Test]
        public void GetPlatformTypes_BLLReturnsSomeData()
        {
            // Arrange
            _mockPlatformType.Setup(a => a.GetAll(false)).Returns(new List<PlatformTypeDTO>());

            // Act
            var res = _controller.Index();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetPlatformTypes_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _mockPlatformType.Setup(a => a.GetAll(false));

            // Act
            var res = _controller.Index();

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(_controller.ControllerContext));
        }


        [Test]
        public void AddPlatformType_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _mockPlatformType.Setup(a => a.AddEntity(It.IsAny<PlatformTypeDTO>())).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();

            // Act
            var res = _controller.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void DetailPlatformType_GetValidItems_ReturnViewResult()
        {
            //Arrange
            _mockPlatformType.Setup(a => a.GetByName(It.IsAny<string>())).Returns(new PlatformTypeDTO()).Verifiable();

            //Act
            var res = _controller.Details("name");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

    }
}
