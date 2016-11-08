using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using AutoMapper;
using AutoMapper.Mappers;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using NLog;
using GameStore.DAL.Infrastracture;
using Ninject.Infrastructure.Introspection;
using NLog.LayoutRenderers.Wrappers;
using System.Linq.Expressions;

namespace GameStore.BLL.Services
{
    public class GameService : IService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public GameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Methods for Entities

        public void AddEntity<T, TD>(TD entity) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            _unitOfWork.Repository<T>().Add(Mapper.Map<TD, T>(entity));
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

        public T GetEntityById<T>(int id) where T : class, IEntityBase, new()
        {
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Id.Equals(id)).FirstOrDefault();
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

        public void DeleteEntityById<T>(int id) where T : class, IEntityBase, IEntityNamed, new()
        {
            _unitOfWork.Repository<T>().Delete(GetEntityById<T>(id));
        }
        #endregion

        #region Methods for Games

        public void AddGame(GameDTO gameDto)
        {
            Validator<GameDTO>.ValidateModel(gameDto);
            if (_unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameDto.Key)).Any())
            {
                throw new ValidationException("Game with such key is already exists", string.Empty);
            }
            var game = Mapper.Map<GameDTO, Game>(gameDto);
            game.OrderDetails = GetAllInstansesOfType<OrderDetail, OrderDetailDTO>(gameDto.OrderDetails);
            game.Genres = GetAllInstansesOfType<Genre, GenreDTO>(gameDto.Genres);
            game.PlatformTypes = GetAllInstansesOfType<PlatformType, PlatformTypeDTO>(gameDto.PlatformTypes);
            _unitOfWork.Repository<Game>().Add(game);
            _logger.Debug("Adding game with Key={0}", gameDto.Key);
        }

        private Game GetGame(GameDTO gameDto)
        {
            var updatingGame = _unitOfWork.Repository<Game>().GetSingle(gameDto.Id);
            updatingGame.Comments = new List<Comment>();
            updatingGame.Genres = new List<Genre>();
            updatingGame.PlatformTypes = new List<PlatformType>();
            return updatingGame;
        }

        public void EditGame(GameDTO gameDto)
        {
            Validator<GameDTO>.ValidateModel(gameDto);
            var updatingGame = GetGame(gameDto);
            Mapper.Map(gameDto, updatingGame);
            updatingGame.Genres = GetAllInstansesOfType<Genre, GenreDTO>(gameDto.Genres);
            updatingGame.PlatformTypes = GetAllInstansesOfType<PlatformType, PlatformTypeDTO>(gameDto.PlatformTypes);
            updatingGame.OrderDetails = GetAllInstansesOfType<OrderDetail, OrderDetailDTO>(gameDto.OrderDetails);
            _unitOfWork.Repository<Game>().Edit(updatingGame);
            _logger.Debug("Game updating gameKey={0}, Id={1} ", gameDto.Key, gameDto.Key);
        }

        public IEnumerable<GameDTO> GetGamesByGenreId(int genreId)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.Genres.Select(
                    genre => genre.Id).ToList().Contains(genreId))).ToList();
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

        public IEnumerable<GameDTO> GetGamesByPlatformTypeId(int platformType)
        {
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(_unitOfWork.Repository<Game>().FindBy(
                game => game.PlatformTypes.Select(
                    platform => platform.Id).ToList().Contains(platformType))).ToList();
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
            var comment = Mapper.Map<CommentDTO, Comment>(commentDto);
            var game = _unitOfWork.Repository<Game>().FindBy(g => g.Key.Equals(gameKey)).FirstOrDefault();
            if (game == null)
                throw new ValidationException("Cannot find game for creating a comment", string.Empty);
            comment.Game = game;
            if (commentDto.ParentId != null)
            {
                var parentComment = _unitOfWork.Repository<Comment>().GetSingle((int)commentDto.ParentId);
                if (parentComment == null)
                    throw new ValidationException("Cannot find parent comment for creating a comment", string.Empty);
                comment.ParentComment = parentComment;
            }

            _unitOfWork.Repository<Comment>().Add(comment);
            _logger.Debug("Adding new comment with Author={0}, Id={1} to game with Key={2}", commentDto.Name,
                commentDto.Id, gameKey);
        }

        public IEnumerable<CommentDTO> GetCommentsByGameId(int gameId)
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
            var entity = Mapper.Map<T>(typeof(GameService).GetMethod("GetEntityByKey")
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

        public T GetById<T>(int id) where T : class, IDtoBase, new()
        {
            var entityType = GetEntityByDto<T>();
            var entity = Mapper.Map<T>(typeof(GameService).GetMethod("GetEntityById")
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
            var entity = Mapper.Map<T>(typeof(GameService).GetMethod("GetEntityByName")
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
            var allDto = Mapper.Map<IEnumerable<T>>(typeof(GameService).GetMethod("GetAllEntities")
                .MakeGenericMethod(entityType)
                .Invoke(this, null)).ToList();
            _logger.Debug("Getting all " + entityType.Name + "s. Returned {0}  " + entityType.Name + "s.", allDto.Count);
            return allDto;
        }

        public void Add<T>(T model) where T : class, IDtoBase, new()
        {
            Validator<T>.ValidateModel(model);
            var entityType = GetEntityByDto<T>();
            var nameParameter = (model is IDtoNamed).Equals(true) ? ((IDtoNamed)model).Name : "";
            var keyParameter = (model is IDtoWithKey).Equals(true) ? ((IDtoWithKey)model).Key : "";
            typeof(GameService).GetMethod("CheckEntityExisting")
               .MakeGenericMethod(entityType)
               .Invoke(this, new object[] { nameParameter, keyParameter });
            typeof(GameService).GetMethod("AddEntity")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { model });
            _logger.Debug("Adding " + typeof(T).Name + " with Id: {0}", model.Id);
        }

        public void DeleteByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new()
        {
            var entityType = GetEntityByDto<T>();
            typeof(GameService).GetMethod("DeleteEntityByName")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { name });
            _logger.Debug("{0} deleting by Name = {1} ", typeof(T).Name, name);
        }

        public void DeleteById<T>(int id) where T : class, IDtoBase, IDtoNamed, new()
        {
            var entityType = GetEntityByDto<T>();
            typeof(GameService).GetMethod("DeleteEntityById")
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
        //Checking name and key for unique
        private void CheckEntityExisting<T>(string name = "", string key = "") where T : class, IEntityBase, new()
        {
            if ((new T() is IEntityNamed).Equals(true) && _unitOfWork.Repository<T>().FindBy(g => (g as IEntityNamed).Name.Equals(name)).Any())
            {
                throw new ValidationException(typeof(T).Name + " with such name is already exists", string.Empty);
            }
            if ((new T() is IEntityWithKey).Equals(true) && _unitOfWork.Repository<T>().FindBy(g => (g as IEntityWithKey).Key.Equals(key)).Any())
            {
                throw new ValidationException(typeof(T).Name + " with such key is already exists", string.Empty);
            }
        }
        private ICollection<T> GetAllInstansesOfType<T, TD>(ICollection<TD> types) where TD : class, IDtoBase, new()
          where T : class, IEntityBase, new()
        {
            var type = typeof(T).Name;
            var allTypes = _unitOfWork.Repository<T>().GetAll().ToList();
            var newTypes = new List<T>();
            if (allTypes.Count > 0)
            {
                newTypes = types
                    .Where(x => allTypes
                        .Exists(g => g.Id.Equals(x.Id)))
                    .Select(x => allTypes
                        .First(g => g.Id.Equals(x.Id))).ToList();
                if (newTypes.Count != types.Count)
                    throw new ValidationException(@"Cannot find " + type, string.Empty);
            }
            else if (types != null && types.Count > 0)
                throw new ValidationException("Cannot find " + type + ". There are no " + type + " exists", string.Empty);
            return newTypes;
        }
        #endregion
    }
}
