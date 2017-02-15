using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
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

        private List<Genre> GetMongoGenres(GameDTO gameDto, List<Genre> genres)
        {
            var genre = genres;
            var mongogenres = _unitOfWork.Repository<Genre>().FindBy(x => x.EntityId == Guid.Empty);
            foreach (var item in mongogenres)
            {
                var genreTranslate = item.Translates.First();
                if (gameDto.Translates.Any(x => x.GenresName.Any(t => t == genreTranslate.Name)))
                {
                    if (!genre.Any(x => x.Translates.Any(t => t.Name.Contains(genreTranslate.Name))))
                    {
                        item.EntityId = Guid.NewGuid();
                        genre.Add(item);
                        _unitOfWork.Repository<Genre>().Add(item);
                    }
                }
            }
            return genre;
        }

        private List<Genre> GetGenres(GameDTO gameDto)
        {
            var genresTranslates =
                _unitOfWork.Repository<GenreTranslate>()
                    .GetAll().Where(x => gameDto.Translates.Any(t => t.GenresName.Any(z=>z == x.Name)))
                    .ToList();
            var genres = new List<Genre>();
            foreach (var item in genresTranslates)
            {
                var entity = _unitOfWork.Repository<Genre>().GetSingle(item.BaseEntityId.ToString());
                if (genres.FirstOrDefault(x => x.EntityId == entity.EntityId) == null)
                {
                    genres.Add(entity);
                }
            }

            var genre = GetMongoGenres(gameDto, genres);
            return genre;
        }

        private Game GetGameWithPublisher(GameDTO gameDto)
        {
            var game = new Game();
            var publishers =
                _unitOfWork.Repository<Publisher>()
                    .GetAll()
                    .Where(x => gameDto.Translates.Any(z => x.Translates.Any(t => t.Name.Contains(z.PublisherName))))
                    .ToList();
            if (publishers.Count>0 && publishers.FirstOrDefault(x => x.EntityId != Guid.Empty) == null)
            {
                var mongoPublisher = publishers.First();
                mongoPublisher.EntityId = Guid.NewGuid();
                game.Publisher = mongoPublisher;
                game.PublisherId = mongoPublisher.EntityId;
                _unitOfWork.Repository<Publisher>().Add(mongoPublisher);
            }
            else
            {
                game.Publisher = publishers.FirstOrDefault(x => x.EntityId != Guid.Empty);
                game.PublisherId = (publishers.FirstOrDefault(x => x.EntityId != Guid.Empty)!=null)?publishers.First(x=>x.EntityId!=Guid.Empty).EntityId:new Guid();
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
                var item in _unitOfWork.Repository<PlatformTypeTranslate>().GetAll().Where(x=>gameDto.Translates.Any(t=>t.PlatformTypesName.Any(z=>z==x.Name))).ToList())
            {
                var entity = _unitOfWork.Repository<PlatformType>().GetSingle(item.BaseEntityId.ToString());
                if (game.PlatformTypes.FirstOrDefault(x => x.EntityId == entity.EntityId) == null)
                {
                    game.PlatformTypes.Add(entity);
                }
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
