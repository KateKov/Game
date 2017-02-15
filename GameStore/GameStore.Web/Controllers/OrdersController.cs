using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Interfaces;
using GameStore.Web.Providers.Payments;
using GameStore.Web.ViewModels.Games;
using GameStore.Web.ViewModels.OrderDetails;
using GameStore.Web.ViewModels.Orders;
using Rotativa;
using WebGrease.Css.Extensions;
using GameStore.Web.Helpers;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.PaymentService;
using GameStore.Web.Providers;
using PaymentStatus = GameStore.DAL.Enums.PaymentStatus;

namespace GameStore.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrderService _service;
        private readonly IGameService _gameService;
        private readonly IService<OrderDetailDTO> _orderDetailService;
        private readonly IPaymentService _paymentService;
        private Payment _pay;
        private CreditCardPayments _creditCardPayments;

        public OrdersController(IOrderService service, IService<OrderDetailDTO> orderDetail, IGameService gameService, IPaymentService payment,
            IAuthenticationManager authentication) : base(authentication)
        {
            _service = service;
            _gameService = gameService;
            _paymentService = payment;
            _orderDetailService = orderDetail;
        }

        [HttpGet]
        public ActionResult Basket(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.Name;
            }

            var basket = _service.GetBasket(username);
            return View(Mapper.Map<OrderViewModel>(basket));
        }

        [HttpGet]
        public ActionResult Order(string username)
        {
            if (!_service.GetAll(false).Any(x => x.IsConfirmed))
            {
                return RedirectToAction("Basket", username);
            }
            var order = _service.GetOrders(username);
            var orderViewModel = Mapper.Map<OrderViewModel>(order);
            var orderPayment = new OrderPaymentViewModel {Order = orderViewModel};
            return View(orderPayment);
        }


        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult OrderHistory(string username)
        {
            if (_service.GetOrders(username) == null)
            {
                return RedirectToAction("Basket", username);
            }
            var order = _service.GetOrders(username);
            var orderViewModel = Mapper.Map<OrderViewModel>(order);
            var orderPayment = new OrderPaymentViewModel {Order = orderViewModel};
            return View(orderPayment);
        }

        [HttpGet]
        public ActionResult MakeOrder(string username)
        {
            _service.ConfirmeOrder(username);
            return RedirectToAction("Order", "Orders", username);
        }

        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult Shipe(string username, bool isShipped)
        {
            _service.ChangeStatus(username, !isShipped);
            return RedirectToAction("Order", "Orders", username);
        }

        [HttpGet]
        public ActionResult Pay(PaymentTypes payment, string username)
        {
            var order = Mapper.Map<OrderViewModel>(_service.GetOrders(username));
            _pay = new Payment(payment);
            _service.Pay(username);
            return _pay.Pay(order, View);
        }

        [HttpPost]
        public ActionResult CardPay(VisaViewModel visa)
        {
            if (!ModelState.IsValid)
            {
                return View(visa);
            }

            var order = _service.GetById(visa.OrderId);
            visa.Months = GetDate.GetAvailableMonths();
            visa.Years = GetDate.GetAvailableYears();

            var paymentParams = Mapper.Map<PaymentParams>(visa);
            paymentParams.ToCardNumber = "11111111111111111";
            paymentParams.Amount = order.OrderDetails.Sum(x => x.Price);
            paymentParams.Purpose = $"Gamestore Invoice number #{order.Id}";

            _creditCardPayments = new CreditCardPayments(_paymentService);
            var payment = _creditCardPayments.Pay(visa.CardType)(paymentParams).Result;
           
            var paymentStatus = Mapper.Map<PaymentStatus>(payment);

            if (paymentStatus != PaymentStatus.Succesful)
            {
                ModelState.AddModelError(string.Empty,
                    string.Format(GlobalRes.OrderTransferError, EnumDropDownListHelper.GetDisplayName(paymentStatus)));
                return View(visa);
            }

            _service.Pay(order.CustomerId);
            return RedirectToAction("Pay", new {payment = PaymentTypes.Bank, username = User.Identity.Name});
        }

        [ChildActionOnly]
        public PartialViewResult Filters(OrderFilterViewModel model)
        {
            return PartialView("Filter", model);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult History(OrderFilterViewModel orderFilterViewModel, int page = 1,
            PageEnum pageSize = PageEnum.Ten)
        {
            var orderListViewModel = GetOrderFilter(orderFilterViewModel, true, page, pageSize);
            return View(orderListViewModel);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult OrderNew(OrderFilterViewModel orderFilterViewModel, int page = 1,
            PageEnum pageSize = PageEnum.Ten)
        {
            var orderListViewModel = GetOrderFilter(orderFilterViewModel, false, page, pageSize);
            return View(orderListViewModel);
        }

        [HttpGet]
        public ActionResult AddGameToOrder(string username)
        {
            var orderDetail = new CreateOrderDetail
            {
                Id = Guid.NewGuid().ToString(),
                AllGames = new List<GameViewModel>(),
                Username = username,
                Quantity = 1
            };
            Mapper.Map<IEnumerable<GameViewModel>>(_gameService.GetAll(false))
                .ForEach(
                    x =>
                        orderDetail.AllGames.Add(new GameViewModel()
                        {
                            Key = x.Key,
                            Name = x.Name,
                            UnitsInStock = x.UnitsInStock,
                            Price = x.Price
                        }));
            return View(orderDetail);
        }

        [HttpPost]
        public ActionResult AddGameToOrder(CreateOrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _service.AddOrderDetail(orderDetail.GameKey, orderDetail.Quantity, orderDetail.Username, false);
                return RedirectToAction("Order", "Orders", orderDetail.Username);
            }
            orderDetail.AllGames = new List<GameViewModel>();
            Mapper.Map<IEnumerable<GameViewModel>>(_gameService.GetAll(false))
                .ForEach(
                    x =>
                        orderDetail.AllGames.Add(new GameViewModel()
                        {
                            Key = x.Key,
                            Name = x.Name,
                            UnitsInStock = x.UnitsInStock,
                            Price = x.Price
                        }));
            return PartialView("AddGameToOrder", orderDetail);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
        public ActionResult EditGameInOrder(string orderDetailId)
        {
            var orderDetail =
                Mapper.Map<CreateOrderDetail>(_orderDetailService.GetById(orderDetailId));
            var game = _gameService.GetByKey(orderDetail.GameKey);
            orderDetail.Price = game.Price;
            orderDetail.UnitsInStock = game.UnitsInStock;
            return View(orderDetail);
        }

        [HttpPost]
        public ActionResult EditGameInOrder(CreateOrderDetail orderDetail, string username)
        {
            if (ModelState.IsValid)
            {
                var orderDetailDto = Mapper.Map<OrderDetailDTO>(orderDetail);
                _service.UpdateOrder(username, orderDetailDto, false);
                return RedirectToAction("Order");
            }
            var game = _gameService.GetByKey(orderDetail.GameKey);
            orderDetail.Price = game.Price;
            orderDetail.UnitsInStock = game.UnitsInStock;
            return PartialView("EditGameInOrder", orderDetail);
        }

        [HttpGet]
        public ActionResult DeleteBusket(string username)
        {
            _service.DeleteOrder(username, true);
            return RedirectToAction("Basket");
        }

        [HttpGet]
        public ActionResult DeleteOrder(string username)
        {
            _service.DeleteOrder(username, false);
            return RedirectToAction("Order");
        }

        [HttpGet]
        public ActionResult DeleteOrderDetail(string orderDetailId, string username)
        {
            _orderDetailService.DeleteById(orderDetailId);
            return RedirectToAction("Order", "Orders", username);
        }

        public ActionResult PdfFormat(OrderViewModel order, string paymentName)
        {
            return new ActionAsPdf("Pay", new {order, paymentName});
        }

        [OutputCache(Duration = 60)]
        public ActionResult BasketInfo()
        {
            var basket = _service.GetBasket(User.Identity.Name);
            var count = basket?.OrderDetails?.Sum(x=>x.Quantity) ?? 0  ;
            var sum = basket?.OrderDetails?.Sum(x=>x.Price) ?? 0;
            var info = new BasketInfo
            {
                Count = count,
                Summ = sum
            };
            return PartialView("BasketInfo", info);
        }

        private OrderFilteringViewModel GetOrderFilter(OrderFilterViewModel orderFilterViewModel, bool isHistory,
            int page = 1, PageEnum pageSize = PageEnum.Ten)
        {
            var filterDto = Mapper.Map<OrderFilterDTO>(orderFilterViewModel);
            var filterResult = _service.GetOrdersByFilter(filterDto, isHistory, page, pageSize);

            var ordersViewModel = Mapper.Map<IEnumerable<OrderViewModel>>(filterResult.Orders);
            var orderListViewModel = new OrderFilteringViewModel
            {
                Orders = ordersViewModel,
                Filter = orderFilterViewModel,
                Page = page,
                PageSize = pageSize,
                TotalItemsCount = filterResult.Count
            };

            return orderListViewModel;
        }
    }
}