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
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.PaymentService;
using GameStore.Web.ViewModels.Orders;
using Filter = GameStore.DAL.Enums.Filter;

namespace GameStore.Web.Tests.Controllers
{
    public class OrdersControllerTest
    {
        private readonly Mock<IGameService> _mockGame;
        private readonly Mock<IOrderService> _mockOrder;
        private readonly Mock<IAuthenticationManager> _mockAuth;
        private readonly Mock<IPaymentService> _mockservice;
        private readonly Mock<IService<OrderDetailDTO>> _mockOrderDetail;
        private readonly UserModel _user;
        private readonly OrdersController _controller;

        public OrdersControllerTest()
        {
            AutoMapperConfiguration.Configure();
            _mockOrder = new Mock<IOrderService>();
            _mockGame = new Mock<IGameService>();
            _mockAuth = new Mock<IAuthenticationManager>();
            _mockservice = new Mock<IPaymentService>();
            _mockOrderDetail = new Mock<IService<OrderDetailDTO>>();
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
            _controller = new OrdersController(_mockOrder.Object, _mockOrderDetail.Object, _mockGame.Object, _mockservice.Object, _mockAuth.Object);
        }

        [Test]
        public void GetOrders_BLLReturnSomeData()
        {
            // Arrange       
            _mockOrder.Setup(a => a.GetAll(false)).Returns(new List<OrderDTO> { new OrderDTO { Id = Guid.NewGuid().ToString(), IsConfirmed = true, Date = DateTime.UtcNow.AddDays(-50), IsPayed = true, IsShipped = false } });
            _mockOrderDetail.Setup(a => a.GetAll(false)).Returns(new List<OrderDetailDTO>()).Verifiable();
            _mockOrder.Setup(a => a.GetOrdersByFilter(It.IsAny<OrderFilterDTO>(),true, It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new OrderFilterResultDTO {Orders = new List<OrderDTO> {new OrderDTO {Id = Guid.NewGuid().ToString(),IsConfirmed = true, IsPayed = false, IsShipped = false} } });
            // Act
            var res = _controller.History(new OrderFilterViewModel {DateFrom = DateTime.UtcNow.AddDays(-100), DateTo = DateTime.UtcNow});

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetOrdersHistory_BLLReturnsSomeData()
        {
            // Arrange       
            _mockOrder.Setup(a => a.GetAll(false)).Returns(new List<OrderDTO>());
            _mockOrderDetail.Setup(a => a.GetAll(false)).Returns(new List<OrderDetailDTO>());
            _mockOrder.Setup(a => a.GetOrdersByFilter(It.IsAny<OrderFilterDTO>(), true, It.IsAny<int>(), It.IsAny<PageEnum>())).Returns(new OrderFilterResultDTO()
            {
                Orders = new List<OrderDTO>()
                { new OrderDTO()}
            });
            // Act
            var res = _controller.History(new OrderFilterViewModel());

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetOrdersHistoru_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            _mockOrder.Setup(a => a.GetAll(false)).Returns(new List<OrderDTO>()).Verifiable();
            _mockOrderDetail.Setup(a => a.GetAll(false)).Returns(new List<OrderDetailDTO>
            {
                new OrderDetailDTO
                {
                      Id =Guid.NewGuid().ToString(),
                      Discount = 0,
                      GameId = Guid.NewGuid().ToString(),
                      GameKey = "key",
                      IsPayed = false,
                      OrderId = Guid.NewGuid().ToString(),
                      Price = 2,
                      Quantity = 4
                }
            }).Verifiable();
            _mockOrder.Setup(a => a.GetOrdersByFilter(It.IsAny<OrderFilterDTO>(), true, It.IsAny<int>(), It.IsAny<PageEnum>()))
                .Returns(new OrderFilterResultDTO
                {
                    Orders = new List<OrderDTO>
                    {
                        new OrderDTO
                        {
                            Id =Guid.NewGuid().ToString(),
                            Date = DateTime.UtcNow,
                            IsConfirmed = false,
                            IsPayed = false,
                            IsShipped = false,
                            CustomerId = "user"
                        }
                    },
                    Count                    = 1
                });
            // Act
            var res = _controller.History(new OrderFilterViewModel {DateFrom = DateTime.UtcNow, DateTo = DateTime.UtcNow, FilterBy = Filter.New});

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void GetOrders_BLLReturnsSomeData()
        {
            // Arrange
            _mockOrder.Setup(a => a.GetAll(false)).Returns(new List<OrderDTO>()).Verifiable();
            _mockOrder.Setup(a => a.GetOrders(It.IsAny<string>())).Returns(new OrderDTO());

            // Act
            var res = _controller.Order("name");

            // Assert
            Assert.AreEqual(res.GetType(), typeof(RedirectToRouteResult));
        }

        [Test]
        public void GetBasket_BLLReturnsReturnsSomeData()
        {
            // Arrange
            _mockOrder.Setup(a => a.GetBasket(It.IsAny<Guid>().ToString())).Returns(new OrderDTO());

            // Act
            var res = _controller.Basket("name");

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }

        [Test]
        public void MakeOrder_BLLGetData()
        {
            // Arrange
            _mockOrder.Setup(x => x.ConfirmeOrder(It.IsAny<Guid>().ToString()));

            // Act
            var res = _controller.MakeOrder(Guid.NewGuid().ToString());

            // Assert
            Assert.AreEqual(res.GetType(), typeof(RedirectToRouteResult));
        }

        [Test]
        public void Pay_BLLGetData()
        {
            // Arrange
            _mockOrder.Setup(x => x.GetOrders(It.IsAny<string>())).Returns(new OrderDTO());
            _mockOrder.Setup(x => x.Pay(It.IsAny<string>()));

            // Act
            var res = _controller.Pay(PaymentTypes.CardPay, "user");

            // Assert
            Assert.AreEqual(res.GetType(), typeof(ViewResult));
        }



        [Test]
        public void DeleteBasket_GetValidItems()
        {
            // Arrange
            _mockOrder.Setup(a => a.DeleteOrder(It.IsAny<Guid>().ToString(), true));

            // Act
            var res = _controller.DeleteBusket("");

            // Assert
            Assert.AreEqual(typeof(RedirectToRouteResult), res.GetType());
        }
    }
}
