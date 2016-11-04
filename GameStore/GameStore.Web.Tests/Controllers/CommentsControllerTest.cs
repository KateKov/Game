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
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<int>())).Returns(new List<CommentDTO> { new CommentDTO(), new CommentDTO() });
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Comments("valid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(List<CommentViewModel>)));
            Assert.That(res.Data as List<CommentViewModel>, Has.Count.EqualTo(0));
        }

        [Test]
        public void Comments_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<int>())).Returns(new List<CommentDTO>());
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Comments("invalid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Empty);
        }

        [Test]
        public void Index_ReturnsGameJson_GetsValidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Returns(new GameDTO());
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Index("valid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(GameViewModel)));
            Assert.That(res.Data as GameViewModel, Is.Not.Null);
        }

        [Test]
        public void Index_ReturnsEmptyJson_GetsInvalidGameKey()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Throws(new ValidationException(string.Empty, string.Empty));
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Index("invalid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Null.Or.Empty);
        }

        [Test]
        public void DownloadGame_GetsInvalidKey_ReturnsNull()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Throws(new ValidationException(string.Empty, string.Empty));
            var sut = new CommentsController(mock.Object);

            // Act
            var res = sut.Download("invalid-key");

            // Assert
            Assert.That(res, Is.Null);
        }
    }
}
