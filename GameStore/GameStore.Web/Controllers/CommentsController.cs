using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.Web.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    public class CommentsController : Controller
    {
        private readonly IService _gameService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public CommentsController(IService service)
        {
            _gameService = service;
        }

        [HttpGet]        
        public ActionResult Details(string key)
        {
            _logger.Info("Request to GameController.Index. Parameters: Key = {0}", key);

            var gameDto = _gameService.GetByKey<GameDTO>(key);
            if(gameDto==null)
                throw  new Exception();
            return View(Mapper.Map<GameDTO, GameViewModel>(gameDto));
        }

        [HttpGet]
        public FileResult Download(string gameKey)
        {
            _logger.Info("Request to GamesController.Download. Parameters: gameKey = {0}", gameKey);
            try
            {
                _gameService.GetByKey<GameDTO>(gameKey);
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

        [HttpPost]
        public HttpStatusCodeResult NewComment(CommentViewModel newComment, string gameKey)
        {
            _logger.Info("Request to GameController.NewComment");
            try
            {
                CommentDTO commentDto = Mapper.Map<CommentViewModel, CommentDTO>(newComment);
               _gameService.AddComment(commentDto, gameKey);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult Comments(string key)
        {
            _logger.Info("Request to CommentsController.GetGameComments. Parameters: gameKey = {0}", key);
             return View(Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(_gameService.GetCommentsByGameKey(key)));
           
        }
    }
}