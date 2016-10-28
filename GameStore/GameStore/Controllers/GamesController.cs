using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.ViewModels;
using NLog;

namespace GameStore.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    public class GamesController : Controller
    {
        private IGameService gameService;

        private ILogger logger;

        public GamesController(ILogger logger)
        {
            this.logger = logger;
            gameService=new GameService();
            
        }
        // GET: Games
      
        public ActionResult Index()
        {
            List<GameViewModel> games = new List<GameViewModel>();

            try
            {
                List<GameDTO> gamesDto = gameService.GetGames();
                games = Mapper.Map<List<GameDTO>, List<GameViewModel>>(gamesDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to load the gameService from GamesController failed : {0}",  ex.StackTrace);
                
            }

            return View(games);
        }


        public ActionResult New(GameViewModel game)
        {
            try
            {
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(game);
                gameService.AddGame(gameDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to add new game with failed : {0}", ex.StackTrace);

            }

            return RedirectToAction("Index");
        }

        public ActionResult Update(GameViewModel game)
        {
            try
            {
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(game);
                gameService.EditGame(gameDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to edit a game with Id {0} failed : {1}", game.Id, ex.StackTrace);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Remove(GameViewModel game)
        {
            try
            {
                GameDTO gameDto = Mapper.Map<GameViewModel, GameDTO>(game);
                gameService.DeleteGame(gameDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to remove a game with Id {0} failed : {1}", game.Id, ex.StackTrace);
            }
            return RedirectToAction("Index");
        }

        

      
       
    }
}