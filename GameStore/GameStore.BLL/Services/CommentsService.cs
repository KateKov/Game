using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class CommentsService : Service<Comment, CommentDTO>, ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public CommentsService(IUnitOfWork unitOfWork, IUserService userService) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddEntity(CommentDTO commentDto, string gameKey)
        {
            Validator.Validate(commentDto);
            if (string.IsNullOrEmpty(gameKey) || string.IsNullOrEmpty(commentDto.GameId) ||
                string.IsNullOrEmpty(commentDto.Body))
            {
                throw new ValidationException("There is no game for comment", string.Empty);
            }

            var comment = Mapper.Map<CommentDTO, Comment>(commentDto);
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();
            if (game == null)
            {
                throw new ValidationException("Cannot find game for creating a comment", string.Empty);
            }

            comment.Game = game;

            if (commentDto.ParentCommentId != null)
            {
                var parentComment = _unitOfWork.Repository<Comment>().GetSingle(commentDto.ParentCommentId);
                if (parentComment == null)
                {
                    throw new ValidationException("Cannot find parent comment for creating a comment", string.Empty);
                }

                comment.ParentComment = parentComment;
            }
            _unitOfWork.Repository<Comment>().Add(comment);
            _logger.Debug("Adding new comment with Author={0}, Id={1}, ParentId={2} to game with Key={3}",
                commentDto.Name,
                commentDto.Id, commentDto.ParentCommentId, gameKey);
        }

        public void EditEntity(CommentDTO commentDto, string gameKey)
        {
            Validator.Validate(commentDto);
            if (string.IsNullOrEmpty(gameKey) || string.IsNullOrEmpty(commentDto.GameId) ||
                string.IsNullOrEmpty(commentDto.Body))
            {
                throw new ValidationException("There is no game for comment", String.Empty);
            }

            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();
            if (game == null)
            {
                throw new ValidationException("Cannot find game for creating a comment", string.Empty);
            }
            var commentEntity = _unitOfWork.Repository<Comment>().GetSingle(commentDto.Id);
            commentEntity.Name = commentDto.Name;
            commentEntity.Body = commentDto.Body;
            commentEntity.Game = game;
            commentEntity.Quote = commentDto.Quote;

            if (commentDto.ParentCommentId != null)
            {
                var parentComment = _unitOfWork.Repository<Comment>().GetSingle(commentDto.ParentCommentId);
                if (parentComment == null)
                {
                    throw new ValidationException("Cannot find parent comment for creating a comment", string.Empty);
                }

                commentEntity.ParentComment = parentComment;
            }
            _unitOfWork.Repository<Comment>().Edit(commentEntity);
            _logger.Debug("Eding comment with Author={0}, Id={1}, ParentId={2} to game with Key={3}",
                commentDto.Name,
                commentDto.Id, commentDto.ParentCommentId, gameKey);
        }

        public IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey)
        {
            var commentsDto = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Key.Equals(gameKey)).Where(x=>!x.IsDeleted)).ToList();
            _logger.Debug("Getting comments by key = {0}. Retured {1} comments", gameKey, commentsDto.Count);
            return commentsDto;
        }
    }
}