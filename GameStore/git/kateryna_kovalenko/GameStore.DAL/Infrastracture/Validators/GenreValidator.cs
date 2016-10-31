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
    public class GenreValidator : AbstractValidator<Genre>
    {
        public GenreValidator()
        {
            RuleFor(x => x.Name).Must(BeUniqueName).WithMessage("Key already exists");
        }

        private bool BeUniqueName(string name)
        {
            return new GameStoreContext().Genres.FirstOrDefault(x => x.Name == name) == null;
        }
    }
}
