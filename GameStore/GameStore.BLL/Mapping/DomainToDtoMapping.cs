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
using GameStore.BLL.DTO.Translation;
using GameStore.DAL.Entities.Translation;

namespace GameStore.BLL.Infrastructure
{
    public class DomainToDtoMapping : Profile
    {
        private const int GuidLength = 36;
        public override string ProfileName
        {
            get { return "DtoToViewModelMapping"; }
        }

        private void CommentToDto()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.EntityId.ToString()))
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.EntityId))
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key))
                .ForMember(dm => dm.ParentCommentId, map => map.MapFrom(dm => dm.ParentComment.EntityId))
                .ForMember(dm => dm.ParrentCommentName, map => map.MapFrom(dm => dm.ParentComment.Name));
        }

        private void GenreToDto()
        {
            CreateMap<Genre, GenreDTO>()
                 .ForMember(dm => dm.Id, map => map.MapFrom(dm => (dm.EntityId == Guid.Empty) ? dm.Id : dm.EntityId.ToString()))
                .ForMember(dm => dm.GamesKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()))
                .ForMember(dm => dm.GamesId, map => map.MapFrom(dm => dm.Games.Select(x => x.EntityId).ToList()));
        }

        private void GameToDto()
        {
            CreateMap<Game, GameDTO>()
                 .ForMember(dm => dm.Id, map => map.MapFrom(dm => (dm.EntityId == Guid.Empty) ? dm.Id : dm.EntityId.ToString()))
                .ForMember(dm => dm.PublisherId, map => map.MapFrom(dm => (dm.Publisher.EntityId != null && dm.Publisher.EntityId != Guid.Empty) ? dm.Publisher.EntityId.ToString() : Guid.NewGuid().ToString()))
                .ForMember(dm => dm.Translates, map => map.MapFrom(dm => GameTranslateToDto(dm.Translates, dm.Genres, dm.PlatformTypes, dm.Publisher)))
                .ForMember(dm => dm.GenresId, map => map.MapFrom(dm => dm.Genres.Select(x => (x.EntityId != null && x.EntityId != Guid.Empty) ? x.EntityId.ToString() : Guid.NewGuid().ToString()).ToList()))
                .ForMember(dm => dm.TypesId, map => map.MapFrom(dm => dm.PlatformTypes.Select(x => x.EntityId).ToList()))
                .ForMember(dm => dm.Comments, map => map.MapFrom(dm => dm.Comments.Select(x => x.EntityId).ToList()));
        }

        private List<GameDTOTranslate> GameTranslateToDto(IEnumerable<GameTranslate> game, IEnumerable<Genre> genres, IEnumerable<PlatformType> types, Publisher publisher)
        {
            var gamesDto = new List<GameDTOTranslate>();
            var games = game.ToList();
            games.ForEach(x => gamesDto.Add(new GameDTOTranslate() {Description = x.Description, Name = x.Name, Language = x.Language, Id = x.Id, GenresName = genres.Select(t=>t.Translates.First(z=>z.Language==x.Language).Name).ToList(), PublisherName = publisher.Translates.First(t=>t.Language==x.Language).Name, PlatformTypesName = types.Select(t=>t.Translates.First(z=>z.Language==x.Language).Name).ToList()}));
            return gamesDto;
        }

        private void PlatformTypeToDto()
        {
            CreateMap<PlatformType, PlatformTypeDTO>()
             .ForMember(dm => dm.Id, map => map.MapFrom(dm => (dm.EntityId == Guid.Empty) ? dm.Id : dm.EntityId.ToString()))
             .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()))
             .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Games.Select(x => x.EntityId).ToList()));
        }

        private void PublisherToDto()
        {
            CreateMap<Publisher, PublisherDTO>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => (dm.EntityId == Guid.Empty) ? dm.Id : dm.EntityId.ToString()))
                .ForMember(dm => dm.HomePage, map => map.MapFrom(dm => (dm.HomePage=="NULL")?"":dm.HomePage));
        }

        private void OrderDetailToDto()
        {
            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => (dm.EntityId == Guid.Empty) ? dm.Id : dm.EntityId.ToString()))
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.EntityId))
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key));
        }

        private void OrderToDto()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => (dm.EntityId == Guid.Empty) ? dm.Id : dm.EntityId.ToString()))
                .ForMember(dm => dm.CustomerId, map => map.MapFrom(dm => dm.CustomerId))
                .ForMember(dm => dm.OrderDetailsId, map => map.MapFrom(dm => dm.OrderDetails.Select(x => x.EntityId).ToList()));
        }

        private void Languanes()
        {
            CreateMap<GenreTranslate, GenreDTOTranslate>().ReverseMap();
            CreateMap<GameTranslate, GameDTOTranslate>().ReverseMap();
            CreateMap<PlatformTypeTranslate, PlatformTypeDTOTranslate>().ReverseMap();
            CreateMap<PublisherTranslate, PublisherDTOTranslate>().ReverseMap();
        }

        private void CommentDtoToEntity()
        {
            CreateMap<CommentDTO, Comment>()
                .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty) ? Guid.Parse(dm.Id) : Guid.NewGuid()));
        }

        private void GenreDtoToEntity()
        {
            CreateMap<GenreDTO, Genre>()
            .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty) ? Guid.Parse(dm.Id) : Guid.NewGuid()));
        }

        private void GameDtoToEntity()
        {
            CreateMap<GameDTO, Game>()
             .ForMember(dm => dm.EntityId,
                 map =>
                     map.MapFrom(
                         dm =>
                             (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                 ? Guid.Parse(dm.Id)
                                 : Guid.NewGuid()))
             .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.Id));
        }

        private void PlatformTypeDtoToEntity()
        {
            CreateMap<PlatformTypeDTO, PlatformType>()
              .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty) ? Guid.Parse(dm.Id) : Guid.NewGuid()));
        }

        private void PublisherDtoToEntity()
        {
            CreateMap<PublisherDTO, Publisher>()
              .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty) ? Guid.Parse(dm.Id) : Guid.NewGuid()));
        }

        private void OrderDetailDtoToEntity()
        {
            CreateMap<OrderDetailDTO, OrderDetail>()
              .ForMember(dm => dm.EntityId, map => map.MapFrom(dm => (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty) ? Guid.Parse(dm.Id) : Guid.NewGuid()))
              .ForMember(dm => dm.OrderId, map => map.MapFrom(dm => Guid.Parse(dm.OrderId)))
              .ForMember(dm => dm.GameId, map => map.MapFrom(dm => (dm.GameId.Length == GuidLength && Guid.Parse(dm.GameId) != Guid.Empty) ? Guid.Parse(dm.GameId) : Guid.NewGuid()));
        }

        private void OrderDtoToEntity()
        {
            CreateMap<OrderDTO, Order>()
                .ForMember(dm => dm.EntityId,
                    map => map.MapFrom(dm => (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty) ? Guid.Parse(dm.Id) : Guid.NewGuid()));
        }

        [Obsolete]
        protected override void Configure()
        {
            CommentToDto();
            GenreToDto();
            GameToDto();
            PlatformTypeToDto();
            PublisherToDto();
            OrderDetailToDto();
            OrderToDto();
            CommentDtoToEntity();
            GenreDtoToEntity();
            GameDtoToEntity();
            PlatformTypeDtoToEntity();
            OrderDetailDtoToEntity();
            PublisherDtoToEntity();
            OrderDtoToEntity();
            Languanes();
        }    
    }   
}
