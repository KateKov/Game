using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.BLL.Interfaces.Translates;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class Service<T, TD> : IService<TD> where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ITranslateService<T, TD> _translateService;
        private readonly IDtoToDomainMapping _dtoToDomain;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _translateService = new TranslateService<T, TD>(_unitOfWork);
            _dtoToDomain = new DtoToDomainMapping(_unitOfWork);
        }

        public virtual void AddEntity(TD model)
        {
            if (model == null)
            {
                throw new ValidationException($"Cannot find {typeof(T).Name}", "EntityNotFound");
            }

            var entity = Mapper.Map<TD, T>(model);
            var result = (T) _dtoToDomain.AddEntities(entity, model);
            if (Translate<TD>.IsTranslate())
            {
                result = _translateService.AddTranslate(result, model);
            }

            _unitOfWork.Repository<T>().Add(result);
        }

        public virtual void EditEntity(TD model)
        {
            if (model == null)
            {
                throw new ValidationException($"Cannot find {typeof(T).Name}", "EntityNotFound");
            }
            var entity = Mapper.Map<TD, T>(model);

            var result = (T)_dtoToDomain.AddEntities(entity, model);
            _unitOfWork.Repository<T>().Edit(result);
            if (Translate<TD>.IsTranslate())
            {
                _translateService.EditTranslate(entity, model);
            }

            _unitOfWork.Save();
        }

        public TD GetById(string id)
        {
            var entity = _unitOfWork.Repository<T>().GetSingle(id);
            if (entity == null)
            {
                throw new ValidationException($"Cannot find {typeof(T).Name}", "EntityNotFound");
            }

            _logger.Debug("Getting" + entity.GetType().Name + "by id={0} ", id);
            return Mapper.Map<TD>(entity);
        }

        public int GetTotalNumber(bool isWithDeleted = false)
        {
            var count = _unitOfWork.Repository<T>().GetTotalNumber(isWithDeleted);
            return count;
        }

        public bool IsExist(string id)
        {
            return _unitOfWork.Repository<T>().IsExist(id);
        }

        public IEnumerable<TD> GetAll(bool isWithDeleted = false)
        {
            var entities = (isWithDeleted)
                ? _unitOfWork.Repository<T>().GetAll().ToList()
                : _unitOfWork.Repository<T>().GetAll().Where(x=>!x.IsDeleted).ToList();
            if (entities == null)
            {
                throw new ValidationException($"Cannot find {typeof(T).Name}", "EntityNotFound");
            }

            return Mapper.Map<IEnumerable<TD>>(entities);
        }

        public void DeleteById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ValidationException("Id isn't exist", "Id");
            }

            var entity = _unitOfWork.Repository<T>().GetSingle(id);
            if (entity == null)
            {
                throw new ValidationException($"Cannot find {typeof(T).Name}", "EntityNotFound");
            }

            _unitOfWork.Repository<T>().Delete(entity);
            _logger.Debug($"{typeof(T).Name} deleting by Id = {id} ");
        }
    }
}