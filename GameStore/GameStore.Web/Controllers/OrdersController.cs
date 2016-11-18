using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.Web.ViewModels;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.Web.Providers.Payments;
using Rotativa;

namespace GameStore.Web.Controllers
{
    public class OrdersController: Controller
    {
        private readonly IOrderService _service;

        private Payment _pay;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Basket(string customerId ="")
        {
            var basket = _service.GetBusket(customerId);
            return View(Mapper.Map<OrderViewModel>(basket));
        }

        [HttpGet]
        public ActionResult Order(string customerId ="")
        {
            var order = _service.GetOrders(customerId);
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
            var order = Mapper.Map<OrderViewModel>(_service.GetOrders(""));
            _pay = new Payment(paymentName);
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
        [HttpPost]
        public ActionResult AddToBasket(BasketViewModel basket)
        {
            if (ModelState.IsValid)
            {
                _service.GetOrderDetail(basket.GameId, short.Parse(basket.Quantity.Trim()), "");
                return PartialView("Success");
            }
            return PartialView("AddToBasket", basket);
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