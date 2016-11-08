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
        public JsonResult Index()
        {
            try
            {           
                _logger.Info("Request to GamesController.Get");
                return Json(Mapper.Map<IEnumerable<GameViewModel>>(_gameService.GetAll<GameDTO>()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to load the gameService from GamesController failed : {0}",  ex.StackTrace);
                return new JsonResult();
            }
        }

        [HttpGet]
        public ActionResult New()
        {
            UpdateGameViewModel game = new UpdateGameViewModel();
            game.Publishers = Mapper.Map<IEnumerable<PublisherDTO>, IEnumerable<PublisherViewModel>>(_gameService.GetAll<PublisherDTO>()).ToList();
            game.Genres =
                Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(_gameService.GetAll<GenreDTO>()).ToList();
            game.Types =
                Mapper.Map<IEnumerable<PlatformTypeDTO>, IEnumerable<PlatformTypeViewModel>>(
                    _gameService.GetAll<PlatformTypeDTO>()).ToList();
            game.Game = new GameViewModel();
            return View(game);
        }

        [HttpPost]
        public HttpStatusCodeResult New(UpdateGameViewModel game)
        {
            GameViewModel gameViewModel;
            try
            {
                gameViewModel = game.Game;
                gameViewModel.Key = GenerateKey(gameViewModel.Name, gameViewModel.PublisherName);
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(gameViewModel);
                _gameService.AddGame(gameDto);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _logger.Info("Game is created. Id {0} Key {1} ", gameViewModel.Id, gameViewModel.Key);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
                _gameService.EditGame(gameDto);
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