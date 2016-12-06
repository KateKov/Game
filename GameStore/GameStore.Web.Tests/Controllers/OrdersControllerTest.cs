
using Castle.Core.Logging;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.Web.Infrastracture;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Controllers;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Tests.Controllers
{
    public class OrdersControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IGameStoreService> _mock;
        private readonly Mock<IOrderService> _mockOrder;

        public OrdersControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
            AutoMapperConfiguration.Configure();
            _mock = new Mock<IGameStoreService>();
            _mockOrder = new Mock<IOrderService>();
        }

        [Test]
        public void GetGames_BLLReturnsSomeData()
        {
            // Arrange       
            _mock.Setup(a => a.GenericService<OrderDTO>().GetAll()).Returns(new List<OrderDTO>());
            _mock.Setup(a => a.GenericService<OrderDetailDTO>().GetAll()).Returns(new List<OrderDetailDTO>()).Verifiable();
            _mockOrder.Setup(a => a.GetOrdersByFilter(It.IsAny<OrderFilterDTO>(), It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new OrderFilterResultDTO());
            var sut = new OrdersController(_mockOrder.Object, _mock.Object);
            // Act
            var res = sut.History(new OrderFilterViewModel());

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetOrders_BLLReturnsSomeData()
        {
            // Arrange
            _mock.Setup(a => a.GenericService<OrderDTO>().GetAll()).Returns(new List<OrderDTO>()).Verifiable();
            _mockOrder.Setup(a => a.GetOrders()).Returns(new OrderDTO());
            var sut = new OrdersController(_mockOrder.Object, _mock.Object);

            // Act
            var res = sut.Order();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(RedirectToRouteResult));
        }

        [Test]
        public void GetBasket_BLLReturnsReturnsSomeData()
        {
            // Arrange
            _mockOrder.Setup(a => a.GetBusket(It.IsAny<Guid>().ToString())).Returns(new OrderDTO());
            var sut = new OrdersController(_mockOrder.Object, _mock.Object);

            // Act
            var res = sut.Basket();

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }


        [Test]
        public void DeleteBasket_GetValidItems()
        {
            // Arrange
            _mockOrder.Setup(a => a.DeleteBusket(It.IsAny<Guid>().ToString()));
            var sut = new OrdersController(_mockOrder.Object, _mock.Object);

            // Act
            var res = sut.DeleteBusket("");

            // Assert
            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
