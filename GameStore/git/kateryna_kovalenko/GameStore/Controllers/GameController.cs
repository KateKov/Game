using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services;
using GameStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using System.Web.UI;
using GameStore.Filters;
using GameStore.BLL.Infrastructure;
using AutoMapperConfiguration = GameStore.Infrastracture.AutoMapperConfiguration;
using System.Net;

namespace GameStore.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    public class GameController : Controller
    {
        private readonly IService _gameService;
       
        private readonly ILogger _logger;
        
        public GameController(IService service, ILogger logger)
        {
            this._logger = logger;  
            _gameService=service;
        }
        [HttpGet]
        [LoggingIpFilter]
        [PerformanceFilter]    
        public JsonResult Index(string Key)
        {
            _logger.Info($"Request to GameController.Index. Parameters: Key = {Key}");
            GameViewModel game=new GameViewModel();
            try
            {
                var automapper = Infrastracture.AutoMapperConfiguration.DtoToView().CreateMapper();
                return Json(automapper.Map<GameViewModel>(_gameService.GetGameByKey(Key)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error($"The attempt to load game with Key {0} by gameService from GameController failed : {1}", Key, ex.StackTrace);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        
            
        }
       
       
        [HttpPost]
        [LoggingIpFilter]
        [PerformanceFilter]
        public HttpStatusCodeResult NewComment(CommentViewModel newComment, string gameKey)
        {
            _logger.Info($"Request to GameController.NewComment");
            CommentDTO commentDto=new CommentDTO();
            var automapper = AutoMapperConfiguration.DtoToView().CreateMapper();
            try
            {
               commentDto = automapper.Map<CommentViewModel, CommentDTO>(newComment);
               _gameService.AddComment(commentDto,gameKey );
            }
            catch (Exception ex)
            {
                _logger.Error($"The attempt to add new comment to game with id {0} the gameService from GameController failed : {1}", newComment.GameId, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpGet]
        [LoggingIpFilter]
        [PerformanceFilter]
        public JsonResult Comments(int id)
        {
            _logger.Info($"Request to CommentsController.GetGameComments. Parameters: gameId = {id}");
            var automapper = AutoMapperConfiguration.DtoToView().CreateMapper();
            List<CommentViewModel> comments=new List<CommentViewModel>();
            try
            {
                return Json(automapper.Map<IEnumerable<CommentViewModel>>(_gameService.GetCommentsByGame(id)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error("The attempt to load  comments by id {0} from commentService from GameController failed : {1}", id, ex.StackTrace);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}