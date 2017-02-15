using AutoMapper;
using GameStore.BLL.Infrastructure;
using GameStore.DAL.Mapping;
using GameStore.Web.Infrastructure;

namespace GameStore.Web.Infrastracture
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DtoToViewModelMapping>();
                x.AddProfile<DomainToDtoMapping>();
                x.AddProfile<MongoToSql>();
            });
        }
    }
}