using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System;
using System.Linq;

namespace GameStore.BLL.Infrastructure
{
    public class DtoToDomain
    {
        private readonly IUnitOfWork _unitOfWork;
   
        public DtoToDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private object GetGames<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objGame = (object)model;
            var objGameDto = (object)dtoModel;
            var game = (Game)objGame;
            var gameDto = (GameDTO)objGameDto;
            game.Comments =
                _unitOfWork.Repository<Comment>()
                    .FindBy(x => gameDto.Comments.Contains(x.Id.ToString()))
                    .ToList();
            game.PlatformTypes =
                _unitOfWork.Repository<PlatformType>().FindBy(x => gameDto.PlatformTypesName.Contains(x.Name)).ToList();
            game.Genres =
                _unitOfWork.Repository<Genre>().FindBy(x => gameDto.GenresName.Contains(x.Name)).ToList();
            game.PublisherId =
                _unitOfWork.Repository<Publisher>()
                    .FindBy(x => gameDto.PublisherName == x.Name)
                    .FirstOrDefault()
                    .Id;
            return game;
        }

        private object GetGenres<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
           where TD : class, IDtoBase, new()
        {
            var objGenre = (object)model;
            var objGenreDto = (object)dtoModel;
            var genre = (Genre)objGenre;
            var genreDto = (GenreDTO)objGenreDto;
            genre.Games =
                _unitOfWork.Repository<Game>().FindBy(x => genreDto.GamesKey.Contains(x.Key)).ToList();
            return genre;
        }

        private object GetTypes<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
          where TD : class, IDtoBase, new()
        {
            var objComment = (object)model;
            var objCommentDto = (object)dtoModel;
            var comment = (Comment)objComment;
            var commentDto = (CommentDTO)objCommentDto;
            comment.Game =
                _unitOfWork.Repository<Game>().FindBy(x => x.Key == commentDto.GameKey).FirstOrDefault();
            return comment;
        }

        private object GetComments<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
          where TD : class, IDtoBase, new()
        {
            var objComment = (object)model;
            var objCommentDto = (object)dtoModel;
            var comment = (Comment)objComment;
            var commentDto = (CommentDTO)objCommentDto;
            comment.Game =
                _unitOfWork.Repository<Game>().FindBy(x => x.Key == commentDto.GameKey).FirstOrDefault();
            return comment;
        }

        private object GetOrderDetails<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
         where TD : class, IDtoBase, new()
        {
            var objOrderDetail = (object)model;
            var objOrderDetailDto = (object)dtoModel;
            var orderDetail = (OrderDetail)objOrderDetail;
            var orderDetailDto = (OrderDetailDTO)objOrderDetailDto;
            orderDetail.Game =
                _unitOfWork.Repository<Game>().GetSingle(Guid.Parse(orderDetailDto.GameId));
            orderDetail.Order =
                _unitOfWork.Repository<Order>().GetSingle(Guid.Parse(orderDetailDto.OrderId));
            return orderDetail;
        }

        public object AddEntities<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            if (model != null)
            {
                if ((model is Game).Equals(true))
                {
                    return GetGames(model, dtoModel);
;                }
                if ((model is Genre).Equals(true))
                {
                    return GetGenres(model, dtoModel);
;                }
                if ((model is Comment).Equals(true))
                {
                    return GetComments(model, dtoModel);
                }
                if ((model is PlatformType).Equals(true))
                {
                    return GetTypes(model, dtoModel);
                }
                if ((model is OrderDetail).Equals(true))
                {
                    return GetOrderDetails(model, dtoModel);
;                }
            }
            return model;
        }
    }
}
