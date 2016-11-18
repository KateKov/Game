using System;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Infrastracture;
using GameStore.DAL.Interfaces;
using Microsoft.Win32;
using NLog;
using System.Collections.Generic;

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
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key))
                .ForMember(dm => dm.ParentCommentId, map => map.MapFrom(dm => dm.ParentComment.Id))
                .ForMember(dm => dm.ParrentCommentName, map => map.MapFrom(dm => dm.ParentComment.Name));
               
            CreateMap<Genre, GenreDTO>()
                .ForMember(dm => dm.GamesKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()))
                .ForMember(dm => dm.GamesId, map => map.MapFrom(dm => dm.Games.Select(x => x.Id).ToList())); ;
            CreateMap<Game, GameDTO>()
                .ForMember(dm => dm.PublisherId, map => map.MapFrom(dm => dm.Publisher.Id))
                .ForMember(dm => dm.PublisherName, map => map.MapFrom(dm => dm.Publisher.Name))
                .ForMember(dm => dm.GenresName, map => map.MapFrom(dm => dm.Genres.Select(x => x.Name).ToList()))
                .ForMember(dm => dm.GenresId, map => map.MapFrom(dm => dm.Genres.Select(x => x.Id).ToList()))
                .ForMember(dm => dm.TypesId, map => map.MapFrom(dm => dm.PlatformTypes.Select(x => x.Id).ToList()))
                .ForMember(dm => dm.PlatformTypesName, map => map.MapFrom(dm => dm.PlatformTypes.Select(x => x.Name).ToList()))
                .ForMember(dm => dm.Comments, map => map.MapFrom(dm => dm.Comments.Select(x => x.Id).ToList()));
       
            CreateMap<PlatformType, PlatformTypeDTO>()
                  .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()))
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Games.Select(x => x.Id).ToList()));
            CreateMap<Publisher, PublisherDTO>()
                  .ForMember(dm => dm.Games, map => map.MapFrom(dm => dm.Games.Select(x => x.Name).ToList())); ;
            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.Id))
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key));
            CreateMap<Order, OrderDTO>()
                .ForMember(dm=>dm.OrderDetailsId, map=>map.MapFrom(dm=>dm.OrderDetails.Select(x=>x.Id).ToList()));
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
