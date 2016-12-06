using System;
using System.Collections.Generic;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Interfaces;


namespace GameStore.BLL.Services
{
    public class GameStoreService : IGameStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private Dictionary<string, object> _genericServices;
        private Dictionary<string, object> _namedServices;
        private Dictionary<string, object> _keyServices;

        public GameStoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public INamedService<T, TD> NamedService<T, TD>() where T : class, ITranslateDTONamed<TD>, IDtoBase, new()
            where TD : IDtoNamed
        {
            if (_namedServices == null)
            {
                _namedServices = new Dictionary<string, object>();
            }
            var type = typeof(T).Name;
            if (!_namedServices.ContainsKey(type))
            {
                var entityType = DtoToEntity<T>.GetEntityType().GetType();
                var entityTranslate = entityType.GetProperty("Translates").GetType();
                var translateType = entityType.GetProperty("Translates").GetType();
                var serviceType = typeof(ModelWithNameService<,,,>);
                var serviceInstance = Activator.CreateInstance(serviceType.MakeGenericType(entityType, typeof(T), entityTranslate, translateType),
                    _unitOfWork);
                _namedServices.Add(type, serviceInstance);
            }
            return (INamedService<T, TD>)_namedServices[type];
        }

        public IWithKeyService<T> KeyService<T>() where T : class, IDtoWithKey, new()
        {
            if (_keyServices == null)
            {
                _keyServices = new Dictionary<string, object>();
            }
            var type = typeof(T).Name;
            if (!_keyServices.ContainsKey(type))
            {
                var entityType = DtoToEntity<T>.GetEntityType().GetType();
                var serviceType = typeof(ModelWithKeyService<,>);
                var serviceInstance = Activator.CreateInstance(serviceType.MakeGenericType(entityType, typeof(T)),
                    _unitOfWork);
                _keyServices.Add(type, serviceInstance);
            }
            return (IWithKeyService<T>)_keyServices[type];
        }

        public IService<T> GenericService<T>() where T : class, IDtoBase, new()
        {
            if (_genericServices == null)
            {
                _genericServices = new Dictionary<string, object>();
            }
            var type = typeof(T).Name;
            if (!_genericServices.ContainsKey(type))
            {
                var entityType = DtoToEntity<T>.GetEntityType().GetType();
                var serviceType = typeof(Service<,>);
                var serviceInstance = Activator.CreateInstance(serviceType.MakeGenericType(entityType, typeof(T)),
                    _unitOfWork);
                _genericServices.Add(type, serviceInstance);
            }
            return (IService<T>)_genericServices[type];
        }     
    }
}
