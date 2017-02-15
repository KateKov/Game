using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class CommentsControllerTest
    {
        private Mock<IGameService> _gameMock;
        private Mock<ICommentService> _mockComment;
        private Mock<IUserService> _mockUser;
        private Mock<IAuthenticationManager> _mockAuth;
        private CommentsController _controller;
        private UserModel _user;

        [SetUp]
        public void SetUp()
        {
            _gameMock = new Mock<IGameService>();
            AutoMapperConfiguration.Configure();
            _mockComment = new Mock<ICommentService>();
            _mockUser = new Mock<IUserService>();
            _mockAuth = new Mock<IAuthenticationManager>();
            _user = new UserModel
            {
                Username = "User",
                IsBanned = false,
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
            _controller = new CommentsController( _gameMock.Object, _mockComment.Object, _mockUser.Object,
                _mockAuth.Object);
        }

        [Test]
        public void Comments_ReturnsCommentsJson_GetsValidGameKey()
        {
            // Arrange 
            _mockComment.Setup(a => a.GetCommentsByGameKey(It.IsAny<string>()))
                .Returns(new List<CommentDTO> {new CommentDTO(), new CommentDTO()});
            // Act
            var res = _controller.Comments("");
            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }

        [Test]
        public void Comments_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            _mockComment.Setup(a => a.GetCommentsByGameKey(It.IsAny<string>())).Returns(new List<CommentDTO>());

            // Act
            var res = _controller.Comments("invalid-key");

            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }

        [Test]
        public void Index_ReturnsGameJson_GetsValidGameKey()
        {
            // Arrange
            _gameMock.Setup(a => a.GetByKey(It.IsAny<string>())).Returns(new GameDTO());

            // Act
            var res = _controller.Comments("valid-key");

            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }

        [Test]
        public void Index_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            _gameMock.Setup(a => a.GetByKey(It.IsAny<string>()))
                .Throws(new ValidationException(string.Empty, string.Empty));

            // Act
            var res = _controller.Comments("invalid-key");

            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }

        [Test]
        public void NewCommentWithQuote_ActionResult_GetValidValue()
        {
            Comments_ReturnsCommentsJson_GetsValidGameKey();
        }

        [Test]
        public void DetailGame_GetInvalidItems_ThrowExeption()
        {
            //Arrange
            _gameMock.Setup(a => a.GetByKey(It.IsAny<Guid>().ToString()))
                .Returns(new GameDTO() {Id = Guid.NewGuid().ToString(), Key = "name"})
                .Verifiable();

            //Act&&Assert
            Assert.Throws<ValidationException>(() => _controller.Details(""));
        }

        [Test]
        public void AddComment_GetValidItems_ReturnPartialViewResult()
        {
            // Act

            var res = _controller.NewComment("key");

            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }

        [Test]
        public void DeleteComment_GetValidItems_ReturnRedirectToRouteResult()
        {
            //Arrange
            _mockComment.Setup(a => a.DeleteById(It.IsAny<Guid>().ToString()));

            // Act
            var res = _controller.Delete("key", "id");

            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }
    }
}