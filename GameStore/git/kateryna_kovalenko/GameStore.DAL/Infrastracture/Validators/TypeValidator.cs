using FluentValidation;
using GameStore.DAL.EF;
using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Infrastracture.Validators
{
    public class TypeValidator : AbstractValidator<PlatformType>
    {
        public TypeValidator()
        {

            RuleFor(x=>x.Type).Must(BeUniqueType).WithMessage("Type already exists");
        }

        private bool BeUniqueType(string type)
        {
            return new GameStoreContext().PlatformTypes.FirstOrDefault(x => x.Type== type) == null;
        }
    }
}
