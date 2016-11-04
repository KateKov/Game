﻿using System;
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
        public JsonResult Index(string key)
        {
            _logger.Info("Request to GameController.Index. Parameters: Key = {0}", key);
            try
            {
                return Json(Mapper.Map<GameDTO, GameViewModel>(_gameService.GetGameByKey(key)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }                
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
        public JsonResult Comments(string key)
        {
            _logger.Info("Request to CommentsController.GetGameComments. Parameters: gameKey = {0}", key);
            try
            {
                return Json(Mapper.Map<IEnumerable<CommentDTO>, IEnumerable<CommentViewModel>>(_gameService.GetCommentsByGameKey(key)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Mapper.Map<IEnumerable<CommentViewModel>>(_gameService.GetCommentsByGameKey(key)), JsonRequestBehavior.AllowGet);
            }
        }
    }
}