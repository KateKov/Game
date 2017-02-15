using GameStore.DAL.Interfaces;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;

namespace GameStore.BLL.Services
{
    public class RolesService : NameService<Role, RoleDTO, RoleTranslate, RoleDTOTranslate>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITranslateService<Role, RoleDTO> _translateService;

        public RolesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _translateService = new TranslateService<Role, RoleDTO>(_unitOfWork);
        }

        public override void AddEntity(RoleDTO model)
        {
            Validator.Validate(model);
            base.AddEntity(model);
        }

        public override void EditEntity(RoleDTO model)
        {          
            Validator.Validate(model);
            var role = _unitOfWork.Repository<Role>().GetSingle(model.Id);
            _translateService.EditTranslate(role, model);
            _unitOfWork.Save();
        }
    }
}
