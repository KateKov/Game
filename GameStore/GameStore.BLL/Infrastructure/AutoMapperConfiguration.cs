using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration DomaintToDto()
        {
            return new MapperConfiguration(m =>
            {
                m.CreateMap<Comment, CommentDTO>()
                 .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.Id))
                 .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key));
                m.CreateMap<Genre, GenreDTO>();
                m.CreateMap<Game, GameDTO>();
                m.CreateMap<PlatformType, PlatformTypeDTO>();
            });
        }

        public static MapperConfiguration DtoToDomain()
        {
            return new MapperConfiguration(m =>
            {
                m.CreateMap<CommentDTO, Comment>();
                m.CreateMap<GenreDTO, Genre>();
                m.CreateMap<GameDTO, Game>();
                m.CreateMap<PlatformTypeDTO, PlatformType>();
            });
        }
    }
}
