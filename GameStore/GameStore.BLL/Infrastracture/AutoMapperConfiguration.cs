using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Infrastracture;

namespace GameStore.BLL.Infrastracture
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            IRepository<Genre> genreRepository = (new UnitOfWork()).Repository<Genre>();
            Mapper.Initialize(c => c.CreateMap<Comment, CommentDTO>()
              .ForMember(x => x.GameKey, map => map.MapFrom(m => m.Game.Key))
             );
            Mapper.Initialize(c => c.CreateMap<Genre, GenreDTO>()
            .ForMember(x => x.ParentName, map => map.MapFrom(m => genreRepository.GetSingle(m.ParentId).Name))
            );
            Mapper.Initialize(c => c.CreateMap<PlatformType, PlatformTypeDTO>());
            Mapper.Initialize(c => c.CreateMap<Game, GameDTO>()
             .ForMember(x => x.Genres, map => map.MapFrom(m => Mapper.Map<List<Genre>, List<GenreDTO>>(m.Genres)))
             .ForMember(x => x.Comments, map => map.MapFrom(m => Mapper.Map<List<Comment>, List<CommentDTO>>(m.Comments)))
             .ForMember(x => x.PlatformTypes, map => map.MapFrom(m => Mapper.Map<List<PlatformType>, List<PlatformTypeDTO>>(m.PlatformTypes)))
             );
            Mapper.Initialize(c => c.CreateMap<CommentDTO, Comment>() );
            Mapper.Initialize(c => c.CreateMap<GenreDTO, Genre>());
            Mapper.Initialize(c => c.CreateMap<PlatformTypeDTO, PlatformType>());
            Mapper.Initialize(c => c.CreateMap<GameDTO, Game>()
             .ForMember(x => x.Genres, map => map.MapFrom(m => Mapper.Map<List<GenreDTO>, List<Genre>>(m.Genres)))
             .ForMember(x => x.Comments, map => map.MapFrom(m => Mapper.Map<List<CommentDTO>, List<Comment>>(m.Comments)))
             .ForMember(x => x.PlatformTypes, map => map.MapFrom(m => Mapper.Map<List<PlatformTypeDTO>, List<PlatformType>>(m.PlatformTypes)))
             );
        }
    }
}
