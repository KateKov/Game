using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Infrastructure
{
    public class DtoToDomain
    {
        private readonly IUnitOfWork _unitOfWork;
   
        public DtoToDomain(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private List<Genre> GetGenres(GameDTO gameDto)
        {
            var genres = _unitOfWork.Repository<Genre>().FindBy(x => gameDto.Translates.Where(z=>z.Language==gameDto.Translates.First().Language).Any(z=>z.GenresName.Contains(x.Translates.First(t => t.Language == z.Language).Name))).ToList();
            var genre = genres;
            genre = genre.Where(x => x.EntityId != Guid.Empty).ToList();
            var mongogenres = genres.Where(x => x.EntityId == Guid.Empty && !genre.Any(z => z.Translates.First(y=>y.Language==Language.En).Name.Contains(x.Translates.First(t=>t.Language==Language.En).Name))).ToList();
            mongogenres.ForEach(x => x.EntityId = Guid.NewGuid());
            mongogenres.ForEach(x => genre.Add(x));
            mongogenres.ForEach(x => _unitOfWork.Repository<Genre>().Add(x));
            return genre;
        }

        private Game GetGameWithPublisher(GameDTO gameDto)
        {
            var game = new Game();
            var publishers =
               _unitOfWork.Repository<Publisher>().FindBy(x => gameDto.Translates.Where(z=>z.Language==gameDto.Translates.First().Language).Any(z=>z.PublisherName.Contains(x.Translates.First(t=>t.Language==z.Language).Name))).ToList();
            if (publishers.FirstOrDefault(x => x.EntityId != Guid.Empty) == null)
            {
                var mongoPublisher = publishers.First();
                mongoPublisher.EntityId = Guid.NewGuid();
                game.Publisher = mongoPublisher;
                game.PublisherId = mongoPublisher.EntityId;
                _unitOfWork.Repository<Publisher>().Add(mongoPublisher);
            }
            else
            {
                game.Publisher = publishers.First(x => x.EntityId != Guid.Empty);
                game.PublisherId = publishers.First(x => x.EntityId != Guid.Empty).EntityId;
            }
            return game;
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
                    .FindBy(x => gameDto.Comments.Contains(x.EntityId.ToString()))
                    .ToList();
            game.PlatformTypes = new List<PlatformType>();
            foreach (
                var item in
                _unitOfWork.Repository<PlatformType>().FindBy(x => gameDto.Translates.Where(z => z.Language == gameDto.Translates.First().Language).Any(z => z.PublisherName.Contains(x.Translates.First(t => t.Language == z.Language).Name))).ToList())
            {
                game.PlatformTypes.Add(item);
            }

            game.Genres = GetGenres(gameDto);
            var gameWithPublisher = GetGameWithPublisher(gameDto);
            game.Publisher = gameWithPublisher.Publisher;
            game.PublisherId = gameWithPublisher.PublisherId;
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
                _unitOfWork.Repository<Game>().GetSingle(orderDetailDto.GameId);
            orderDetail.Order =
                _unitOfWork.Repository<Order>().GetSingle(orderDetailDto.OrderId);
            return orderDetail;
        }

        public object AddEntities<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            if (model != null)
            {
                if ((model is Game).Equals(true))
                {
                    return (Game) GetGames(model, dtoModel);
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
