using System;
using System.Collections.Generic;
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
                return Json(Mapper.Map<IEnumerable<GameViewModel>>(_gameService.GetGames()), JsonRequestBehavior.AllowGet);
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
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(game);
                _gameService.AddGame(gameDto);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _logger.Info("Game is created. Id {0} Key {1} ", game.Id, game.Key);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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