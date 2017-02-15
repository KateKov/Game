using System.Linq;
using AutoMapper;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.BLL.Services
{
    public class KeyService<T, TD> : IWithKeyService<TD> where T: class, IEntityWithKey, new() where TD: class, IDtoWithKey, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly DtoToDomain _dtoToDomain;

        public KeyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomain(unitOfWork);
        }

        public TD GetByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ValidationException("Key isn't correct", "");
            }
            var entity = _unitOfWork.Repository<T>().FindBy(x => x.Key == key).FirstOrDefault();
            if (entity == null)
            {
                throw new ValidationException(typeof(T).Name + " wasn't found", string.Empty);
            }
            _logger.Debug("Getting" + typeof(T).Name + "by key={0} ", key);
            return Mapper.Map<TD>(entity);
        }
    }
}
