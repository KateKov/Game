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
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IGameStoreService _gameService;
        private readonly ICommentService _commentService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public CommentsController(IGameStoreService service, ICommentService commentService)
        {
            _gameService = service;
            _commentService = commentService;
        }

        [HttpGet]        
        public ActionResult Details(string key)
        {
            _logger.Info("Request to GameController.Index. Parameters: Key = {0}", key);

            var gameDto = _gameService.KeyService<GameDTO>().GetByKey(key);
            if(gameDto==null)
                throw  new Exception();
            return View(Mapper.Map<GameDTO, GameViewModel>(gameDto));
        }

        [HttpGet]
        public ActionResult Download(string gameKey)
        {
            if (gameKey == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imgArray = new byte[100];
            _logger.Info("Game {0} was successfully downloaded", gameKey);
            return File(imgArray, "application/pdf", "test.pdf");
        }

        [HttpGet]
        public ActionResult NewComment(string gameKey, string parentId="",  string body="")
        {
            if (!string.IsNullOrEmpty(gameKey))
            {
                var qoute = body;
                var comment = new CommentViewModel() {Id = Guid.NewGuid().ToString(), Name = "", Body = "", GameKey = gameKey, ParentCommentId = parentId, Quote = qoute};
                return PartialView("NewComment", comment);
            }
            return RedirectToAction("Comments");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewComment(CommentViewModel newComment)
        {
            _logger.Info("Request to GameController.NewComment");
            if (ModelState.IsValid) { 
                var commentDto = Mapper.Map<CommentViewModel, CommentDTO>(newComment);
                commentDto.GameId = _gameService.KeyService<GameDTO>().GetByKey(commentDto.GameKey).Id;
               _commentService.AddComment(commentDto, newComment.GameKey);
                return PartialView("Success");
            }
            return PartialView("NewComment", newComment);
        }

        [HttpGet]
        public ActionResult Comments(string key)
        {
            ViewBag.GameKey = key;
            _logger.Info("Request to CommentsController.GetGameComments. Parameters: gameKey = {0}", key);
             return View(Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(_commentService.GetCommentsByGameKey(key)));
        }

        [HttpGet]
        public ActionResult Delete(string gameKey, string commentId)
        {
            if (!string.IsNullOrEmpty(gameKey) && !string.IsNullOrEmpty(commentId))
            {
                _gameService.GenericService<CommentDTO>().DeleteById(commentId);
                _logger.Info("Comments is deleted from game: CommentId={0} GameKey ={1} ", commentId, gameKey);
                return RedirectToAction("Comments");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult NewCommentWithQuote(string gameKey, string commentId)
        {
            if (!string.IsNullOrEmpty(gameKey) && !string.IsNullOrEmpty(commentId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Comments");
        }

        [HttpGet]
        public ActionResult Ban(string name)
        {
            var ban = new BanViewModel {Name = name};
            return View(ban);
        }

        [HttpPost]
        public ActionResult Ban(BanViewModel ban)
        {
            return PartialView("BanUser", ban);
        }
    }
}