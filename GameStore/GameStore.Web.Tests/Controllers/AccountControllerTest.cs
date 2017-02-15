using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Accounts;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    class AccountControllerTest
    {
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly Mock<IUserService> _userService;
        private readonly UserModel _user;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            AutoMapperConfiguration.Configure();
            _userService = new Mock<IUserService>();
            _mockAuth = new Mock<IAuthenticationManager>();
            _user = new UserModel
            {
                Username = "User",
                IsBanned = false,
                Roles =
                    new List<UserRole>
                    {
                        UserRole.Administrator,
                        UserRole.Guest,
                        UserRole.Manager,
                    }
            };
            _mockAuth.Setup(x => x.CurrentUser).Returns(new UserProvider(_user));
            _controller = new AccountController(_userService.Object, _mockAuth.Object);
        }

       

        [Test]
        public void AddUser_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _userService.Setup(a => a.AddEntity(It.IsAny<UserDTO>())).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();

            // Act
            var res = _controller.Login("url");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void ProfileManager_GetValidItems_ReturnViewResult()
        {
            //Arrange
            _userService.Setup(a => a.GetUserByName(It.IsAny<string>())).Returns(new UserDTO()).Verifiable();

            //Act
            var res = _controller.ManagerProfile();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void UpdateUser_GetValidItem_ReturneViewResult()
        {
            _userService.Setup(a => a.AddEntity(It.IsAny<UserDTO>())).Verifiable();
            _userService.Setup(a => a.GetAll(true)).Returns(new List<UserDTO>()).Verifiable();
            _userService.Setup(a => a.GetUserByName("name"));

            var res = _controller.Register(new RegisterViewModel(), "url");

            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
