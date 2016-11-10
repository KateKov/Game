using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.Web.Filters;
using GameStore.Web.ViewModels;
using NLog;
using AutoMapperConfiguration = GameStore.Web.Infrastracture.AutoMapperConfiguration;

namespace GameStore.Web.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    [LoggingIpFilter]
    [PerformanceFilter]
    public class GamesController : Controller
    {
        private readonly IService _gameService;
        private readonly ILogger _logger;
        public GamesController(IService gameService, ILogger logger)
        {
            _logger = logger;
            _gameService = gameService;        
        }

        [HttpGet]
        public FileResult Download(string gameKey)
        {
            _logger.Info("Request to GamesController.Download. Parameters: gameKey = {0}", gameKey);
            try
            {
                _gameService.GetGameByKey(gameKey);
                string filePath = Server.MapPath("~/App_Data/Games/game.txt");
                string fileType = "application/text/plain";
                string fileName = "game.txt";
                var res = File(filePath, fileType, fileName);
                _logger.Info("Game is downloaded. Key = " + gameKey);
                return res;
            }
            catch (ValidationException)
            {
                _logger.Warn("Game downloading failed. Game wasn't found. Key = {0} ", gameKey);
                return null;
            }
            catch (IOException)
            {
                _logger.Warn("Game downloading failed. Cannot get game file. Key ={0}", gameKey);
                return null;
            }
        }
      
        [HttpGet]   
        public JsonResult Index()
        {
            var mapper = AutoMapperConfiguration.DtoToView().CreateMapper();
            try
            {           
                _logger.Info("Request to GamesController.Get");
                return Json(mapper.Map<IEnumerable<GameViewModel>>(_gameService.GetGames()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to load the gameService from GamesController failed : {0}",  ex.StackTrace);
                return new JsonResult();
            }
        }   
        
        [HttpPost]
        public HttpStatusCodeResult New(GameViewModel game)
        {
            try
            {
                var automapper = AutoMapperConfiguration.ViewToDto().CreateMapper();
                GameDTO gameDto = automapper.Map<GameViewModel, GameDTO>(game);
                _gameService.AddGame(gameDto);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to add new game with failed : {0}", ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _logger.Info("Game is created. Id {0} Key {1} ", game.Id, game.Key);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
       
        [HttpPost]
        public HttpStatusCodeResult Update(GameViewModel game)
        {
            _logger.Info("Request to GamesController.Update");
            var automapper = AutoMapperConfiguration.ViewToDto().CreateMapper();
            try
            {
                GameDTO gameDto = automapper.Map<GameViewModel, GameDTO>(game);
                _gameService.EditGame(gameDto);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to edit a game with Id {0} failed : {1}", game.Id, ex.StackTrace);
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
                _gameService.DeleteGame(game.Id);
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