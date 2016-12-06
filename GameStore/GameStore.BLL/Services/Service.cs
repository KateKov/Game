using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class Service<T, TD> : IService<TD> where T: class, IEntityBase, new() where TD: class, IDtoBase, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly DtoToDomain _dtoToDomain;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomain(unitOfWork);
        }

        public void AddEntity(TD model) 
        {
            if (model == null)
                throw new ValidationException("Cannot find " + typeof(T).Name, string.Empty);
            var entity = Mapper.Map<TD, T>(model);
            var result = (T)_dtoToDomain.AddEntities(entity, model);
            _unitOfWork.Repository<T>().Add(result);
        }

        private void EditGame(Game gameModel, GameDTO model)
        {
            var entityGame = gameModel;
            var game = _unitOfWork.Repository<Game>().GetSingle(model.Id);
            game.Translates = entityGame.Translates.Select(x=>new GameTranslate() {Description = x.Description, EntityId = x.EntityId, Id = x.Id, Name = x.Name, Language = x.Language}).ToList();        
            game.Discountinues = entityGame.Discountinues;
            game.UnitsInStock = entityGame.UnitsInStock;
            game.DateOfAdding = DateTime.UtcNow;
            game.Price = entityGame.Price;
            game.Genres.Clear();
            game.PlatformTypes.Clear();
            var result = (Game)_dtoToDomain.AddEntities(game, model);
            _unitOfWork.Repository<Game>().Edit(result);
            _unitOfWork.Save();
        }

        public void EditEntity(TD model)
        {
            if (model == null)
                throw new ValidationException("Cannot find " + typeof(T).Name, string.Empty);
            var entity = Mapper.Map<TD, T>(model);

            if ((model is GameDTO).Equals(true))
            {
                EditGame(entity as Game, model as GameDTO);
            }
            else
            {
                var result = (T)_dtoToDomain.AddEntities(entity, model);
                _unitOfWork.Repository<T>().Edit(result);
                _unitOfWork.Save();
            }
        }

        public TD GetById(string id)
        {
            var entity = _unitOfWork.Repository<T>().GetSingle(id);
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + entity.GetType().Name + "by id={0} ", id);
            return Mapper.Map<TD>(entity);
        }

        public IEnumerable<TD> GetAll()
        {
            var entities =  _unitOfWork.Repository<T>().GetAll().ToList();
            if (entities == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            var dto = Mapper.Map<IEnumerable<TD>>(entities);
            return Mapper.Map<IEnumerable<TD>>(entities);
        }

        public void DeleteById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            var entity = _unitOfWork.Repository<T>().GetSingle(id);
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            _unitOfWork.Repository<T>().Delete(entity);
            _logger.Debug("{0} deleting by Id = {1} ", typeof(T).Name, id);
        }

        public void AddOrUpdate(TD model, bool isAdding)
        {
            var action = isAdding ? "Add" : "Edit";
            Validator<TD>.ValidateModel(model);
            if (isAdding)
            {
                AddEntity(model);
            }
            else
            {
                EditEntity(model);
            }
            _logger.Debug(action + " " + typeof(T).Name + " with Id: {0}", model.Id);
        }
    }
}
