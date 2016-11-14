using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;


namespace GameStore.BLL.Services
{
    public class GameStoreService : IService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public GameStoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Methods for Entities

        private object AddEntities<T, TD>(T model, TD dtoModel) where T : class, IEntityBase, new() where TD: class, IDtoBase, new()
        {
            if (model != null && (model is Game).Equals(true))
            {
                var objGame =  (object) model;
                var objGameDto = (object) dtoModel;
                var game = (Game) objGame;
                var gameDto = (GameDTO) objGameDto;
                game.Comments =
                    _unitOfWork.Repository<Comment>().FindBy(x => gameDto.Comments.Contains(x.Id.ToString())).ToList();
                game.PlatformTypes =
                    _unitOfWork.Repository<PlatformType>().FindBy(x => gameDto.TypesName.Contains(x.Name)).ToList();
                game.Genres =
                    _unitOfWork.Repository<Genre>().FindBy(x => gameDto.GenresName.Contains(x.Name)).ToList();
                game.Publisher =
                    _unitOfWork.Repository<Publisher>().FindBy(x => gameDto.PublisherName==x.Name).FirstOrDefault();
                return game;
            }
            if (model != null && (model is Genre).Equals(true))
            {
                var objGenre = (object)model;
                var objGenreDto = (object)dtoModel;
                var genre = (Genre) objGenre;
                var genreDto = (GenreDTO)objGenreDto;
                genre.Games =
                    _unitOfWork.Repository<Game>().FindBy(x => genreDto.GamesKey.Contains(x.Key)).ToList();
                return genre;
            }
            if (model != null && (model is Comment).Equals(true))
            {
                var objComment = (object)model;
                var objCommentDto = (object)dtoModel;
                var comment = (Comment) objComment;
                var commentDto = (CommentDTO)objCommentDto;
                comment.Game =
                    _unitOfWork.Repository<Game>().FindBy(x => x.Key == commentDto.GameKey).FirstOrDefault();
                return comment;
            }
            if (model != null && (model is PlatformType).Equals(true))
            {
                var objType = (object)model;
                var objTypeDto = (object)dtoModel;
                var type = (PlatformType)objType;
                var typeDto = (PlatformTypeDTO)objTypeDto;
                type.Games =
                    _unitOfWork.Repository<Game>().FindBy(x => typeDto.GameKey.Contains(x.Key)).ToList();
                return type;
            }
            return null;
        }

        public void AddEntity<T, TD>(TD model) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            if (model == null)
                throw new ValidationException("Cannot find "+ typeof(T).Name, string.Empty);
            var entity = Mapper.Map<TD, T>(model);
            var result = (T) AddEntities(entity, model);
            //var name = (model is IDtoNamed).Equals(true) ? ((IDtoNamed)model).Name : "";
            //var key = (model is IDtoWithKey).Equals(true) ? ((IDtoWithKey)model).Key : "";
            //CheckName(key);

            _unitOfWork.Repository<T>().Add(result);
        }

        public void EditEntity<T, TD>(TD entity) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            if (entity == null)
                throw new ValidationException("Cannot find " + typeof(T).Name, string.Empty);
            _unitOfWork.Repository<T>().Edit(Mapper.Map<TD, T>(entity));
        }

        public IEnumerable<T> GetAllEntities<T>() where T : class, IEntityBase, new()
        {
            return _unitOfWork.Repository<T>().GetAll().ToList();
        }

        public T GetEntityByKey<T>(string key) where T : class, IEntityBase, IEntityWithKey, new()
        {
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Key.Equals(key)).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            return entity;
        }

        public T GetEntityById<T>(string id) where T : class, IEntityBase, new()
        {
            var entity = _unitOfWork.Repository<T>().GetSingle(Guid.Parse(id));
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            return entity;
        }

        private T GetEntityByName<T>(string name) where T : class, IEntityBase, IEntityNamed, new()
        {
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Name.Equals(name)).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            return entity;
        }

        public void DeleteEntityByName<T>(string name) where T : class, IEntityBase, IEntityNamed, new()
        {
            _unitOfWork.Repository<T>().Delete(GetEntityByName<T>(name));
        }

        public void DeleteEntityById<T>(string id) where T : class, IEntityBase, IEntityNamed, new()
        {
            _unitOfWork.Repository<T>().Delete(GetEntityById<T>(id));
        }
        #endregion

        #region Methods for Games

        public IEnumerable<GameDTO> GetGamesByGenreId(string genreId)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Id).ToList().Contains(Guid.Parse(genreId)))).ToList();
            _logger.Debug("Getting games by name of genre id {0}. Returned {1} games", genreId, gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByGenreName(string genreName)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Name).ToList().Contains(genreName))).ToList();
            _logger.Debug("Getting games by name of genre {0}. Returned {1} games", genreName, gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformTypeId(string platformType)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Id).ToList().Contains(Guid.Parse(platformType)))).ToList();
            _logger.Debug("Getting games by platform id {0}. Returned {1} games", platformType, gamesDto.Count);
            return gamesDto;
        }

        public IEnumerable<GameDTO> GetGamesByPlatformTypeName(string platformType)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Name).ToList().Contains(platformType))).ToList();
            _logger.Debug("Getting games by platform {0}. Returned {1} games", platformType, gamesDto.Count);
            return gamesDto;
        }
        #endregion

        #region Methods for Comments
        public void AddComment(CommentDTO commentDto, string gameKey)
        {
            Validator<CommentDTO>.ValidateModel(commentDto);
            if(string.IsNullOrEmpty(gameKey) || string.IsNullOrEmpty(commentDto.GameId) || String.IsNullOrEmpty(commentDto.Body))
                throw  new ValidationException("There is no game for comment", String.Empty);
            var comment = Mapper.Map<CommentDTO, Comment>(commentDto);
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Cannot find game for creating a comment", string.Empty);
            comment.Game = game;
            if (commentDto.ParentId != null)
            {
                var parentComment = _unitOfWork.Repository<Comment>().GetSingle(Guid.Parse(commentDto.ParentId));
                if (parentComment == null)
                    throw new ValidationException("Cannot find parent comment for creating a comment", string.Empty);
                comment.ParentComment = parentComment;
            }
            _unitOfWork.Repository<Comment>().Add(comment);
            _logger.Debug("Adding new comment with Author={0}, Id={1} to game with Key={2}", commentDto.Name,
                commentDto.Id, gameKey);
        }

        public IEnumerable<CommentDTO> GetCommentsByGameId(string gameId)
        {
            var commentsDto = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Id.Equals(gameId))).ToList();
            _logger.Debug("Getting comments by id = {0}. Retured {1} comments", gameId, commentsDto.Count);
            return commentsDto;
        }

        public IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey)
        {
            var commentsDto = Mapper.Map<IEnumerable<CommentDTO>>(_unitOfWork.Repository<Comment>().FindBy(
                comment => comment.Game.Key.Equals(gameKey))).ToList();
            _logger.Debug("Getting comments by key = {0}. Retured {1} comments", gameKey, commentsDto.Count);
            return commentsDto;
        }
        #endregion

        #region Generic Methods

        public T GetByKey<T>(string key) where T : class, IDtoBase, IDtoWithKey, new()
        {
            var entityType = GetEntityByDto<T>();
            var entity = Mapper.Map<T>(typeof(GameStoreService).GetMethod("GetEntityByKey")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { key }));
            if (entity == null)
            {
                throw new ValidationException(entityType.Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + entityType.Name + "by key={0} ", key);
            var gameDto = Mapper.Map<GameDTO>(entityType);
            return entity;
        }

        public T GetById<T>(string id) where T : class, IDtoBase, new()
        {
            var entityType = GetEntityByDto<T>();
            var entity = Mapper.Map<T>(typeof(GameStoreService).GetMethod("GetEntityById")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { id }));
            if (entity == null)
            {
                throw new ValidationException(entityType.Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + entityType.Name + "by id={0} ", id);
            var gameDto = Mapper.Map<GameDTO>(entityType);
            return entity;
        }

        public T GetByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new()
        {
            var entityType = GetEntityByDto<T>();
            var entity = Mapper.Map<T>(typeof(GameStoreService).GetMethod("GetEntityByName")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { name }));
            if (entity == null)
            {
                throw new ValidationException(entityType.Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + entityType.Name + "by name={0} ", name);
            var gameDto = Mapper.Map<GameDTO>(entityType);
            return entity;
        }

        public IEnumerable<T> GetAll<T>() where T : class, IDtoBase, new()
        {
            var entityType = GetEntityByDto<T>();
            var allDto = Mapper.Map<IEnumerable<T>>(typeof(GameStoreService).GetMethod("GetAllEntities")
                .MakeGenericMethod(entityType)
                .Invoke(this, null)).ToList();
            _logger.Debug("Getting all " + entityType.Name + "s. Returned {0}  " + entityType.Name + "s.", allDto.Count);
            return allDto;
        }

        public void AddOrUpdate<T>(T model, bool isAdding) where T : class, IDtoBase, new()
        {
            var action = isAdding ? "Add" : "Edit";
            Validator<T>.ValidateModel(model);
            var entityType = GetEntityByDto<T>();
            typeof(GameStoreService).GetMethod(action+"Entity")
                .MakeGenericMethod(entityType, typeof(T))
                .Invoke(this, new object[] { model });
            _logger.Debug(action+" " + typeof(T).Name + " with Id: {0}", model.Id);
        }

        public void DeleteByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new()
        {
            var entityType = GetEntityByDto<T>();
            typeof(GameStoreService).GetMethod("DeleteEntityByName")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { name });
            _logger.Debug("{0} deleting by Name = {1} ", typeof(T).Name, name);
        }

        public void DeleteById<T>(string id) where T : class, IDtoBase, IDtoNamed, new()
        {
            var entityType = GetEntityByDto<T>();
            typeof(GameStoreService).GetMethod("DeleteEntityById")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { id });
            _logger.Debug("{0} deleting by Id = {1} ", typeof(T).Name, id);
        }

        #endregion

        #region Generic Helpers
        //Finding in namespace GameStore.DAL.Entities class that is entity type for dto 
        private Type GetEntityByDto<T>() where T : class, IDtoBase, new()
        {
            var typeName = typeof(T).Name;
            var assembly = typeof(PlatformType).Assembly;
            var nameSpace = typeof(PlatformType).Namespace;
            var entityType = assembly.GetType(nameSpace + "." + typeName.Substring(0, typeName.Length - 3));
            return entityType;
        }

        public void CheckName<T>(string name, string id) where T : class, IEntityBase, IEntityNamed, new()
        {

            if (!string.IsNullOrEmpty(name)&&_unitOfWork.Repository<T>().FindBy(g => g.Name.Equals(name) && g.Id != Guid.Parse(id)).Any())
            {
                throw new ValidationException(typeof(T).Name + " with such name is already exists", string.Empty);
            }
        }

        public void CheckKey<T>(string key, string id) where T : class, IEntityBase, IEntityWithKey, new() 
        {
            if (!string.IsNullOrEmpty(key) && _unitOfWork.Repository<T>().FindBy(g => g.Key.Equals(key) && g.Id != Guid.Parse(id)).Any())
            {
                throw new ValidationException(typeof(T).Name + " with such key is already exists", string.Empty);
            }
        }

        
    
        #endregion

        #region Orders

        public OrderDTO GetOrder(string id)
        {
            decimal sum = 0;
            var order = _unitOfWork.Repository<Order>().GetSingle(Guid.Parse(id));
            if (order == null)
            {
                throw new ValidationException( "Order wasn't found", string.Empty);
            }
            _logger.Debug("Getting order by id={0} ", id);
            order.OrderDetails.ToList().ForEach(x=>sum+=x.Price);
            order.Sum = sum;
            return Mapper.Map<OrderDTO>(order);
        }

        public OrderDetailDTO GetOrderDetail(string gameId, short quantity, string customerId = "")
        {
            var game = GetById<GameDTO>(gameId);
            var orderDetail = new OrderDetailDTO()
            {
                Discount = 0,
                GameId = gameId,
                Price = game.Price,
                Quantity = quantity
            };
            AddOrUpdate<OrderDetailDTO>(orderDetail, true);
            return orderDetail;
        }

        public OrderDTO GetOrderByCustomer(string id)
        {
            var order = _unitOfWork.Repository<Order>().FindBy(x => x.CustomerId == Guid.Parse(id)).FirstOrDefault();
            if (order == null)
            {
                throw new ValidationException("Order wasn't found", string.Empty);
            }
            _logger.Debug("Getting order by Customer id={0} ", id);
            return Mapper.Map<OrderDTO>(order);
        }

        public OrderDTO ConfirmeOrder(string id)
        {
            var order = _unitOfWork.Repository<Order>().GetSingle(Guid.Parse(id));
            if (order == null)
            {
                throw new ValidationException("Order wasn't found", string.Empty);
            }
            _logger.Debug("Confirming order by id={0} ", id);
            order.IsConfirmed = true;
            _unitOfWork.Repository<Order>().Edit(order);
            return Mapper.Map<OrderDTO>(order);
        }

        public OrderDTO GetBusket(string customerId)
        {
            var basket = _unitOfWork.Repository<Order>().FindBy(x => x.CustomerId == Guid.Parse(customerId) && !x.IsConfirmed).FirstOrDefault();
            if (basket == null)
            {
                throw new ValidationException("Basket wasn't found", string.Empty);
            }
            _logger.Debug("Getting basket by Customer id={0} ", customerId);
            return Mapper.Map<OrderDTO>(basket);
        }

        public void DeleteBusket(string customerId)
        {
            var busket = _unitOfWork.Repository<Order>().FindBy(x => x.CustomerId == Guid.Parse(customerId) && !x.IsConfirmed).FirstOrDefault();
            if (busket == null)
            {
                throw new ValidationException("Busket wasn't found", string.Empty);
            }
            _unitOfWork.Repository<Order>().Delete(busket);
            _logger.Debug("Deleting busket by Customer id={0} ", customerId);
        }

        public IEnumerable<OrderDTO> GetOrders(string customerId)
        {
            var orders = _unitOfWork.Repository<Order>().FindBy(x => x.CustomerId == Guid.Parse(customerId) && x.IsConfirmed).ToList();
            if (orders == null)
            {
                throw new ValidationException("Busket wasn't found", string.Empty);
            }
            _logger.Debug("Getting order by Customer id={0} ", customerId);
            return Mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public void AddToBusket(OrderDetailDTO product, string customerId)
        {
            Validator<OrderDetailDTO>.ValidateModel(product);
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ValidationException("CustomerId is not valid for adding to busket", string.Empty);
            }
            var busket = _unitOfWork.Repository<Order>().FindBy(x => x.CustomerId == Guid.Parse(customerId) && !x.IsConfirmed).FirstOrDefault();
            if (busket == null)
            {
                var order = new OrderDTO() {Id =Guid.NewGuid().ToString()};
                var productOrder = product;
                productOrder.OrderId = order.Id;
                var orderDetail = productOrder;
   
                var orderEntity = Mapper.Map<Order>(order);
                _unitOfWork.Repository<Order>().Add(orderEntity);
                var orderDetailEntity = Mapper.Map<OrderDetail>(orderDetail);
                _unitOfWork.Repository<OrderDetail>().Add(orderDetailEntity);
            }
            else
            {
                var productEntity = Mapper.Map<OrderDetail>(product);
                productEntity.OrderId = busket.Id;
                busket.OrderDetails.Add(productEntity);
            }
            _logger.Debug("Adding product with Game {0} to the busket of Customer {1} ", product.GameId, customerId);
           
        }
        #endregion

    }
}
