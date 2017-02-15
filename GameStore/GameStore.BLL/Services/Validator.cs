using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;

namespace GameStore.BLL.Services
{
    public static class Validator
    {
        public static void Validate(GameDTO game)
        {
            if (game == null)
            {
                throw new ValidationException("Cannot create game from null", "Game");
            }
            if (string.IsNullOrEmpty(game.Key))
            {
                throw new ValidationException("Property key cannot be empty: GameDTO", "Key");
            }

            if (game.Translates.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                throw new ValidationException("Property name cannot be empty: GameDTO", "Name");
            }
        }

        public static void Validate(PublisherDTO publisher)
        {
            if (publisher == null)
            {
                throw new ValidationException("Cannot create publisher from null", "Publisher");
            }
            if (publisher.Translates.Any(x=>string.IsNullOrEmpty(x.Name)))
            {
                throw new ValidationException("Property name cannot be empty: PublisherDTO", "Name");
            }
        }

        public static void Validate(GenreDTO genre)
        {
            if (genre == null)
            {
                throw new ValidationException("Cannot create genre from null", "Genre");
            }
            if (genre.Translates.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                throw new ValidationException("Property name cannot be empty: GenreDTO", "Name");
            }
        }

        public static void Validate(CommentDTO comment)
        {
            if (comment == null)
            {
                throw new ValidationException("Cannot create comment from null", "Comment");
            }
            if (string.IsNullOrEmpty(comment.Name))
            {
                throw new ValidationException("Property name cannot be empty: CommentDTO", "Name");
            }
        }

        public static void Validate(PlatformTypeDTO type)
        {
            if (type == null)
            {
                throw new ValidationException("Cannot create type from null", "PlatformType");
            }
            if (type.Translates.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                throw new ValidationException("Property name cannot be empty: PlatformTypeDTO", "Name");
            }
        }

        public static void Validate(RoleDTO role)
        {
            if (role == null)
            {
                throw new ValidationException("Cannot create role from null", "Role");
            }
            if (role.Translates.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                throw new ValidationException("Property name cannot be empty:RoleDTO", "Name");
            }
        }
    }
}
