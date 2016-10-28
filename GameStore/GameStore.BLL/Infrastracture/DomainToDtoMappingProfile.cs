using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Infrastracture
{
    public class DomainToDtoMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "DomainToDTOMappings"; }
        }

        protected override void Configure()
        {

            CreateMap<Comment, CommentDTO>()
                .ForMember(dm => dm.GameId, map => map.MapFrom(dm => dm.Game.Id));
            CreateMap<Genre, GenreDTO>()
                .ForMember(dm => dm.ParentId, map => map.MapFrom(d => d.ParentId));
            CreateMap<Game, GameDTO>();
            CreateMap<PlatformType, PlatformTypeDTO>();
            //.ForMember(dm => dm.Genres, map => map.MapFrom(dm => Mapper.Map<List<Genre>, List<GenreDTO>>(dm.Genres)))
            //.ForMember(dm => dm.PlatformTypes,
            //    map => map.MapFrom(dm => Mapper.Map<List<PlatformType>, List<PlatformTypeDTO>>(dm.PlatformTypes)))
            //.ForMember(dm => dm.Comments,
            //    map => map.MapFrom(dm => Mapper.Map<List<Comment>, List<CommentDTO>>(dm.Comments)));
            //.ForMember(dm => dm.Id, map => map.MapFrom(dto => dto.Id))
            //.ForMember(dm => dm.Description, map => map.MapFrom(dto => dto.Description))
            //.ForMember(dm => dm.Name, map => map.MapFrom(dto => dto.Name));

        }
        //private List<GenreDTO> GenreToDto(List<Genre> genres )
        //{
        //    List<GenreDTO> dtos=new List<GenreDTO>();
        //    foreach (Genre genre in genres)
        //    {
        //        dtos.Add(new GenreDTO() {Id=genre.Id, ParentId = genre.ParentId, Name = genre.Name});
        //    }
        //}

     
    }
}
