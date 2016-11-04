using System;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastructure
{
    public class DomainToDtoMapping : Profile
    {
        public override string ProfileName
        {
            get { return "DtoToViewModelMapping"; }
        }

        [Obsolete]
        protected override void Configure()
        {
            CreateMap<Comment, CommentDTO>()
                 .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.Id))
                 .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key));
            CreateMap<Genre, GenreDTO>();
            CreateMap<Game, GameDTO>();
            CreateMap<PlatformType, PlatformTypeDTO>();
            CreateMap<Publisher, PublisherDTO>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<GenreDTO, Genre>();
            CreateMap<GameDTO, Game>();
            CreateMap<PlatformTypeDTO, PlatformType>();
            CreateMap<PublisherDTO, Publisher>();
            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<OrderDTO, Order>();
        }    
    }
}
