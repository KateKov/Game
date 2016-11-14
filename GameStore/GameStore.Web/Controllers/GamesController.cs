using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
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
            var game = new UpdateGameViewModel();
            game.Game = new GameViewModel();
            game.Publishers = Mapper.Map<IEnumerable<PublisherDTO>, IEnumerable<PublisherViewModel>>(_gameService.GetAll<PublisherDTO>()).ToList();
            game.Genres =
                Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(_gameService.GetAll<GenreDTO>()).ToList();
            game.Types =
                Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(
                    _gameService.GetAll<PlatformTypeDTO>()).ToList();
            return View(game);
        }

        [HttpPost]
        public ActionResult New(UpdateGameViewModel game)
        {
           
                GameViewModel gameViewModel;
                //try
                //{
                gameViewModel = game.Game;
                gameViewModel.Key = GenerateKey(gameViewModel.Name, gameViewModel.PublisherName);
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(gameViewModel);
                _gameService.AddOrUpdate<GameDTO>(gameDto, true);
                //}
                //catch (Exception)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}

                _logger.Info("Game is created. Id {0} Key {1} ", gameViewModel.Id, gameViewModel.Key);
            
            return RedirectToAction("Index");
        }

        private string GenerateKey(string name, string publisherName)
        {
            return name + "_" + publisherName;
        }

        [HttpPost]
        public HttpStatusCodeResult Update(GameViewModel game)
        {
            _logger.Info("Request to GamesController.Update");
            try
            {
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(game);
                _gameService.AddOrUpdate(gameDto, false);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
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