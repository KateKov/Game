using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTest
    {
        private readonly Mock<NLog.ILogger> _loggerMock;

        public GameControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
        }

        [Test]
        public void Comments_GetsValidGameKey_ReturnsCommentsJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<int>())).Returns(new List<CommentDTO> { new CommentDTO(), new CommentDTO() });
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Comments("valid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(List<CommentViewModel>)));
            Assert.That(res.Data as List<CommentViewModel>, Has.Count.EqualTo(2));
        }

        [Test]
        public void Comments_GetsInvalidGameKey_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<int>())).Returns(new List<CommentDTO>());
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Comments("invalid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Empty);
        }

        [Test, TestCaseSource(typeof(CommentViewModelData), nameof(CommentViewModelData.CommentValid))]
        public void NewComment_GetsValidData_ReturnsStatusCodeOk(CommentViewModel commentView, string gamekey)
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.AddComment(It.IsAny<CommentDTO>(), gamekey)).Verifiable();
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.NewComment(commentView, gamekey);

            // Assert            
            Assert.That(res.StatusCode, Is.EqualTo(201));
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(CommentViewModelData), nameof(CommentViewModelData.CommentInvalid))]
        public void NewComment_GetsInvalidData_ReturnsStatusCodeBadRequest(CommentViewModel commentView, string gamekey)
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.AddComment(It.IsAny<CommentDTO>(), gamekey)).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.NewComment(commentView, gamekey);

            // Assert            
            Assert.That(res.StatusCode, Is.EqualTo(400));
            Mock.Verify(mock);
        }

        [Test]
        public void Index_GetsValidGameKey_ReturnsGameJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Returns(new GameDTO());
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Index("valid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(GameViewModel)));
            Assert.That(res.Data as GameViewModel, Is.Not.Null);
        }

        [Test]
        public void Index_GetsInvalidGameKey_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Throws(new ValidationException("", ""));
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Index("invalid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Null.Or.Empty);
        }
    }
}
