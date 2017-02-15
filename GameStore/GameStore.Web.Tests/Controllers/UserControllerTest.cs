using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Users;
using Moq;

namespace GameStore.Web.Tests.Controllers
{
    class UserControllerTest
    {
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<INamedService<RoleDTO, RoleDTOTranslate>> _roleService;
        private readonly UserModel _user;
        private readonly UsersController _controller;

        public UserControllerTest()
        {
            AutoMapperConfiguration.Configure();
           _userService = new Mock<IUserService>();
            _mockAuth = new Mock<IAuthenticationManager>();
            _roleService = new Mock<INamedService<RoleDTO, RoleDTOTranslate>>();
            _gameService = new Mock<IGameService>();
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
            _controller = new UsersController(_gameService.Object, _userService.Object, _roleService.Object, _mockAuth.Object);
        }

        [Test]
        public void GetUsers_BLLReturnsSomeData()
        {
            // Arrange
           _userService.Setup(a => a.GetAll(false)).Returns(new List<UserDTO>());

            // Act
            var res = _controller.Index();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetUsers_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _userService.Setup(a => a.GetAll(false));

            // Act
            var res = _controller.Index();

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(_controller.ControllerContext));
        }


        [Test]
        public void AddUser_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _userService.Setup(a => a.AddEntity(It.IsAny<UserDTO>())).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();

            // Act
            var res = _controller.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void DetailUser_GetValidItems_ReturnViewResult()
        {
            //Arrange
            _userService.Setup(a => a.GetUserByName(It.IsAny<string>())).Returns(new UserDTO()).Verifiable();

            //Act
            var res = _controller.Details("name");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void UpdateUser_GetValidItem_ReturneViewResult()
        {
            _userService.Setup(a => a.AddEntity(It.IsAny<UserDTO>())).Verifiable();
            _userService.Setup(a => a.GetAll(true)).Returns(new List<UserDTO>()).Verifiable();
            _userService.Setup(a => a.GetUserByName("name"));

            var res = _controller.Update(new CreateUserViewModel());

            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}

