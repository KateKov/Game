using System;
using System.Collections.Generic;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Infrastructure
{
    public class DtoToEntity<T> where T: class, IDtoBase, new()
    {
        private static readonly Dictionary<MappingDto, IEntityBase> _mapping;
        private static readonly IEntityBase _entityType;
        private enum MappingDto
        {
            CommentDTO,
            GameDTO,
            GenreDTO,
            OrderDetailDTO,
            OrderDTO,
            PlatformTypeDTO,
            PublisherDTO
        };

    static DtoToEntity()
    {
        _mapping = new Dictionary<MappingDto, IEntityBase>()
            {
                {MappingDto.CommentDTO, new Comment()},
                {MappingDto.GenreDTO, new Genre()},
                {MappingDto.OrderDTO, new Order()},
                {MappingDto.OrderDetailDTO, new OrderDetail()},
                {MappingDto.PlatformTypeDTO, new PlatformType()},
                {MappingDto.PublisherDTO, new Publisher()},
                {MappingDto.GameDTO, new Game() }};
        if (IsEntity())
        {
            var entityEnum = Enum.Parse(typeof(MappingDto), typeof(T).Name);
            _entityType = _mapping[(MappingDto)entityEnum];
        }
    }

    public static bool IsEntity()
    {
        var name = typeof(T).Name;
        return (name.Contains("Comment") || name.Contains("Genre") || name.Contains("Game") ||
                name.Contains("OrderDetail") || name.Contains("PlatformType") || name.Contains("Publisher")||
                name.Contains("Order"));
    }

    public static IEntityBase GetEntityType()
    {
        return (IsEntity()) ? _entityType : null;
    }
}
}
