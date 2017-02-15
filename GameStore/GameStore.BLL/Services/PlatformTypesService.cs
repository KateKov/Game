using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Services
{
    public class PlatformTypesService : NameService<PlatformType, PlatformTypeDTO, PlatformTypeTranslate, PlatformTypeDTOTranslate>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDtoToDomainMapping _dtoToDomain;
        private readonly ITranslateService<PlatformType, PlatformTypeDTO> _translateService;

        public PlatformTypesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomainMapping(_unitOfWork);
            _translateService = new TranslateService<PlatformType, PlatformTypeDTO>(_unitOfWork);
        }

        public override void AddEntity(PlatformTypeDTO model)
        {
            Validator.Validate(model);
            base.AddEntity(model);
        }

        public override void EditEntity(PlatformTypeDTO model)
        {
            Validator.Validate(model);
            var type = _unitOfWork.Repository<PlatformType>().GetSingle(model.Id);

            var result = (PlatformType)_dtoToDomain.AddEntities(type, model);

            _unitOfWork.Repository<PlatformType>().Edit(result);
            _translateService.EditTranslate(result, model);
            _unitOfWork.Save();
        }
    }
}
