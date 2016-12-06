using System.Linq;
using AutoMapper;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class ModelWithNameService<T, TD, TK, TU> : INamedService<TD, TU> where T: class, IEntityBase, ITranslateNamed<TK>, new() where TD: class, IDtoBase, ITranslateDTONamed<TU>, new() where TU: IDtoNamed where TK: IEntityNamed
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ModelWithNameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TD GetByName(string name) 
        {
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Translates.Any(t=>t.Name.Equals(name))).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + typeof(TD).Name + "by name={0} ", name);
            return Mapper.Map<TD>(entity);
        }

        public void DeleteByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Translates.Any(t=>t.Name.Equals(name))).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            _unitOfWork.Repository<T>().Delete(entity);
            _logger.Debug("{0} deleting by Name = {1} ", typeof(T).Name, name);
        }

    }
}
