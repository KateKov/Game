using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Services
{
    public static class Validator<T> where T : class, IDtoBase, new()
    {
        public static void ValidateModel(T model)
        {
            var modelName = typeof(T).Name;
            if (model == null)
                throw new ValidationException("Cannot create " + modelName +" from null", "");
            if ((model is IDtoWithKey).Equals(true) && string.IsNullOrEmpty(((IDtoWithKey)model).Key))
            {
                throw new ValidationException("Property cannot be empty", "Key");
            }
            if ((model is IDtoNamed).Equals(true) && string.IsNullOrEmpty(((IDtoNamed)model).Name))
            {
                throw new ValidationException("Property cannot be empty", "Name"); 
            }
        }
    }
}
