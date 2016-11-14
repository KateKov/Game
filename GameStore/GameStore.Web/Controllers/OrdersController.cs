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
using GameStore.Web.Providers.Payments;

namespace GameStore.Web.Controllers
{
    public class OrdersController: Controller
    {
        private readonly IService _service;

        private Payment _pay;

        public OrdersController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Payment(string id ="")
        {
           var order = Mapper.Map<OrderViewModel>(_service.GetById<OrderDTO>(id));
           var bank = new Bank();
            return bank.Pay(order);
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
            return View(Mapper.Map<OrderViewModel>(order));
        }

        [HttpGet]
        public ActionResult MakeOrder(OrderViewModel basket)
        {
            basket.IsConfirmed = true;
            _service.AddOrUpdate<PublisherDTO>(Mapper.Map<PublisherDTO>(basket), false);
            return RedirectToAction("Order");
        }

        [HttpGet]
        public ActionResult Pay(OrderViewModel order, string paymentName)
        {
            _pay = new Payment(paymentName);
            return _pay.Pay(order);
        }

      

        [HttpGet]
        public ActionResult IBoxPay(OrderPaymentViewModel order)
        {
            var pay = new Payment();
            return pay.Pay(order.Order);
        }

        [HttpGet]
        public ActionResult AddToBusket(string gameId, short quantity, string customerId ="")
        {
            var product = _service.GetOrderDetail(gameId, quantity, customerId);
            _service.AddToBusket(product, customerId);
            return RedirectToAction("Basket");
        }

        [HttpGet]
        public ActionResult DeleteBusket(string customerId)
        {
            _service.DeleteBusket(customerId);
            return RedirectToAction("Basket");
        }

     
    }
}