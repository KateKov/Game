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
    public class GameValidator : AbstractValidator<Game>
    {
        public GameValidator()
        {
            RuleFor(x => x.Key).Must(BeUniqueKey).WithMessage("Key already exists");
        }

        private bool BeUniqueKey(string key)
        {
            return new GameStoreContext().Games.FirstOrDefault(x => x.Key == key) == null;
        }
    }
}
