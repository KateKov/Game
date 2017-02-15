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
    public class DtoToDomainMapping : IDtoToDomainMapping
    {
        private readonly IUnitOfWork _unitOfWork;

        public DtoToDomainMapping(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object AddEntities<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            if (model != null)
            {
                switch (typeof(T).Name)
                {
                    case "Game":
                        return GetGames(model, dtoModel);
                    case "Genre":
                        return GetGenres(model, dtoModel);
                    case "OrderDetail":
                        return GetOrderDetails(model, dtoModel);
                    case "Comment":
                        return GetComments(model, dtoModel);
                    case "PlatformType":
                        return GetTypes(model, dtoModel);
                    case "User":
                        return GetUser(model, dtoModel);
                    case "Role":
                        return GetRole(model, dtoModel);
                }
            }

            return model;
        }

        public IEnumerable<Genre> GetMongoGenres(GameDTO gameDto, List<Genre> genres)
        {
            var genre = genres;
            var mongogenres = _unitOfWork.Repository<Genre>().FindBy(x => x.Id == Guid.Empty);
            foreach (var item in mongogenres)
            {
                var genreTranslate = item.Translates.First();
                if (gameDto.Translates.Any(x => x.GenresName.Any(t => t == genreTranslate.Name)) &&
                    !genre.Any(x => x.Translates.Any(t => t.Name.Contains(genreTranslate.Name))))
                {
                    item.Id = Guid.NewGuid();
                    genre.Add(item);
                    _unitOfWork.Repository<Genre>().Add(item);
                }
            }
            return genre;
        }

        public IEnumerable<Genre> GetGenres(GameDTO gameDto)
        {
            var genresTranslates =
                _unitOfWork.Repository<GenreTranslate>()
                    .GetAll().Where(x => gameDto.Translates.Any(t => t.GenresName.Any(z => z == x.Name)))
                    .ToList();
            var genres = new List<Genre>();
           foreach (var item in genresTranslates)
            {
                var entity = _unitOfWork.Repository<Genre>().GetSingle(item.BaseEntityId.ToString());
                if (genres.FirstOrDefault(x => x.Id == entity.Id) == null)
                {
                    genres.Add(entity);
                }
            }

            var genre = GetMongoGenres(gameDto, genres);
            return genre;
        }

        public Game GetGameWithPublisher(GameDTO gameDto)
        {
            var game = new Game();
            var publishers =
                _unitOfWork.Repository<Publisher>()
                    .GetAll()
                    .Where(x => gameDto.Translates.Any(z => x.Translates.Any(t => t.Name.Contains(z.PublisherName))))
                    .ToList();
            if (publishers.Count > 0 && publishers.FirstOrDefault(x => x.Id != Guid.Empty) == null)
            {
                var mongoPublisher = publishers.First();
                mongoPublisher.Id = Guid.NewGuid();
                game.Publisher = mongoPublisher;
                game.PublisherId = mongoPublisher.Id;
                _unitOfWork.Repository<Publisher>().Add(mongoPublisher);
            }
            else
            {
                game.Publisher = publishers.FirstOrDefault(x => x.Id != Guid.Empty);
                game.PublisherId = null;
                game.PublisherId = (publishers.FirstOrDefault(x => x.Id != Guid.Empty)!=null)?
                    publishers.First(x=>x.Id!=Guid.Empty).Id:game.PublisherId;
            }

            return game;
        }

        public object GetGames<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objGame = (object) model;
            var objGameDto = (object) dtoModel;
            var game = (Game) objGame;
            var gameDto = (GameDTO) objGameDto;
            game.Comments =
                _unitOfWork.Repository<Comment>()
                    .GetAll(x => gameDto.Comments != null && gameDto.Comments.Contains(x.Id.ToString()))
                    .ToList();
            game.PlatformTypes = new List<PlatformType>();
            foreach (
                var item in
                _unitOfWork.Repository<PlatformTypeTranslate>()
                    .GetAll()
                    .Where(x => gameDto.Translates.Any(t => t.PlatformTypesName.Any(z => z == x.Name)))
                    .ToList())
            {
                var entity = _unitOfWork.Repository<PlatformType>().GetSingle(item.BaseEntityId.ToString());
                if (game.PlatformTypes.FirstOrDefault(x => x.Id == entity.Id) == null)
                {
                    game.PlatformTypes.Add(entity);
                }
            }

            game.Genres = GetGenres(gameDto).ToList();
            var gameWithPublisher = GetGameWithPublisher(gameDto);
            game.Publisher = gameWithPublisher.Publisher;
            game.PublisherId = gameWithPublisher.PublisherId;
            return game;
        }

        public object GetGenres<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objGenre = (object) model;
            var objGenreDto = (object) dtoModel;
            var genre = (Genre) objGenre;
            var genreDto = (GenreDTO) objGenreDto;
            genre.ParentGenre =
                _unitOfWork.Repository<Genre>()
                    .FindBy(x => x.Translates.Any())
                    .FirstOrDefault(x => x.Translates.Any(z => genreDto.Translates.Any(t => t.ParentName == z.Name)));

            return genre;
        }

        public object GetTypes<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objType= (object) model;
            var type = (PlatformType) objType;
            return type;
        }

        public object GetComments<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objComment = (object) model;
            var objCommentDto = (object) dtoModel;
            var comment = (Comment) objComment;
            var commentDto = (CommentDTO) objCommentDto;
            comment.Game =
                _unitOfWork.Repository<Game>().FindBy(x => x.Key == commentDto.GameKey).FirstOrDefault();
            return comment;
        }

        public object GetOrderDetails<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objOrderDetail = (object) model;
            var objOrderDetailDto = (object) dtoModel;
            var orderDetail = (OrderDetail) objOrderDetail;
            var orderDetailDto = (OrderDetailDTO) objOrderDetailDto;
            orderDetail.Game =
                _unitOfWork.Repository<Game>().GetSingle(orderDetailDto.GameId);
            if (orderDetailDto.OrderId != null)
            {
                orderDetail.Order =
                    _unitOfWork.Repository<Order>().GetSingle(orderDetailDto.OrderId);
            }
            return orderDetail;
        }

        public object GetUser<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
         where TD : class, IDtoBase, new()
        {
            var objUser = (object)model;
            var objUserDto = (object)dtoModel;
            var user = (User)objUser;
            var userDto = (UserDTO)objUserDto;
            user.Comments = _unitOfWork.Repository<Comment>()
                    .FindBy(x => userDto.Comments.Contains(x.Id.ToString()))
                    .ToList();
            user.Orders =
                _unitOfWork.Repository<Order>().FindBy(x => userDto.Orders.Contains(x.Id.ToString())).ToList();
            if (user.ManagerProfile != null)
            {
                user.ManagerProfile =_unitOfWork.Repository<ManagerProfile>().GetSingle(userDto.Id);
            }
            user.Roles =
                _unitOfWork.Repository<Role>()
                    .FindBy(x => userDto.Roles.Contains(x.Id.ToString())).ToList();
            user.Bans = _unitOfWork.Repository<Ban>().FindBy(x => userDto.Bans.Contains(x.Id.ToString())).ToList();
            return user;
        }

        public object GetRole<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new()
            where TD : class, IDtoBase, new()
        {
            var objRole = (object)model;
            var objRoleDto = (object)dtoModel;
            var role = (Role)objRole;
            var roleDto = (RoleDTO)objRoleDto;
            role.Users =
                _unitOfWork.Repository<User>().FindBy(x => roleDto.Users!=null).Where(x=>roleDto.Users.Contains(x.Id.ToString())).ToList();
            return role;
        }
    }
}