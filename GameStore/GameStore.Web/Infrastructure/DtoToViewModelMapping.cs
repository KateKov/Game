using System;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Infrastructure
{
    public class DtoToViewModelMapping : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToViewModelMapping"; }
        }

        [Obsolete]
        protected override void Configure()
        {
            CreateMap<CommentDTO, CommentViewModel>();
            CreateMap<GenreDTO, GenreViewModel>();
            CreateMap<GameDTO, GameViewModel>();
            CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            CreateMap<CommentViewModel, CommentDTO>();
            CreateMap<GenreViewModel, GenreDTO>();
            CreateMap<GameViewModel, GameDTO>();
            CreateMap<PlatformTypeViewModel, PlatformTypeDTO>();
        }
    }
}