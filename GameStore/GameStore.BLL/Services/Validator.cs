using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;

namespace GameStore.BLL.Services
{
    public class Validator
    {
        public static void ValidateGameModel(GameDTO gameDto)
        {
            if (gameDto == null)
                throw new ValidationException("Cannot create game from null", "");
            if (string.IsNullOrEmpty(gameDto.Key))
                throw new ValidationException("Property cannot be empty", "Key");
            if (string.IsNullOrEmpty(gameDto.Name))
                throw new ValidationException("Property cannot be empty", "Name");
        }

        public static void ValidateCommentModel(CommentDTO commentDto)
        {
            if (commentDto == null)
                throw new ValidationException("Cannot create comment from null", "");
            if (string.IsNullOrEmpty(commentDto.Name))
                throw new ValidationException("Property cannot be empty", "Name");
        }

        public static void ValidateGenreModel(GenreDTO genreDto)
        {
            if (genreDto == null)
                throw new ValidationException("Cannot create genre from null", "");
            if (string.IsNullOrEmpty(genreDto.Name))
                throw new ValidationException("Property cannot be empty", "Name");
        }

        public static void ValidatePlatformModel(PlatformTypeDTO platformDto)
        {
            if (platformDto == null)
                throw new ValidationException("Cannot create platform from null", "");
            if (string.IsNullOrEmpty(platformDto.Type))
                throw new ValidationException("Property cannot be empty", "PlatformType");
        }
        public static void ValidatePublisherModel(PublisherDTO publisherDto)
        {
            if (publisherDto == null)
                throw new ValidationException("Cannot create platform from null", "");
            if (string.IsNullOrEmpty(publisherDto.CompanyName))
                throw new ValidationException("Property cannot be empty", "CompanyName");
        }
    }
}
