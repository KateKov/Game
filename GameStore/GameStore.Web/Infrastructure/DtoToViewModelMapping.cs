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
            CreateMap<GameDTO, UpdateGameViewModel>();
             
             
            CreateMap<UpdateGameViewModel, GameDTO>();
            CreateMap<GenreDTO, GenreViewModel>();
            CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            CreateMap<OrderDetailDTO, OrderDetailViewModel>();
            CreateMap<OrderDetailViewModel, OrderDetailDTO>();
            CreateMap<OrderDTO, OrderViewModel>();
            CreateMap<OrderViewModel, OrderDTO>();
            CreateMap<PublisherDTO, PublisherViewModel>();
            CreateMap<PublisherViewModel, PublisherDTO>();
            CreateMap<CommentViewModel, CommentDTO>();
            CreateMap<GenreViewModel, GenreDTO>();
            CreateMap<GameViewModel, GameDTO>();
            CreateMap<PlatformTypeViewModel, PlatformTypeDTO>();
            CreateMap<GameDTO, GameViewModel>();
        }
    }
}