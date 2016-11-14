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
                Games = Mapper.Map<List<GameViewModel>>(_service.GetAll<GameDTO>().Where(x=>string.IsNullOrEmpty(x.PublisherId)))
            };
            return View(publisher);
        }

        [HttpPost]
        public ActionResult New(CreatePublisherViewModel createPublisher)
        {
            var publisher = new PublisherViewModel() { Id = createPublisher.Id, Name=createPublisher.Name,  Description = createPublisher.Description, HomePage = createPublisher.HomePage };
            var publisherDto = Mapper.Map<PublisherDTO>(publisher);
            _service.AddOrUpdate(publisherDto, true);
            //var games = _service.GetAll<GameDTO>().Where(x => createPublisher.SelectedGames != null && createPublisher.SelectedGames.Contains(x.Key)).ToList();
            //games.ForEach(x => x.PublisherId = publisher.Id);
            //games.ForEach(x => x.PublisherName = publisher.Name);
            //games.ForEach(x => _service.AddOrUpdate(x, false));
          
            return RedirectToAction("Index");
        }
    }
}