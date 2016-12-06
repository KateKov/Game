using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastracture;
using GameStore.Web.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class CommentsControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<ICommentService> _mockComment;
        private readonly Mock<IGameStoreService> _mock;

        public CommentsControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
            AutoMapperConfiguration.Configure();
            _mockComment = new Mock<ICommentService>();
            _mock = new Mock<IGameStoreService>();
        }
     
        [Test]
        public void Comments_ReturnsCommentsJson_GetsValidGameKey()
        {
            // Arrange 
            _mockComment.Setup(a => a.GetCommentsByGameKey(It.IsAny<string>()))
                .Returns(new List<CommentDTO> {new CommentDTO(), new CommentDTO()});
            var sut = new CommentsController(_mock.Object, _mockComment.Object);
            // Act
            var res = sut.Comments("");
            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void Comments_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            _mockComment.Setup(a => a.GetCommentsByGameKey(It.IsAny<string>())).Returns(new List<CommentDTO>());
            var sut = new CommentsController(_mock.Object, _mockComment.Object);

            // Act
            var res = sut.Comments("invalid-key");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void Index_ReturnsGameJson_GetsValidGameKey()
        {
            // Arrange
            _mock.Setup(a => a.KeyService<GameDTO>().GetByKey(It.IsAny<string>())).Returns(new GameDTO());
            var sut = new CommentsController(_mock.Object, _mockComment.Object);

            // Act
            var res = sut.Comments("valid-key");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void Index_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            _mock.Setup(a => a.KeyService<GameDTO>().GetByKey(It.IsAny<string>()))
                .Throws(new ValidationException(string.Empty, string.Empty));
            var sut = new CommentsController(_mock.Object, _mockComment.Object);

            // Act
            var res = sut.Comments("invalid-key");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
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
            _mock.Setup(a => a.KeyService<GameDTO>().GetByKey(It.IsAny<Guid>().ToString())).Returns(new GameDTO() {Id = Guid.NewGuid().ToString(), Key = "name"}).Verifiable();
            var sut = new CommentsController(_mock.Object, _mockComment.Object);

            //Act&&Assert
            Assert.Throws<Exception>(() => sut.Details(""));
        }

        [Test]
        public void AddComment_GetValidItems_ReturnPartialViewResult()
        {
            //Arrange
            var sut = new CommentsController(_mock.Object, _mockComment.Object);

            // Act
            var res = sut.NewComment("key");

            // Assert
            Assert.AreEqual(typeof(PartialViewResult), res.GetType());
        }

        [Test]
        public void DeleteComment_GetValidItems_ReturnRedirectToRouteResult()
        {
            //Arrange
            _mock.Setup(a => a.GenericService<CommentDTO>().DeleteById(It.IsAny<Guid>().ToString()));
            var sut = new CommentsController(_mock.Object, _mockComment.Object);

            // Act
            var res = sut.Delete("key", "id");

            // Assert
            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
