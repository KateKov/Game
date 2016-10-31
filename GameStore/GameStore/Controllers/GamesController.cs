using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.ViewModels;
using NLog;
using GameStore.Filters;
using AutoMapperConfiguration = GameStore.Infrastracture.AutoMapperConfiguration;
using System.IO;
using System.Net;

namespace GameStore.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    public class GamesController : Controller
    {
        private readonly IService _gameService;

        private readonly ILogger _logger;

        public GamesController(IService gameService, ILogger logger)
        {
            this._logger = logger;
            this._gameService=gameService;
            
        }
        [HttpGet]
        [LoggingIpFilter]
        [PerformanceFilter]
        public FileResult Download(string gameKey)
        {
            _logger.Info($"Request to GamesController.Download. Parameters: gameKey = {gameKey}");
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
                _logger.Warn("Game downloading failed. Game wasn't found. Key = " + gameKey);
                return null;
            }
            catch (IOException)
            {
                _logger.Warn("Game downloading failed. Cannot get game file. Key = " + gameKey);
                return null;
            }
        }



      
        [HttpGet]
        [LoggingIpFilter]
        [PerformanceFilter]
        public JsonResult Index()
        {
            List<GameViewModel> games = new List<GameViewModel>();
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
        [LoggingIpFilter]
        [PerformanceFilter]
        public ActionResult New(GameViewModel game)
        {
            _logger.Info("Request to GamesController.New");
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
          
            return RedirectToAction("Index");
        }
       
       
        [HttpPost]
        [LoggingIpFilter]
        [PerformanceFilter]
        public ActionResult Update(GameViewModel game)
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
            return RedirectToAction("Index");
        }
       
       
        [HttpPost]
        [LoggingIpFilter]
        [PerformanceFilter]
        public ActionResult Remove(GameViewModel game)
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
            return RedirectToAction("Index");
        }

        

      
       
    }
}