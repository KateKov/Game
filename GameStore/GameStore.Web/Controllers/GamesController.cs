using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.Web.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    public class GamesController : Controller
    {
        private readonly IService _gameService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public GamesController(IService gameService)
        {
            _gameService = gameService;        
        }
      
        [HttpGet]   
        public ActionResult Index()
        {
            try
            {           
                _logger.Info("Request to GamesController.Get");
               
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to load the gameService from GamesController failed : {0}",  ex.StackTrace);
               
            }
            return View(Mapper.Map<IEnumerable<GameViewModel>>(_gameService.GetAll<GameDTO>()));
        }

        [HttpGet]
        public ActionResult New()
        {
            var game = new UpdateGameViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                AllPublishers = Mapper.Map<IEnumerable<PublisherDTO>, IEnumerable<PublisherViewModel>>(_gameService.GetAll<PublisherDTO>()).ToList(),
                AllTypes = Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(_gameService.GetAll<PlatformTypeDTO>()).ToList(),
                AllGenres = Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(_gameService.GetAll<GenreDTO>()).ToList()
            };
            return View(game);
        }

        [HttpPost]
        public ActionResult New(UpdateGameViewModel game)
        {
            if (ModelState.IsValid)
            {
                var gameViewModel = game;
                gameViewModel.Key = GenerateKey(game.Name, game.PublisherName);
                var gameDto = Mapper.Map<GameDTO>(gameViewModel);
                _gameService.AddOrUpdate(gameDto, true);
                _logger.Info("Game is created. Id {0} Key {1} ", gameViewModel.Id, gameViewModel.Key);
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private string GenerateKey(string name, string publisherName)
        {
            return name+"_"+publisherName;
        }

        [HttpGet]
        public ActionResult Update(string key)
        {
            var game = _gameService.GetByKey<GameDTO>(key);
            var gameUpdate = Mapper.Map<GameDTO, UpdateGameViewModel>(game);
            gameUpdate.AllTypes = Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(
                    _gameService.GetAll<PlatformTypeDTO>()).ToList();
            gameUpdate.AllGenres = Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(_gameService.GetAll<GenreDTO>()).ToList();
            gameUpdate.AllPublishers =
                Mapper.Map<IEnumerable<PublisherDTO>, IEnumerable<PublisherViewModel>>(_gameService.GetAll<PublisherDTO>()).ToList();
            return View(gameUpdate);
        }

        [HttpPost]
        public ActionResult Update(UpdateGameViewModel game)
        {
            _logger.Info("Request to GamesController.Update");
            if(ModelState.IsValid)
            { GameDTO gameDto = Mapper.Map<UpdateGameViewModel, GameDTO>(game);
                _gameService.AddOrUpdate(gameDto, false);
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
              
        [HttpPost] 
        public HttpStatusCodeResult Remove(GameViewModel game)
        {
            _logger.Info("Request to GamesController.Remove");
            try
            {
                _gameService.DeleteById<GameDTO>(game.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to remove a game with Id {0} failed : {1}", game.Id, ex.StackTrace);
            }

            _logger.Info($"Game is removed. Id: {game.Id}; Key: {game.Key}");
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }  
    }
}