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
    public class PublishersService : NameService<Publisher, PublisherDTO, PublisherTranslate, PublisherDTOTranslate>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDtoToDomainMapping _dtoToDomain;
        private readonly ITranslateService<Publisher, PublisherDTO> _translateService;

        public PublishersService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomainMapping(_unitOfWork);
            _translateService = new TranslateService<Publisher, PublisherDTO>(_unitOfWork);
        }

        public override void AddEntity(PublisherDTO model)
        {
            Validator.Validate(model);
            base.AddEntity(model);
        }

        public override void EditEntity(PublisherDTO model)
        {
            Validator.Validate(model);
            var publisher = _unitOfWork.Repository<Publisher>().GetSingle(model.Id);

            publisher.HomePage = model.HomePage;

            var result = (Publisher) _dtoToDomain.AddEntities(publisher, model);

            _unitOfWork.Repository<Publisher>().Edit(result);
            _translateService.EditTranslate(result, model);
            _unitOfWork.Save();
        }
    }
}