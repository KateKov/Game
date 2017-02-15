using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Enums;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.PaymentService;
using GameStore.Web.ViewModels.Accounts;
using GameStore.Web.ViewModels.Comments;
using GameStore.Web.ViewModels.Games;
using GameStore.Web.ViewModels.Translates;
using GameStore.Web.ViewModels.Genres;
using GameStore.Web.ViewModels.OrderDetails;
using GameStore.Web.ViewModels.Orders;
using GameStore.Web.ViewModels.PlatformTypes;
using GameStore.Web.ViewModels.Publishers;
using GameStore.Web.ViewModels.Roles;
using GameStore.Web.ViewModels.Users;

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

            CreateMap<PublisherDTO, CreatePublisherViewModel>()
                .ForMember(x => x.Name,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name))
                .ForMember(x => x.Description,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Description))
                .ForMember(x => x.Translates,
                    map => map.MapFrom(dm => GetTranslatesWithDescription(dm.Translates.ToList())));

            CreateMap<OrderDetailDTO, CreateOrderDetail>().ReverseMap();

            CreateMap<GameDTO, CreateGameViewModel>()
                .ForMember(x => x.GenresName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).GenresName))
                .ForMember(x => x.Translates,
                    map => map.MapFrom(dm => GetTranslatesWithDescription(dm.Translates.ToList())))
                .ForMember(x => x.Name,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name))
                .ForMember(x => x.PlatformTypesName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).PlatformTypesName))
                .ForMember(x => x.Description,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Description))
                .ForMember(x => x.PublisherName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).PublisherName));


            CreateMap<GenreDTO, CreateGenreViewModel>()
                .ForMember(x => x.ParentName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).ParentName))
                .ForMember(x => x.Translates, map => map.MapFrom(dm => GetTranslates(dm.Translates.ToList())))
                .ForMember(x => x.Name,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<PlatformTypeDTO, CreatePlatformTypeViewModel>()
                .ForMember(x => x.Translates, map => map.MapFrom(dm => GetTranslates(dm.Translates.ToList())))
                .ForMember(x => x.Name,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<FilterDTO, FilterViewModel>();
            CreateMap<OrderFilterDTO, OrderFilterViewModel>();
            CreateMap<OrderFilterViewModel, OrderFilterDTO>();
            CreateMap<FilterViewModel, FilterDTO>();

            CreateMap<CreateGameViewModel, GameDTO>()
                .ForMember(x => x.Id, map => map.MapFrom(dm => dm.Id))
                .ForMember(x => x.Translates,
                    map =>
                        map.MapFrom(
                            dm =>
                                GetGameDtoTranslates(dm.Translates.ToList(), dm.Name, dm.Description,
                                    dm.PlatformTypesName, dm.GenresName, dm.PublisherName)));

            CreateMap<CreatePublisherViewModel, PublisherDTO>()
                .ForMember(x => x.Id, map => map.MapFrom(dm => dm.Id))
                .ForMember(x => x.Translates,
                    map => map.MapFrom(dm => GetPublisherDtoTranslates(dm.Translates.ToList(), dm.Name, dm.Description)));

            CreateMap<CreateGenreViewModel, GenreDTO>()
                .ForMember(x => x.Id, map => map.MapFrom(dm => dm.Id))
                .ForMember(x => x.Translates,
                    map => map.MapFrom(dm => GetGenreDtoTranslates(dm.Translates.ToList(), dm.Name, dm.ParentName)));

            CreateMap<CreatePlatformTypeViewModel, PlatformTypeDTO>()
                .ForMember(x => x.Id, map => map.MapFrom(dm => dm.Id))
                .ForMember(x => x.Translates,
                    map => map.MapFrom(dm => GetTypeDtoTranslates(dm.Translates.ToList(), dm.Name)));

            CreateMap<UserDTO, UserViewModel>()
                .ForMember(x => x.Roles, map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).RolesName));

            CreateMap<UserViewModel, UserDTO>();

            CreateMap<UserDTO, CreateUserViewModel>()
                .ForMember(x => x.SelectedRoles,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).RolesName));

            CreateMap<RoleDTO, RoleViewModel>()
                .ForMember(x => x.Name, map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Name))
                .ForMember(x => x.Key,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<RoleViewModel, RoleDTO>();

            CreateMap<RoleDTO, CreateRoleViewModel>()
                .ForMember(x => x.Translates, map => map.MapFrom(dm => GetTranslates(dm.Translates.ToList())))
                .ForMember(x => x.Name,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<CreateRoleViewModel, RoleDTO>()
                .ForMember(x => x.Id, map => map.MapFrom(dm => dm.Id))
                .ForMember(x => x.Translates,
                    map => map.MapFrom(dm => GetRoleDtoTranslates(dm.Translates.ToList(), dm.Name)));

            CreateMap<CreateUserViewModel, UserDTO>()
                .ForMember(x => x.Translates, map => map.MapFrom(dm => new List<UserDTOTranslate>
                {
                    new UserDTOTranslate
                    {
                        RolesName = dm.SelectedRoles,
                        Language = Language.en
                    }
                }));

            CreateMap<GenreDTO, GenreViewModel>()
                .ForMember(x => x.Name, map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Name))
                .ForMember(x => x.ParentName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).ParentName))
                .ForMember(x => x.Key,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<PlatformTypeDTO, PlatformTypeViewModel>()
                .ForMember(x => x.Name, map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Name))
                .ForMember(x => x.Key,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<VisaViewModel, PaymentParams>()
                 .ForMember(x => x.FromCardNumber, y => y.MapFrom(m => m.CardNumber))
                .ForMember(x => x.NameSurname, y => y.MapFrom(m => m.CardHolderName))
                .ForMember(x => x.ExpirationMonth, y => y.MapFrom(m => m.MonthOfExpire))
                .ForMember(x => x.ExpirationYear, y => y.MapFrom(m => m.YearOfExpire))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(m => m.Phone));

            CreateMap<OrderDetailDTO, OrderDetailViewModel>().ReverseMap();

            CreateMap<OrderDTO, OrderViewModel>().ReverseMap();

            CreateMap<PublisherDTO, PublisherViewModel>()
                .ForMember(x => x.Name, map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Name))
                .ForMember(x => x.Description,
                    map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Description))
                .ForMember(x => x.Key,
                    map => map.MapFrom(dm => dm.Translates.First(x => x.Language == Language.en).Name));

            CreateMap<PublisherViewModel, PublisherDTO>()
                .ForMember(x => x.Translates, map => map.MapFrom(dm => new List<PublisherDTOTranslate>()
                {
                    new PublisherDTOTranslate()
                    {
                        Description = dm.Description,
                        Language = (Language) Enum.Parse(typeof(Language),
                            CultureInfo.CurrentCulture.TwoLetterISOLanguageName),
                        Name = dm.Name
                    }
                }));

            CreateMap<CommentViewModel, CommentDTO>();
            CreateMap<GenreViewModel, GenreDTO>();
            CreateMap<GameViewModel, GameDTO>();
            CreateMap<PlatformTypeViewModel, PlatformTypeDTO>();

            CreateMap<GameDTO, GameViewModel>()
                .ForMember(x => x.Name, map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Name))
                .ForMember(x => x.GenresName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).GenresName))
                .ForMember(x => x.Description,
                    map => map.MapFrom(dm => GetCurrentTranslate(dm.Translates.ToList()).Description))
                .ForMember(x => x.PlatformTypesName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).PlatformTypesName))
                .ForMember(x => x.PublisherName,
                    map => map.MapFrom(dm => GetDefaultTranslate(dm.Translates.ToList()).PublisherName));

            CreateMap<RegisterViewModel, UserDTO>()
                .ForMember(x => x.PasswordHash, map => map.MapFrom(dm => dm.Password))
                .ForMember(x=>x.Email, map => map.MapFrom(dm => dm.Email));
            CreateMap<UserDTO, UserModel>()
                .ForMember(x => x.Roles,
                    map => map.MapFrom(x => x.Translates.First(z => z.Language == Language.en).RolesName));
        }

        public T GetDefaultTranslate<T>(List<T> translates) where T : class, IDTOTranslate, new()
        {
            var translate =
                translates.FirstOrDefault(
                    x =>
                        x.Language ==
                        (Language) Enum.Parse(typeof(Language), CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

            var result = translate ?? translates.First();

            return result;
        }

        private List<GenreDTOTranslate> GetGenreDtoTranslates(IList<TranslateViewModel> translates, string name,
            string parentName)
        {
            var translatesDto = new List<GenreDTOTranslate>();
            translates.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translatesDto.Add(new GenreDTOTranslate
                    {
                        Name = (string.IsNullOrEmpty(x.Name)) ? "Default" + name : x.Name,
                        Id = x.Id,
                        Language = x.Language,
                        ParentName = parentName
                    }));
            translatesDto.Add(new GenreDTOTranslate
            {
                Name = name,
                Language = Language.en,
                ParentName = parentName
            });

            return translatesDto;
        }

        private List<TranslateViewModel> GetTranslates<T>(IEnumerable<T> translatesDto)
            where T : class, IDTOTranslate, new()
        {
            var translates = new List<TranslateViewModel>();
            translatesDto.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translates.Add(new TranslateViewModel
                    {
                        Name = (x.Name != null && x.Name.Contains("Default")) ? "" : x.Name,
                        Id = x.Id,
                        Language = x.Language
                    }));

            return translates;
        }

        private List<TranslateViewModelDescription> GetTranslatesWithDescription<T>(IEnumerable<T> translatesDto)
            where T : class, IDTOTranslateWithDescription, new()
        {
            var translates = new List<TranslateViewModelDescription>();
            translatesDto.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translates.Add(new TranslateViewModelDescription
                    {
                        Description = (x.Description != null && x.Description.Contains("Default")) ? "" : x.Description,
                        Name = (x.Name != null && x.Name.Contains("Default")) ? "" : x.Name,
                        Id = x.Id,
                        Language = x.Language
                    }));

            return translates;
        }

        private List<GameDTOTranslate> GetGameDtoTranslates(IList<TranslateViewModelDescription> translates, string name,
            string description, ICollection<string> platforms, ICollection<string> genres, string publisher)
        {
            var translatesDto = new List<GameDTOTranslate>();
            translates.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translatesDto.Add(new GameDTOTranslate
                    {
                        Description = x.Description,
                        Name = (string.IsNullOrEmpty(x.Name)) ? "Default" + name : x.Name,
                        Id = x.Id,
                        Language = x.Language,
                        GenresName = genres,
                        PlatformTypesName = platforms,
                        PublisherName = publisher
                    }));
            translatesDto.Add(new GameDTOTranslate
            {
                Description = description,
                Name = name,
                Language = Language.en,
                GenresName = genres,
                PlatformTypesName = platforms,
                PublisherName = publisher
            });

            return translatesDto;
        }

        private List<PlatformTypeDTOTranslate> GetTypeDtoTranslates(IList<TranslateViewModel> translates, string name)
        {
            var translatesDto = new List<PlatformTypeDTOTranslate>();
            translates.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translatesDto.Add(new PlatformTypeDTOTranslate
                    {
                        Name = (string.IsNullOrEmpty(x.Name)) ? "Default" + name : x.Name,
                        Id = x.Id,
                        Language = x.Language
                    }));
            translatesDto.Add(new PlatformTypeDTOTranslate
            {
                Name = name,
                Language = Language.en
            });

            return translatesDto;
        }

        private List<RoleDTOTranslate> GetRoleDtoTranslates(IList<TranslateViewModel> translates, string name)
        {
            var translatesDto = new List<RoleDTOTranslate>();
            translates.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translatesDto.Add(new RoleDTOTranslate
                    {
                        Name = (string.IsNullOrEmpty(x.Name)) ? "Default" + name : x.Name,
                        Id = x.Id,
                        Language = x.Language
                    }));
            translatesDto.Add(new RoleDTOTranslate
            {
                Name = name,
                Language = Language.en
            });

            return translatesDto;
        }

        private List<PublisherDTOTranslate> GetPublisherDtoTranslates(IList<TranslateViewModelDescription> translates,
            string name, string description)
        {
            var translatesDto = new List<PublisherDTOTranslate>();
            translates.Where(x => x.Language != Language.en).ToList().ForEach(
                x =>
                    translatesDto.Add(new PublisherDTOTranslate
                    {
                        Description = x.Description,
                        Name = (string.IsNullOrEmpty(x.Name)) ? "Default" + name : x.Name,
                        Id = x.Id,
                        Language = x.Language
                    }));
            translatesDto.Add(new PublisherDTOTranslate
            {
                Description = description,
                Name = name,
                Language = Language.en
            });

            return translatesDto;
        }

        private T GetCurrentTranslate<T>(List<T> translates) where T : IDTOTranslate
        {
            var translate =
                translates.FirstOrDefault(
                    x =>
                        x.Language ==
                        (Language) Enum.Parse(typeof(Language), CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

            var result = translate != null && !translate.Name.Contains("Default")
                ? translate
                : translates.First(x => !x.Name.Contains("Default"));

            return result;
        }
    }
}