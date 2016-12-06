using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Providers;
using GameStore.Web.ViewModels;
using NLog;
using WebGrease.Css.Extensions;
using Filter = GameStore.DAL.Enums.Filter;

namespace GameStore.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameStoreService _service;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGameService _gameService;
        public GamesController(IGameStoreService service, IGameService gameService)
        {
            _service = service;
            _gameService = gameService;
        }
      
        [HttpGet]   
        public ActionResult Index(FilterViewModel gameFilterViewModel,  int page = 1,  PageEnum pageSize = PageEnum.Ten)
        {
            //var filterDto = Mapper.Map<FilterDTO>(gameFilterViewModel);
            //var filterResult = _gameService.GetAllByFilter(filterDto, page, pageSize);
            //var gameViewModel = Mapper.Map<IEnumerable<GameViewModel>>(filterResult.Games);
            var games = _service.GenericService<GameDTO>().GetAll();
            var gameListViewModel = new GameFilteringViewModel
            {
                //Games = gameViewModel,
                
                Games = Mapper.Map<List<GameViewModel>>(games),
                Filter = gameFilterViewModel,
                Page = page,
                PageSize = pageSize,
                //TotalItemsCount = filterResult.Count
                TotalItemsCount = 100
            };
            return View(gameListViewModel);
        }


        [HttpGet]
        public ActionResult New()
        {
            var game = new UpdateGameViewModel()
            {
                Id = Guid.NewGuid().ToString(),
            };
            game = GetModels(game);
            return View(game);
        }

        [HttpPost]
        public ActionResult New(UpdateGameViewModel game)
        {
            if (ModelState.IsValid)
            {
                var gameViewModel = game;
                gameViewModel.DateOfAdding = DateTime.UtcNow;
                gameViewModel.Key = GenerateKey(game.Name);
                var gameDto = Mapper.Map<GameDTO>(gameViewModel);
                _service.GenericService<GameDTO>().AddOrUpdate(gameDto, true);
                _logger.Info("Game is created. Id {0} Key {1} ", gameViewModel.Id, gameViewModel.Key);
                return RedirectToAction("Index");
            }
            game = GetModels(game);
            return View("New", game);
        }

        private UpdateGameViewModel GetModels(UpdateGameViewModel game)
        {
            game.AllPublishers =
              Mapper.Map<IEnumerable<PublisherDTO>, IEnumerable<PublisherViewModel>>(
                  _service.GenericService<PublisherDTO>().GetAll()).ToList();
            game.AllTypes =
                Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(
                    _service.GenericService<PlatformTypeDTO>().GetAll()).ToList();
            game.AllGenres =
                Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(_service.GenericService<GenreDTO>().GetAll()).ToList();
            return game;
        }

        private string GenerateKey(string name)
        {
            var keyArray = name.Split(' ');
            string key = "";
            keyArray.ForEach(x => key += x + "_");
            return key.Substring(0, key.Length - 2);
        }

        [HttpGet]
        public ActionResult Update(string key)
        {
            var game = _service.KeyService<GameDTO>().GetByKey(key);
            var gameUpdate = Mapper.Map<GameDTO, UpdateGameViewModel>(game);
            gameUpdate = GetModels(gameUpdate);
            return View(gameUpdate);
        }

        [HttpPost]
        public ActionResult Update(UpdateGameViewModel game)
        {
            _logger.Info("Request to GamesController.Update");
            if(ModelState.IsValid)
            {
                var gameViewModel = game;
                var gameDto = Mapper.Map<GameDTO>(gameViewModel);
                _service.GenericService<GameDTO>().AddOrUpdate(gameDto, false);
                return RedirectToAction("Index");
            }
            game = GetModels(game);
            return View("Update", game);
        }
              
        [HttpGet]
        public ActionResult AddToBasket(string gameKey, short unitsInStock, string customerId="")
        {
            var basket = new BasketViewModel() { GameKey = gameKey, CustomerId = customerId, UnitInStock = unitsInStock };
            return PartialView("AddToBasket", basket);
        }

        [OutputCache(Duration = 60)]
        [ChildActionOnly]
        public ActionResult CountGames()
        {
            var count = _service.GenericService<GameDTO>().GetAll().Count();
            return PartialView("CountGames", count);
        }

        [ChildActionOnly]
        public PartialViewResult Filters(FilterViewModel model)
        {
            var filterViewModel = GetFilterViewModel(model.SelectedGenresName, model.SelectedTypesName, model.SelectedPublishersName);
            return PartialView("Filter",filterViewModel);
        }

        private FilterViewModel GetFilterViewModel(IEnumerable<string> genresName = null, IEnumerable<string> typesName = null, IEnumerable<string> publishersName = null)
        { 
            var model = new FilterViewModel()
            {
                ListGenres = GetListOfItems<GenreDTOTranslate>(),
                ListTypes = GetListOfItems<PlatformTypeDTOTranslate>(),
                ListPublishers = GetListOfItems<PublisherDTOTranslate>()
            };
            if (genresName != null)
            {
                model.SelectedGenres = model.ListGenres.Where(x => genresName.Contains(x.Text));
                model.SelectedGenresName = genresName;
            }
            if (typesName != null)
            {
                model.SelectedType = model.ListTypes.Where(x => typesName.Contains(x.Text));
                model.SelectedTypesName = typesName;
            }
            if (publishersName != null)
            {
                model.SelectedPublishers = model.ListPublishers.Where(x => publishersName.Contains(x.Text));
                model.SelectedPublishersName = publishersName;
            }
            return model;
        }

        private List<CheckBox> GetListOfItems<T>() where T : class, IDtoNamed, new()
        {
            var list= new List<CheckBox>();
            _service.GenericService<T>().GetAll().ToList().ForEach(x => list.Add(new CheckBox()
            {
                Text = x.Name,
                Checked = false
            }));
            return list;
        }
    }
}