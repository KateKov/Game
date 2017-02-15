using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Accounts;
using GameStore.Web.ViewModels.Comments;
using GameStore.Web.ViewModels.Games;
using NLog;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;

namespace GameStore.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public CommentsController(IGameService game, ICommentService commentService,
            IUserService userService, IAuthenticationManager authentication) : base(authentication)
        {
            _gameService = game;
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Details(string key)
        {
            _logger.Info("Request to GameController.Index. Parameters: Key = {0}", key);

            var gameDto = _gameService.GetByKey(key);
            if (gameDto == null)
            {
                throw new ValidationException("The game doesn't exist in database", string.Empty);
            }

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
        public ActionResult NewComment(string gameKey, string parentId = "", string body = "")
        {
            if (!string.IsNullOrEmpty(gameKey))
            {
                var qoute = body;
                var comment = new CommentViewModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = (User.IsInRole("Guest"))? "Anonumous" : User.Identity.Name,
                    Body = "",
                    GameKey = gameKey,
                    ParentCommentId = parentId,
                    Quote = qoute
                };

                return PartialView("NewComment", comment);
            }
            return RedirectToAction("Comments");
        }

        [HttpPost]
        public ActionResult NewComment(CommentViewModel newComment)
        {
            _logger.Info("Request to GameController.NewComment");
            if (ModelState.IsValid)
            {
                var commentDto = Mapper.Map<CommentViewModel, CommentDTO>(newComment);
                commentDto.GameId = _gameService.GetByKey(commentDto.GameKey).Id;
                if (_commentService.IsExist(newComment.Id))
                {
                    _commentService.EditEntity(commentDto, newComment.GameKey);
                }
                else
                {
                    _commentService.AddEntity(commentDto, newComment.GameKey);
                }

                return PartialView("Success");
            }
            return PartialView("NewComment", newComment);
        }

        [HttpGet]
        public ActionResult Comments(string key)
        {
            ViewBag.GameKey = key;
            _logger.Info("Request to CommentsController.GetGameComments. Parameters: gameKey = {0}", key);
            return
                PartialView(
                    Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(
                        _commentService.GetCommentsByGameKey(key)));
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Moderator)]
        public ActionResult Delete(string key, string commentId)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(commentId))
            {
                var commentDto = _commentService.GetById(commentId);
                var comment = Mapper.Map<CommentViewModel>(commentDto);
                return PartialView("Delete", comment);
            }
            return View("Comments");
        }


        [HttpPost]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Moderator)]
        [ActionName("Delete")]
        public ActionResult DeleteComment(string key, string commentId)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(commentId))
            {
                _commentService.DeleteById(commentId);
                _logger.Info("Comments is deleted from game: CommentId={0} GameKey ={1} ", commentId, key);
                var comments =
                    Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(
                        _commentService.GetCommentsByGameKey(key));
                return PartialView("Comments", comments);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Moderator)]
        public ActionResult Edit(string gameKey, string commentId)
        {
            if (!string.IsNullOrEmpty(gameKey) && !string.IsNullOrEmpty(commentId))
            {
                var comment = Mapper.Map<CommentViewModel>(_commentService.GetById(commentId));
                return PartialView("NewComment", comment);
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
        [UserAuthorize(Roles = UserRole.Administrator)]
        public ActionResult Ban(string name)
        {
            var ban = new BanViewModel {Name = name};
            return View(ban);
        }

        [HttpPost]
        [UserAuthorize(Roles = UserRole.Administrator | UserRole.Moderator)]
        public ActionResult Ban(BanViewModel ban)
        {
            _userService.Ban(ban.Name, "", ban.Duration);
            return RedirectToAction("Index", "Games", new FilterViewModel());
        }
    }
}