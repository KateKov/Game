using System;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;

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
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.Id.ToString()))
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.Id))
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key))
                .ForMember(dm => dm.ParentCommentId, map => map.MapFrom(dm => dm.ParentComment.Id))
                .ForMember(dm => dm.ParrentCommentName, map => map.MapFrom(dm => dm.ParentComment.Name));
        }

        private void GenreToDto()
        {
            CreateMap<Genre, GenreDTO>()
                .ForMember(dm => dm.Id,
                    map => map.MapFrom(dm => dm.Id.ToString()))
                .ForMember(dm => dm.GamesKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()))
                .ForMember(dm => dm.GamesId, map => map.MapFrom(dm => dm.Games.Select(x => x.Id).ToList()))
                .ForMember(dm => dm.Translates,
                    map => map.MapFrom(dm => GenreTranslateToDto(dm.Translates.ToList(), dm.ParentGenre)));
        }

        private List<UserDTOTranslate> UserTranslateToDto(List<Role> roles)
        {
            var result = new List<UserDTOTranslate>();
            var enRoles = roles.Where(x => x.Translates.Any(y => y.Language == Language.en)).ToList();
            var enRolesStrings = new List<string>();
            foreach (var role in enRoles)
            {
                var translatesEn = role.Translates.FirstOrDefault(x => x.Language == Language.en);
                var translate = (translatesEn != null) ? translatesEn.Name : role.Translates.First().Name;
                enRolesStrings.Add(translate);
            }

            result.Add(new UserDTOTranslate
            {
                Language = Language.en,
                RolesName = enRolesStrings
            });

            var rusRoles = roles.Where(x => x.Translates.Any(y => y.Language == Language.ru)).ToList();
            var rusRolesStrings = new List<string>();
            foreach (var role in rusRoles)
            {
                var translatesRu = role.Translates.FirstOrDefault(x => x.Language == Language.ru);
                var translate = (translatesRu != null) ? translatesRu.Name : role.Translates.First().Name;
                rusRolesStrings.Add(translate);
            }

            result.Add(new UserDTOTranslate
            {
                Language = Language.ru,
                RolesName = rusRolesStrings
            });

            return result;
        }

        private void GameToDto()
        {
            CreateMap<Game, GameDTO>()
                .ForMember(dm => dm.Id,
                    map => map.MapFrom(dm =>  dm.Id.ToString()))
                .ForMember(dm => dm.PublisherId,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Publisher.Id != null && dm.Publisher.Id != Guid.Empty)
                                    ? dm.Publisher.Id.ToString()
                                    : Guid.NewGuid().ToString()))
                .ForMember(dm => dm.Translates,
                    map =>
                        map.MapFrom(
                            dm => GameTranslateToDto(dm.Translates, dm.Genres, dm.PlatformTypes, dm.Publisher)))
                .ForMember(dm => dm.GenresId,
                    map =>
                        map.MapFrom(
                            dm =>
                                dm.Genres.Select(
                                    x =>
                                        (x.Id != null && x.Id != Guid.Empty)
                                            ? x.Id.ToString()
                                            : Guid.NewGuid().ToString()).ToList()))
                .ForMember(dm => dm.TypesId, map => map.MapFrom(dm => dm.PlatformTypes.Select(x => x.Id).ToList()))
                .ForMember(dm => dm.Comments, map => map.MapFrom(dm => dm.Comments.Select(x => x.Id).ToList()));
        }

        private IEnumerable<GenreDTOTranslate> GenreTranslateToDto(IList<GenreTranslate> genreTranslates, Genre parent)
        {
            var genresDto = new List<GenreDTOTranslate>();
            foreach (var item in genreTranslates)
            {
                string parentGenre;
                if (parent != null)
                {
                    parentGenre = parent.Translates.First(t => t.Language == item.Language).Name;
                    parentGenre = parentGenre.Contains("Default")
                        ? parent.Translates.First(x => !x.Name.Contains("Default")).Name
                        : parentGenre;
                }
                else
                {
                    parentGenre = "";
                }

                var translate = new GenreDTOTranslate
                {
                    Id = item.Id.ToString(),
                    Language = item.Language,
                    Name = item.Name,
                    ParentName = parentGenre
                };
                genresDto.Add(translate);
            }
            return genresDto;
        }

        private List<GameDTOTranslate> GameTranslateToDto(IEnumerable<GameTranslate> game, IEnumerable<Genre> genre,
            IEnumerable<PlatformType> type, Publisher publisher)
        {
            var gamesDto = new List<GameDTOTranslate>();
            var games = game.ToList();
            var genres = (genre != null) ? genre.ToList() : null;
            var types = (type != null) ? type.ToList() : null;
            foreach (var item in games)
            {
                var publisherName = "";
                if (publisher != null)
                {
                    publisherName =
                        (!publisher.Translates.First(t => t.Language == item.Language).Name.Contains("Default"))
                            ? publisher.Translates.First(t => t.Language == item.Language).Name
                            : publisher.Translates.First(t => !t.Name.Contains("Default")).Name;
                }
                else
                {
                    publisherName = item.Language == Language.en ? "unknown" : "неизвестный";
                }

                var gameTranslate = new GameDTOTranslate()
                {
                    Description = item.Description,
                    Name = item.Name,
                    Language = item.Language,
                    Id = item.Id.ToString(),
                    GenresName = (genre != null) ? GetNames<Genre, GenreTranslate>(genres, item) : new List<string>(),
                    PublisherName = publisherName,
                    PlatformTypesName =
                        (types != null)
                            ? GetNames<PlatformType, PlatformTypeTranslate>(types, item)
                            : new List<string>()
                };
                gamesDto.Add(gameTranslate);
            }
            return gamesDto;
        }

        private List<string> GetNames<T, TD>(List<T> models, GameTranslate game) where T : ITranslateNamed<TD>
            where TD : ITranslate
        {
            var names = new List<string>();
            foreach (var item in models)
            {
                var name = item.Translates.First(x => x.Language == game.Language).Name;
                var itemName = (!name.Contains("Default"))
                    ? name
                    : item.Translates.First(x => !x.Name.Contains("Default")).Name;
                names.Add(itemName);
            }

            return names;
        }

        private void PlatformTypeToDto()
        {
            CreateMap<PlatformType, PlatformTypeDTO>()
                .ForMember(dm => dm.Id,
                    map => map.MapFrom(dm =>  dm.Id.ToString()))
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()))
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Games.Select(x => x.Id).ToList()));
        }

        private void PublisherToDto()
        {
            CreateMap<Publisher, PublisherDTO>()
                .ForMember(dm => dm.Id,
                    map => map.MapFrom(dm => dm.Id.ToString()))
                .ForMember(dm => dm.HomePage, map => map.MapFrom(dm => (dm.HomePage == "NULL") ? "" : dm.HomePage))
                .ForMember(dm => dm.GamesKey, map => map.MapFrom(dm => dm.Games.Select(x => x.Key).ToList()));
        }

        private void OrderDetailToDto()
        {
            CreateMap<OrderDetail, OrderDetailDTO>()
                .ForMember(dm => dm.Id,
                    map => map.MapFrom(dm =>  dm.Id.ToString()))
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.Id))
                .ForMember(dm => dm.GameKey, map => map.MapFrom(dm => dm.Game.Key));
        }

        private void OrderToDto()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dm => dm.Id,
                    map => map.MapFrom(dm => dm.Id.ToString()))
                .ForMember(dm => dm.CustomerId, map => map.MapFrom(dm => dm.User.Username))
                .ForMember(dm => dm.OrderDetailsId,
                    map => map.MapFrom(dm => dm.OrderDetails.Select(x => x.Id).ToList()));
        }

        private void Languanes()
        {
            CreateMap<GenreTranslate, GenreDTOTranslate>().ReverseMap();
            CreateMap<GameTranslate, GameDTOTranslate>().ReverseMap();
            CreateMap<PlatformTypeTranslate, PlatformTypeDTOTranslate>().ReverseMap();
            CreateMap<RoleTranslate, RoleDTOTranslate>().ReverseMap();
            CreateMap<PublisherTranslate, PublisherDTOTranslate>().ReverseMap();
        }

        private void CommentDtoToEntity()
        {
            CreateMap<CommentDTO, Comment>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()));
        }

        private void GenreDtoToEntity()
        {
            CreateMap<GenreDTO, Genre>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()));
        }

        private void GameDtoToEntity()
        {
            CreateMap<GameDTO, Game>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()));
        }

        private void PlatformTypeDtoToEntity()
        {
            CreateMap<PlatformTypeDTO, PlatformType>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()));
        }

        private void RoleToDto()
        {
            CreateMap<Role, RoleDTO>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.Id.ToString()));
        }

        private void UserToDto()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => dm.Id.ToString()))
                .ForMember(dm => dm.Comments,
                    map => map.MapFrom(dm => dm.Comments.Select(x => x.Id.ToString()).ToList()))
                .ForMember(dm => dm.Orders,
                    map => map.MapFrom(dm => dm.Orders.Select(x => x.Id.ToString()).ToList()))
                .ForMember(dm => dm.Roles,
                    map => map.MapFrom(dm => dm.Roles.Select(x => x.Id.ToString()).ToList()))
                .ForMember(dm => dm.Method, map=>map.MapFrom(dm=>dm.ManagerProfile.Method))
                .ForMember(dm => dm.Translates,
                    map => map.MapFrom(dm => UserTranslateToDto(dm.Roles.ToList())))
                .ForMember(dm => dm.Bans, map => map.MapFrom(dm => dm.Bans.Select(x => x.Id.ToString()).ToList()));
        }

        private void PublisherDtoToEntity()
        {
            CreateMap<PublisherDTO, Publisher>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()));
        }

        private void OrderDetailDtoToEntity()
        {
            CreateMap<OrderDetailDTO, OrderDetail>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()))
                .ForMember(dm => dm.OrderId, map => map.MapFrom(dm => Guid.Parse(dm.OrderId)))
                .ForMember(dm => dm.GameId,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.GameId.Length == GuidLength && Guid.Parse(dm.GameId) != Guid.Empty)
                                    ? Guid.Parse(dm.GameId)
                                    : Guid.NewGuid()));
        }

        private void OrderDtoToEntity()
        {
            CreateMap<OrderDTO, Order>()
                .ForMember(dm => dm.Id,
                    map =>
                        map.MapFrom(
                            dm =>
                                (dm.Id.Length == GuidLength && Guid.Parse(dm.Id) != Guid.Empty)
                                    ? Guid.Parse(dm.Id)
                                    : Guid.NewGuid()));
        }

        private void UserDtoToEntity()
        {
            CreateMap<UserDTO, User>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => Guid.Parse(dm.Id)));
        }

        private void RoleDtoToEntity()
        {
            CreateMap<RoleDTO, Role>()
                .ForMember(dm => dm.Id, map => map.MapFrom(dm => Guid.Parse(dm.Id)));
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
            RoleToDto();
            UserToDto();
            RoleDtoToEntity();
            UserDtoToEntity();
            Languanes();
        }
    }
}