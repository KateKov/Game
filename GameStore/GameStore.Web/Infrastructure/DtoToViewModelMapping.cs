using System;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Enums;
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
            CreateMap<GameDTO, UpdateGameViewModel>()
                .ForMember(x=>x.Name, map=>map.MapFrom(dm => dm.Translates.First(x=>x.Language==Language.En).Name))
                .ForMember(x=>x.GenresName, map => map.MapFrom(dm => dm.Translates.First(x=>x.Language==Language.En).GenresName))
                .ForMember(x=>x.Description, map => map.MapFrom(dm => dm.Translates.First(x=>x.Language == Language.En).Description));

            CreateMap<FilterDTO, FilterViewModel>();
            CreateMap<OrderFilterDTO, OrderFilterViewModel>();
            CreateMap<OrderFilterViewModel, OrderFilterDTO>();
            CreateMap<FilterViewModel, FilterDTO>();
            CreateMap<UpdateGameViewModel, GameDTO>()
                .ForMember(x => x.Id, map => map.MapFrom(dm => dm.Id));
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
            CreateMap<GameDTO, GameViewModel>()
                .ForMember(x => x.Name, map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.En).Name))
                .ForMember(x => x.GenresName, map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.En).GenresName))
                .ForMember(x => x.Description, map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.En).Description)); ;
        }
    }
}