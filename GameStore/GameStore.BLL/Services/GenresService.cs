using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Interfaces;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Infrastructure;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;

namespace GameStore.BLL.Services
{
    public class GenresService : NameService<Genre, GenreDTO, GenreTranslate, GenreDTOTranslate>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDtoToDomainMapping _dtoToDomain;
        private readonly ITranslateService<Genre, GenreDTO> _translateService;

        public GenresService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dtoToDomain = new DtoToDomainMapping(_unitOfWork);
            _translateService = new TranslateService<Genre, GenreDTO>(_unitOfWork);
        }

        public override void AddEntity(GenreDTO genre)
        {
            Validator.Validate(genre);
            base.AddEntity(genre);
        }

        public override void EditEntity(GenreDTO model)
        {
            Validator.Validate(model);
            var genre = _unitOfWork.Repository<Genre>().GetSingle(model.Id);

            var result = (Genre)_dtoToDomain.AddEntities(genre, model);

            _unitOfWork.Repository<Genre>().Edit(result);
            _translateService.EditTranslate(result, model);
            _unitOfWork.Save();
        }
    }
}
