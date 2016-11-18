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
        private readonly DtoToDomain _dtoToDomain;

        public GameStoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomain(unitOfWork);
        }

        #region Methods for Entities

        public void AddEntity<T, TD>(TD model) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            if (model == null)
                throw new ValidationException("Cannot find "+ typeof(T).Name, string.Empty);
            var entity = Mapper.Map<TD, T>(model);
            var result = (T) _dtoToDomain.AddEntities(entity, model);
            _unitOfWork.Repository<T>().Add(result);
        }

        public void EditEntity<T, TD>(TD model) where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
        {
            if (model == null)
                throw new ValidationException("Cannot find " + typeof(T).Name, string.Empty);
            var entity = Mapper.Map<TD, T>(model);
            var result = (T) _dtoToDomain.AddEntities(entity, model);
            _unitOfWork.Repository<T>().Edit(result);
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

        public T GetEntityByName<T>(string name) where T : class, IEntityBase, IEntityNamed, new()
        {
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Name.Equals(name)).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            return entity;
        }

        #endregion

        #region Generic Methods

        public T GetByKey<T>(string key) where T : class, IDtoBase, IDtoWithKey, new()
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ValidationException("Key isn't correct", "");
            }
            var entityType = GetEntityByDto<T>();
            var entity = Mapper.Map<T>(typeof(GameStoreService).GetMethod("GetEntityByKey")
                .MakeGenericMethod(entityType)
                .Invoke(this, new object[] { key }));
            if (entity == null)
            {
                throw new ValidationException(entityType.Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + entityType.Name + "by key={0} ", key);
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
            return entity;
        }

        public T GetByName<T>(string name) where T : class, IDtoBase, IDtoNamed, new()
        {
            var entityType = GetEntityByDto<T>();
            if (entityType == null || string.IsNullOrEmpty(name))
            {
                throw new ValidationException("This parameters aren't correct", string.Empty);
            }
            var entity = Mapper.Map<T>(typeof(GameStoreService).GetMethod("GetEntityByName")
             .MakeGenericMethod(entityType)
             .Invoke(this, new object[] { name }));
            if (entity == null)
            {
                throw new ValidationException(entityType.Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + entityType.Name + "by name={0} ", name);
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


    }
}
