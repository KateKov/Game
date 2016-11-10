using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using GameStore.BLL.DTO;
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
    public class GameController : Controller
    {
        private readonly IService _gameService;    
        private readonly ILogger _logger;       
        public GameController(IService service, ILogger logger)
        {
            this._logger = logger;  
            _gameService = service;
        }

        [HttpGet]        
        public JsonResult Index(string key)
        {
            _logger.Info("Request to GameController.Index. Parameters: Key = {0}", key);
            try
            {
                var automapper = AutoMapperConfiguration.DtoToView().CreateMapper();
                return Json(automapper.Map<GameDTO, GameViewModel>(_gameService.GetGameByKey(key)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to load game with Key {0} by gameService from GameController failed : {1}", key, ex.StackTrace);
                return Json(null, JsonRequestBehavior.AllowGet);
            }                
        }
       
        [HttpPost]
        public HttpStatusCodeResult NewComment(CommentViewModel newComment, string gameKey)
        {
            _logger.Info("Request to GameController.NewComment");
            CommentDTO commentDto = new CommentDTO();
            var automapper = AutoMapperConfiguration.DtoToView().CreateMapper();
            try
            {
               commentDto = automapper.Map<CommentViewModel, CommentDTO>(newComment);
               _gameService.AddComment(commentDto, gameKey);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to add new comment to game with id {0} the gameService from GameController failed : {1}", newComment.GameId, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult Comments(string key)
        {
            _logger.Info("Request to CommentsController.GetGameComments. Parameters: gameKey = {0}", key);
            var automapper = AutoMapperConfiguration.DtoToView().CreateMapper();
            try
            {
                return Json(automapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(_gameService.GetCommentsByGameKey(key)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error(
                    "The attempt to load  comments by id {0} from gameService from GameController failed : {1}", key,
                    ex.StackTrace);
                return Json(automapper.Map<IEnumerable<CommentViewModel>>(_gameService.GetCommentsByGameKey(key)), JsonRequestBehavior.AllowGet);
            }
        }
    }
}