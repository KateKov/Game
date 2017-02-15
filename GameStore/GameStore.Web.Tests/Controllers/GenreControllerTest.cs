using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Genres;
using Moq;

namespace GameStore.Web.Tests.Controllers
{
    public class GenreControllerTest
    { 
        private readonly Mock<INamedService<GenreDTO, GenreDTOTranslate>> _mockGenre;
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly UserModel _user;
        private readonly GenresController _controller;

        public GenreControllerTest()
        {
            _mockGenre = new Mock<INamedService<GenreDTO, GenreDTOTranslate>>();
            _mockAuth = new Mock<IAuthenticationManager>();
            AutoMapperConfiguration.Configure();
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
            _controller = new GenresController(_mockGenre.Object, _mockAuth.Object);
        }

        [Test]
        public void GetGenres_BLLReturnsSomeData()
        {
            // Arrange       
            _mockGenre.Setup(a => a.GetAll(false)).Returns(new List<GenreDTO>()).Verifiable();
     
            // Act
            var res = _controller.Index();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetGames_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _mockGenre.Setup(a => a.GetAll(true)).Returns(new List<GenreDTO>()).Verifiable();
            // Act
            var res = _controller.Index();

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(_controller.ControllerContext));
        }


        [Test]
        public void AddGenre_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _mockGenre.Setup(a => a.GetAll(false)).Returns(new List<GenreDTO>()).Verifiable();

            // Act
            var res = _controller.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void UpdateGenre_GetValidItem_ReturneViewResult()
        {
            _mockGenre.Setup(a => a.AddEntity(It.IsAny<GenreDTO>())).Verifiable();
            _mockGenre.Setup(a => a.GetAll(true)).Returns(new List<GenreDTO>()).Verifiable();
            _mockGenre.Setup(a => a.GetByName("name"));

            var res = _controller.Update(new CreateGenreViewModel());

            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
