using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.Controllers;
using GameStore.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GameStore.Tests.Controllers
{
    class GameControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;

        public GameControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
        }

        #region GetGameComments

        [Test]
        public void GetGameComments_GetsValidGameKey_ReturnsCommentsJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<int>())).Returns(new List<CommentDTO> { new CommentDTO(), new CommentDTO() });
            var gameController= new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = gameController.Index("gamekey");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(List<CommentViewModel>)));
            Assert.That(res.Data as List<CommentViewModel>, Has.Count.EqualTo(2));
        }

        

        [Test]
        public void GetGameComments_GetsInvalidGameKey_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<int>())).Returns(new List<CommentDTO>());
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Comments(1);

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Empty);
        }

        #endregion

        #region CreateComment

        [Test, TestCaseSource(typeof(CommentViewModelData), nameof(CommentViewModelData.CommentValid))]
        public void CreateComment_GetsValidData_ReturnsStatusCodeOk(CommentViewModel commentView, string gamekey)
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
        [Test]
        public void GetGameDetails_GetsValidGameKey_ReturnsGameJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Returns(new GameDTO());
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Index("key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(GameViewModel)));
            Assert.That(res.Data as GameViewModel, Is.Not.Null);
        }
        [Test]
        public void GetGameDetails_GetsInvalidGameKey_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IService>();
            mock.Setup(a => a.GetGameByKey(It.IsAny<string>())).Throws(new ValidationException("", ""));
            var sut = new GameController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Index("key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Null.Or.Empty);
        }
        [Test, TestCaseSource(typeof(CommentViewModelData), nameof(CommentViewModelData.CommentInvalid))]
        public void CreateComment_GetsInvalidData_ReturnsStatusCodeBadRequest(CommentViewModel commentView, string gamekey)
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

        #endregion
    }
}
