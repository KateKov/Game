using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastracture;
using GameStore.BLL.Interfaces;
using GameStore.DAL.EF;
using GameStore.DAL.Entities;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentsRepository;

        private readonly IUnitOfWork _unitOfWork;
        private ILogger logger;
        private int _gameId;    
    
        public CommentService(ILogger logger)
        {
            this.logger = logger;
            _unitOfWork = new UnitOfWork();
            _commentsRepository = _unitOfWork.Repository<Comment>();

        }

        public CommentService(int gameId) : base()
        {
            _gameId = gameId;
        }

        public void AddComment(CommentDTO commentDto)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                Comment comment = Mapper.Map<CommentDTO, Comment>(commentDto);
                _commentsRepository.Add(comment);
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to add comment  to the game with Id {0} from CommentService failed: {1}", _gameId, ex.StackTrace);
            }

        }

        public List<CommentDTO> GetCommentsByGame(int gameId)
        {
            try
            {
                AutoMapperConfiguration.Configure();
                List<Comment> comments = _commentsRepository.GetAll().Where(x => x.Game.Id == gameId).ToList();
                List<CommentDTO> commentsDto = Mapper.Map<List<Comment>, List<CommentDTO>>(comments);
                return commentsDto;
            }
            catch (Exception ex)
            {
                logger.Error("The attempt to get comments by game Id {0} from CommentService failed: {1}", gameId, ex.StackTrace);
                return new List<CommentDTO>();
            }
        }
    }
}
