using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.Web.ViewModels;
using AutoMapper;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces.Services;

namespace GameStore.Web.Controllers
{
    public class PublishersController : Controller
    {

        private readonly IGameStoreService _service;

        public PublishersController(IGameStoreService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            var publishers = _service.GenericService<PublisherDTO>().GetAll();
            return View(Mapper.Map<IEnumerable<PublisherViewModel>>(publishers));
        }

        [HttpGet]
        public ActionResult Details(string companyName)
        {
            var publisher = _service.NamedService<PublisherDTO, PublisherDTOTranslate>().GetByName(companyName);
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
                _service.GenericService<PublisherDTO>().AddOrUpdate(publisherDto, true);
                return RedirectToAction("Index");
            }
            return View("New", createPublisher);
        }
    }
}