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
            return View(publisher);
        }

        [HttpGet]
        public ActionResult New()
        {
            var publisher = new CreatePublisherViewModel()
            {
                Games = Mapper.Map<List<GameViewModel>>(_service.GetAll<GameDTO>()),
                Publisher = new PublisherViewModel()
            };
            return View(publisher);
        }

        [HttpPost]
        public ActionResult New(CreatePublisherViewModel createPublisher)
        {
            var publisher = createPublisher.Publisher;
            var publisherDto = Mapper.Map<PublisherDTO>(publisher);
            publisherDto.Games = _service.GetAll<GameDTO>().Where(x => createPublisher.SelectedGames.Contains(x.Key)).ToList();
            _service.AddOrUpdate<PublisherDTO>(publisherDto, true);
            return RedirectToAction("Index");
        }
    }
}