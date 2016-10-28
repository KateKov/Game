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

namespace GameStore.Controllers
{
    [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
    public class GameController : Controller
    {
        private ICommentService commentService;
        private IGameService gameService;
        private ILogger logger;
        
        public GameController(ILogger logger)
        {
            this.logger = logger;
            commentService = new CommentService(logger);
            gameService=new GameService();
        }
        
        public ActionResult Index(string Key)
        {
            GameViewModel game=new GameViewModel();
            try
            {
                GameDTO gameDto = gameService.GetGameByKey(Key);
               game = Mapper.Map<GameDTO, GameViewModel>(gameDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to load game with Key {0} by gameService from GameController failed : {1}", Key, ex.StackTrace);
            }
        
            return View(game);
        }
       
        public ActionResult NewComment(CommentViewModel newComment, CommentViewModel parrentComment = null)
        {
            CommentDTO commentDto = new CommentDTO();
            try
            {
               commentDto = Mapper.Map<CommentViewModel, CommentDTO>(newComment);
                commentService.AddComment(commentDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to add new comment to game with id {0} the gameService from GameController failed : {1}", newComment.GameId, ex.StackTrace);
            }
            return RedirectToAction("Index", commentDto.GameId);
        }
       
        public ActionResult Comments(int id)
        {
            List<CommentViewModel> comments=new List<CommentViewModel>();
            try
            {
                List<CommentDTO> commentsDto = commentService.GetCommentsByGame(id);
                comments = Mapper.Map<List<CommentDTO>, List<CommentViewModel>>(commentsDto);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to load  comments by id {0} from commentService from GameController failed : {1}", id, ex.StackTrace);
            }
            return View(comments);
        }
    }
}