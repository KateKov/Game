using AutoMapper;
using GameStore.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.DAL.Entities;
using GameStore.ViewModels;

namespace GameStore.Infrastracture
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration DtoToView()
        {
            return new MapperConfiguration(m =>
            {
                m.CreateMap<CommentDTO, CommentViewModel>();
                
                m.CreateMap<GenreDTO, GenreViewModel>();
                m.CreateMap<GameDTO, GameViewModel>();
                m.CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            });
        }
        public static MapperConfiguration ViewToDto()
        {
            return new MapperConfiguration(m =>
            {
                m.CreateMap<CommentViewModel, CommentDTO>();
                m.CreateMap<GenreViewModel, GenreDTO>();
                m.CreateMap<GameViewModel, GameDTO>();
                m.CreateMap<PlatformTypeViewModel, PlatformTypeDTO>();
            });
        }
    }
}