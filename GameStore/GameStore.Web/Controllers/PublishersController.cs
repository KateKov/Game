using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.Web.ViewModels;
using AutoMapper;

namespace GameStore.Web.Controllers
{
    public class PublishersController : Controller
    {

        private readonly IService _service;

        public PublishersController(IService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            var publishers = _service.GetAll<PublisherDTO>();
            return View(Mapper.Map<IEnumerable<PublisherViewModel>>(publishers));
        }

        [HttpGet]
        public ActionResult Details(string companyName)
        {
            var publisher = _service.GetByName<PublisherDTO>(companyName);
            return View(Mapper.Map<PublisherViewModel>(publisher));
        }

        [HttpGet]
        public ActionResult New()
        {
            var publisher = new PublisherViewModel()
            {
                Id =Guid.NewGuid().ToString()
            };
            return View(publisher);
        }

        [HttpPost]
        public ActionResult New(PublisherViewModel createPublisher)
        {
            if (ModelState.IsValid)
            {
                var publisherDto = Mapper.Map<PublisherDTO>(createPublisher);
                _service.AddOrUpdate(publisherDto, true);
                return RedirectToAction("Index");
            }
            return View("New", createPublisher);
        }
    }
}