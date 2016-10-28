using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GameStore.ViewModels;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.Infrastracture
{
    public class DtoToViewModelMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "DTOtoViewModelMappings"; }
        }

        protected override void Configure()
        {

            CreateMap<CommentDTO, CommentViewModel>();
              
            CreateMap<GenreDTO, GenreViewModel>();
            CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            CreateMap<GameDTO, GameViewModel>()
                .ForMember(m => m.Genres,
                    map => map.MapFrom(m => Mapper.Map<List<GenreDTO>, List<GenreViewModel>>(m.Genres)))
                .ForMember(m => m.Comments,
                    map => map.MapFrom(m => Mapper.Map<List<CommentDTO>, List<CommentViewModel>>(m.Comments)))
                .ForMember(m => m.PlatformTypes,
                    map =>
                        map.MapFrom(
                            m => Mapper.Map<List<PlatformTypeDTO>, List<PlatformTypeViewModel>>(m.PlatformTypes)));




        }
    }
}