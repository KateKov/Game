using System.Linq;
using AutoMapper;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class NameService<T, TD, TK, TU> : Service<T, TD>, INamedService<TD, TU>
        where T : class, IEntityBase, ITranslateNamed<TK>, new()
        where TD : class, IDtoBase, ITranslateDTONamed<TU>, new()
        where TU : class, IDTOTranslate, new()
        where TK : class, ITranslate, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public NameService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TD GetByName(string name)
        {
            var entity = GetEntity(name);
            return Mapper.Map<TD>(entity);
        }

        public void DeleteByName(string name)
        {
            var entity = GetEntity(name);
            _unitOfWork.Repository<T>().Delete(entity);
            _logger.Debug($"{typeof(T).Name} deleting by Name = {name} ");
        }

        private T GetEntity(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ValidationException("Name is empty", "Name");
            }

            var entity =
               _unitOfWork.Repository<T>()
                   .FindBy(t => t.Translates.Any())
                   .FirstOrDefault(x => x.Translates.Any(z => z.Name == name));
            if (entity == null)
            {
                throw new ValidationException($"{typeof(T).Name} wasn't found", "EntityNotFound");
            }
            return entity;
        }
    }
}