using System.Collections.Generic;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Interfaces.Services
{
    public interface ITranslateService<T, TD>  where T : class, IEntityBase, new() where TD : class, IDtoBase, new()
    {
        T AddTranslate(T entity, TD dto);
        void EditTranslate(T entity, TD dto);

        IEnumerable<TK> AddTranslate<TK, TU>(T entity, TD dto) where TK : class, ITranslate, new()
            where TU : class, IDTOTranslate, new();

        void EditTranslate<TK, TU>(T entity, TD dto) where TK : class, ITranslate, new()
            where TU : class, IDTOTranslate, new();

        IEnumerable<TK> AddTranslateWithDescription<TK, TU>(T entity, TD dto)
            where TK : class, ITranslateWithDescription, new() where TU : class, IDTOTranslateWithDescription, new();

        void EditTranslateWithDescription<TK, TU>(T entity, TD dto) where TK : class, ITranslateWithDescription, new()
            where TU : class, IDTOTranslateWithDescription, new();
    }
}
