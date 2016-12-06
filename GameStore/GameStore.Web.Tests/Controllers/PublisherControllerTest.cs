using GameStore.BLL.Interfaces;
using GameStore.Web.Infrastracture;
using Moq;
using NLog;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.Web.Controllers;
using GameStore.Web.ViewModels;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces.Services;

namespace GameStore.Web.Tests.Controllers
{
    public class PublisherControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IGameStoreService> _mock;

        public PublisherControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
            AutoMapperConfiguration.Configure();
            _mock = new Mock<IGameStoreService>();
        }

        [Test]
        public void GetPublishers_BLLReturnsSomeData()
        {
            // Arrange
            _mock.Setup(a => a.GenericService<PublisherDTO>().GetAll()).Returns(new List<PublisherDTO>());
            var sut = new PublishersController(_mock.Object);

            // Act
            var res = sut.Index();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetPublishers_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _mock.Setup(a => a.GenericService<PublisherDTO>().GetAll());
            var sut = new PublishersController(_mock.Object);

            // Act
            var res = sut.Index();

            // Assert
            Assert.Throws<ArgumentNullException>(() => res.ExecuteResult(sut.ControllerContext));
        }


        [Test]
        public void AddPublisher_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            _mock.Setup(a => a.GenericService<PublisherDTO>().AddOrUpdate(It.IsAny<PublisherDTO>(), true)).Throws(new ValidationException(string.Empty, string.Empty)).Verifiable();
            var sut = new PublishersController(_mock.Object);

            // Act
            var res = sut.New();

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

        [Test]
        public void DetailPublisher_GetValidItems_ReturnViewResult()
        {
            //Arrange
            _mock.Setup(a => a.NamedService<PublisherDTO, PublisherDTOTranslate>().GetByName(It.IsAny<string>())).Returns(new PublisherDTO()).Verifiable();
            var sut = new PublishersController(_mock.Object);

            //Act
            var res = sut.Details("name");

            // Assert
            Assert.AreEqual(typeof(ViewResult), res.GetType());
        }

    }
}
