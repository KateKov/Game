using AutoMapper;
using GameStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.DAL.Entities;
using GameStore.ViewModels;

namespace GameStore.Infrastracture
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {

            Mapper.Initialize(c => c.CreateMap<CommentDTO, CommentViewModel>());

            Mapper.Initialize(c => c.CreateMap<GenreDTO, GenreViewModel>()
            
            );
            Mapper.Initialize(c => c.CreateMap<PlatformTypeViewModel, PlatformTypeDTO>());
            Mapper.Initialize(c => c.CreateMap<GameViewModel, GameDTO>()
             .ForMember(x => x.Genres, map => map.MapFrom(m => Mapper.Map<List<GenreViewModel>, List<GenreDTO>>(m.Genres)))
             .ForMember(x => x.Comments, map => map.MapFrom(m => Mapper.Map<List<CommentViewModel>, List<CommentDTO>>(m.Comments)))
             .ForMember(x => x.PlatformTypes, map => map.MapFrom(m => Mapper.Map<List<PlatformTypeViewModel>, List<PlatformTypeDTO>>(m.PlatformTypes)))
             );
            Mapper.Initialize(c => c.CreateMap<CommentDTO, CommentViewModel>());
            Mapper.Initialize(c => c.CreateMap<GenreDTO, GenreViewModel>());
            Mapper.Initialize(c => c.CreateMap<PlatformTypeDTO, PlatformTypeViewModel>());
            Mapper.Initialize(c => c.CreateMap<GameDTO, Game>()
             .ForMember(x => x.Genres, map => map.MapFrom(m => Mapper.Map<List<GenreDTO>, List<Genre>>(m.Genres)))
             .ForMember(x => x.Comments, map => map.MapFrom(m => Mapper.Map<List<CommentDTO>, List<Comment>>(m.Comments)))
             .ForMember(x => x.PlatformTypes, map => map.MapFrom(m => Mapper.Map<List<PlatformTypeDTO>, List<PlatformType>>(m.PlatformTypes)))
             );
        }
    }
}