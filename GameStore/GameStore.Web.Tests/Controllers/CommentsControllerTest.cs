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
    public class CommentsControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;

        public CommentsControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
            AutoMapperConfiguration.Configure();
        }

        [Test]
        public void Comments_ReturnsCommentsJson_GetsValidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGameKey(It.IsAny<string>())).Returns(new List<CommentDTO> { new CommentDTO(), new CommentDTO() });
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Comments("");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void Comments_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGameKey(It.IsAny<string>())).Returns(new List<CommentDTO>());
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Comments("invalid-key");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void Index_ReturnsGameJson_GetsValidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetByKey<GameDTO>(It.IsAny<string>())).Returns(new GameDTO());
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Comments("valid-key");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void Index_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetByKey<GameDTO>(It.IsAny<string>())).Throws(new ValidationException(string.Empty, string.Empty));
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Comments("invalid-key");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void DownloadGame_GetsInvalidKey_ReturnsNull()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetByKey<GameDTO>(It.IsAny<string>())).Throws(new ValidationException(string.Empty, string.Empty));
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Download("invalid-key");

            // Assert
            Assert.That(res, Is.Null);
        }
    }
}
