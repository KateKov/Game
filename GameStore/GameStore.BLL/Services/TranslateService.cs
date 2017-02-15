using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.BLL.Interfaces.Translates;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Services
{
    public class TranslateService<T, TD> : ITranslateService<T, TD> where T : class, IEntityBase, new()
        where TD : class, IDtoBase, new()
    {
        private readonly IUnitOfWork _unitOfWork;

        public TranslateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public T AddTranslate(T entity, TD dto)
        {
            if (entity == null || dto == null)
            {
                throw new ValidationException("The values are null", string.Empty);
            }

            if (entity is Game)
            {
                (entity as Game).Translates =
                    AddTranslateWithDescription<GameTranslate, GameDTOTranslate>(entity, dto).ToList();
            }
            else if (entity is Genre)
            {
                (entity as Genre).Translates = AddTranslate<GenreTranslate, GenreDTOTranslate>(entity, dto).ToList();
            }
            else if (entity is PlatformType)
            {
                (entity as PlatformType).Translates =
                    AddTranslate<PlatformTypeTranslate, PlatformTypeDTOTranslate>(entity, dto).ToList();
            }
            else if (entity is Publisher)
            {
                (entity as Publisher).Translates =
                    AddTranslateWithDescription<PublisherTranslate, PublisherDTOTranslate>(entity, dto).ToList();
            }
            else if (entity is Role)
            {
                (entity as Role).Translates =
                    AddTranslate<RoleTranslate, RoleDTOTranslate>(entity, dto).ToList();
            }
            return entity;
        }

        public void EditTranslate(T entity, TD dto)
        {
            if (entity == null || dto == null)
            {
                throw new ValidationException("The values are null", string.Empty);
            }

            var type = new T();
            if (type is Game)
            {
                EditTranslateWithDescription<GameTranslate, GameDTOTranslate>(entity, dto);
            }
            else if (type is Genre)
            {
                EditTranslate<GenreTranslate, GenreDTOTranslate>(entity, dto);
            }
            else if (type is PlatformType)
            {
                EditTranslate<PlatformTypeTranslate, PlatformTypeDTOTranslate>(entity, dto);
            }
            else if (type is Publisher)
            {
                EditTranslateWithDescription<PublisherTranslate, PublisherDTOTranslate>(entity, dto);
            }
            else if (type is Role)
            {
                EditTranslate<RoleTranslate, RoleDTOTranslate>(entity, dto);
            }
        }

        public IEnumerable<TK> AddTranslate<TK, TU>(T entity, TD dto) where TK : class, ITranslate, new()
            where TU : class, IDTOTranslate, new()
        {
            var translatesWithDescription = new List<TK>();
            ((ITranslateDTONamed<TU>)dto).Translates.ToList().ForEach(x => translatesWithDescription.Add(new TK
            {
                Id = Guid.NewGuid(),
                Language = x.Language,
                Name = x.Name
            }));

            return translatesWithDescription;
        }

        public void EditTranslate<TK, TU>(T entity, TD dto) where TK : class, ITranslate, new()
            where TU : class, IDTOTranslate, new()
        {
            var translatesWithName = _unitOfWork.Repository<TK>().FindBy(x => x.BaseEntityId == entity.Id).ToList();

            if (((ITranslateDTONamed<TU>)dto).Translates == null)
            {
                throw new ValidationException("There are no translates for " + typeof(TD).Name, string.Empty);
            }

            foreach (var item in ((ITranslateDTONamed<TU>)dto).Translates)
            {
                var currentTranslate =
               translatesWithName.FirstOrDefault(
                   x => x.Language == item.Language);

                if (currentTranslate != null)
                {
                    currentTranslate.Name = item.Name;
       
                    _unitOfWork.Repository<TK>().Edit(currentTranslate);
                }
                else
                {
                    var translate = new TK()
                    {
                        BaseEntityId = entity.Id,
                        Id = Guid.NewGuid(),
                        Language = item.Language,
                        Name = item.Name
                    };

                    _unitOfWork.Repository<TK>().Add(translate);
                }
            }
        }

        public IEnumerable<TK> AddTranslateWithDescription<TK, TU>(T entity, TD dto)
            where TK : class, ITranslateWithDescription, new() where TU : class, IDTOTranslateWithDescription, new()
        {
            var translatesWithDescription = new List<TK>();
            ((ITranslateDTODescriptioned<TU>) dto).Translates.ToList().ForEach(x=>translatesWithDescription.Add(new TK
            {
                Id = Guid.NewGuid(),
                Language = x.Language,
                Name = x.Name,
                Description = x.Description
            }));

            return translatesWithDescription;
        }

        public void EditTranslateWithDescription<TK, TU>(T entity, TD dto)
            where TK : class, ITranslateWithDescription, new() where TU : class, IDTOTranslateWithDescription, new()
        {
            var translatesWithDescription = _unitOfWork.Repository<TK>().FindBy(x => x.BaseEntityId == entity.Id).ToList();

            if (((ITranslateDTODescriptioned<TU>)dto).Translates == null)
            {
                throw new ValidationException("There are no translates for " + typeof(TD).Name, string.Empty);
            }

            foreach (var item in ((ITranslateDTODescriptioned<TU>)dto).Translates)
            {
                var currentTranslate =
               translatesWithDescription.FirstOrDefault(
                   x => x.Language == item.Language);

                if (currentTranslate != null)
                {
                    currentTranslate.Name = item.Name;
                    currentTranslate.Description = item.Description;
                    _unitOfWork.Repository<TK>().Edit(currentTranslate);
                }
                else
                {
                    var translate = new TK()
                    {
                        BaseEntityId = entity.Id,
                        Description = item.Description,
                        Id = Guid.NewGuid(),
                        Language = item.Language,
                        Name = item.Name
                    };

                    _unitOfWork.Repository<TK>().Add(translate);
                }
            }       
        }
    }
}