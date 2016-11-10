using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Infrastracture
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