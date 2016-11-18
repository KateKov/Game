using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.Web.Providers;
using GameStore.Web.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IService _service;
        private readonly IGameService _gameService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public GamesController(IService service, IGameService gameService)
        {
            _service = service;
            _gameService = gameService;
        }
      
        [HttpGet]   
        public ActionResult Index()
        {
            var games = Mapper.Map<IEnumerable<GameViewModel>>(_service.GetAll<GameDTO>());
            var gamesWithFilter = new GameFilteringViewModel() {Filter = GetDefaultFilters(), Games = games};
            return View(gamesWithFilter);
        }

        private FilterViewModel GetDefaultFilters()
        {
            var filter = new FilterViewModel()
            {
                ListGenres = _service.GetAll<GenreDTO>().Select(x => new Providers.CheckBox() { Text = x.Name, Checked = false }).ToList(),
                ListPublishers = _service.GetAll<PublisherDTO>().Select(x => new Providers.CheckBox() { Text = x.Name, Checked = false }).ToList(),
                ListTypes = _service.GetAll<PlatformTypeDTO>().Select(x => new Providers.CheckBox() { Text = x.Name, Checked = false }).ToList()
            };
            return filter;
        }

        private IEnumerable<GameViewModel> GetValues<T>(IEnumerable<GameViewModel> games, string name) where T : class, IDtoNamed, new()
        {
            return Mapper.Map<IEnumerable<GameViewModel>>(
                _gameService.GetGamesByNameOfProperty<GenreDTO>(
                    Mapper.Map<IEnumerable<GameDTO>>(games), name));
        }

        [HttpPost]
        public ActionResult Filter(FilterViewModel filter)
        {
            var games = new List<GameViewModel>();
            var selectedGenres = filter.ListGenres.Where(x=>x.Checked).Select(x =>x.Text).ToList();
            var selectedTypes = filter.ListTypes.Where(x => x.Checked).Select(x => x.Text).ToList();
            var selectedPublishers = filter.ListPublishers.Where(x => x.Checked).Select(x => x.Text).ToList();
            if (selectedPublishers.Count<1 && selectedPublishers.Count < 1 && selectedGenres.Count < 1 && string.IsNullOrEmpty(filter.SelectedFilter))
            {
                games = Mapper.Map<IEnumerable<GameViewModel>>(_service.GetAll<GameDTO>()).ToList();
            }
            else
            {
                selectedGenres.ForEach(
                    x =>
                        games.AddRange(GetValues<GenreDTO>(games,x)));
                selectedTypes.ForEach(
                    x =>
                        games.AddRange(GetValues<PlatformTypeDTO>(games, x)));
                selectedPublishers.ForEach(
                    x =>
                        games.AddRange(GetValues<PublisherDTO>(games, x)));
                var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(games);
                games = Mapper.Map<IEnumerable<GameViewModel>>(_gameService.Filter(gamesDto, filter.SelectedFilter)).ToList();
            }
            var gamesWithFilters = new GameFilteringViewModel() {Filter = filter, Games = games};
            return View("Index", gamesWithFilters);
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
                gameViewModel.Key = GenerateKey(game.Name, game.PublisherName);
                var gameDto = Mapper.Map<GameDTO>(gameViewModel);
                _service.AddOrUpdate(gameDto, true);
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
                  _service.GetAll<PublisherDTO>()).ToList();
            game.AllTypes =
                Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(
                    _service.GetAll<PlatformTypeDTO>()).ToList();
            game.AllGenres =
                Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(_service.GetAll<GenreDTO>()).ToList();
            return game;
        }

        private string GenerateKey(string name, string publisherName)
        {
            return name+"_"+publisherName;
        }

        [HttpGet]
        public ActionResult Update(string key)
        {
            var game = _service.GetByKey<GameDTO>(key);
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
                var gameDto = Mapper.Map<UpdateGameViewModel, GameDTO>(game);
                _service.AddOrUpdate(gameDto, false);
                 Response.StatusCode=(int) HttpStatusCode.OK;
                return RedirectToAction("Index");
            }
            game = GetModels(game);
            return View("Update", game);
        }
              
        [HttpGet]
        public ActionResult AddToBasket(string gameId, short unitsInStock, string customerId="")
        {
            var basket = new BasketViewModel() {GameId = gameId, CustomerId = customerId, UnitInStock = unitsInStock};
            return PartialView("AddToBasket", basket);
        }
    }
}