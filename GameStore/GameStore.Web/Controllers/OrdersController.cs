using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.Web.ViewModels;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Providers.Payments;
using Rotativa;

namespace GameStore.Web.Controllers
{
    public class OrdersController: Controller
    {
        private readonly IOrderService _service;
        private readonly IGameStoreService _gameservice;

        private Payment _pay;

        public OrdersController(IOrderService service, IGameStoreService gameStoreService)
        {
            _service = service;
            _gameservice = gameStoreService;
        }

        [HttpGet]
        public ActionResult Basket(string customerId ="")
        {
            var basket = _service.GetBusket(customerId);
            return View(Mapper.Map<OrderViewModel>(basket));
        }

        [HttpGet]
        public ActionResult Order()
        {
            if (!_gameservice.GenericService<OrderDTO>().GetAll().Any(x => x.IsConfirmed))
            {
                return RedirectToAction("Basket");
            }
            var order = _service.GetOrders();
            var orderViewModel = Mapper.Map<OrderViewModel>(order);
            var orderPayment = new OrderPaymentViewModel();
            orderPayment.Order = orderViewModel;
            return View(orderPayment);
        }

        [HttpGet]
        public ActionResult MakeOrder(string id)
        {
            _service.ConfirmeOrder(id);
            return RedirectToAction("Order");
        }

        [HttpGet]
        public ActionResult Pay(string orderId, string paymentName)
        {
            var order = Mapper.Map<OrderViewModel>(_service.GetOrders());
            _pay = new Payment((Payment.PaymentTypes)Enum.Parse(typeof(Payment.PaymentTypes), paymentName));
            _service.PayOrder();
            return _pay.Pay(order, View);
        }

        [HttpPost]
        public ActionResult Visa(VisaViewModel visa)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Pay", new {orderId="", paymentName="Bank"});
            }
            return View("~/Views/Orders/Visa.cshtml", visa);
        }

        [ChildActionOnly]
        public PartialViewResult Filters(OrderFilterViewModel model)
        {
            return PartialView("Filter", model);
        }

        [HttpGet]
        public ActionResult History(OrderFilterViewModel orderFilterViewModel, int page = 1, PageEnum pageSize = PageEnum.Ten)
        {
            var filterDto = Mapper.Map<OrderFilterDTO>(orderFilterViewModel);
            var filterResult = _service.GetOrdersByFilter(filterDto, page, pageSize);

            var ordersViewModel = Mapper.Map<IEnumerable<OrderViewModel>>(filterResult.Orders);
            var orderListViewModel = new OrderFilteringViewModel
            {
                Orders = ordersViewModel,
                Filter = orderFilterViewModel,
                Page = page,
                PageSize = pageSize,
                TotalItemsCount = filterResult.Count
            };
            return View(orderListViewModel);
        }

        [HttpPost]
        public ActionResult AddToBasket(BasketViewModel basketModel)
        {      
            if (ModelState.IsValid)
            {
                var basket = basketModel;
                _service.GetOrderDetail(basket.GameKey, short.Parse(basket.Quantity.Trim()), "");
                return PartialView("Success");
            }
            return PartialView("AddToBasket", basketModel);
        }

        [HttpGet]
        public ActionResult DeleteBusket(string customerId)
        {
            _service.DeleteBusket(customerId);
            return RedirectToAction("Basket");
        }

        public ActionResult PdfFormat(OrderViewModel order, string paymentName)
        {
            return new ActionAsPdf("Pay",new { order, paymentName} );
        }
     
    }
}