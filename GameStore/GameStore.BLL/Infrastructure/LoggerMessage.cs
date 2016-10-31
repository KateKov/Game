using System.Text;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Infrastructure
{

    public class LoggerMessage
    {
        public static string generateDataString_GameDTO(GameDTO game)
        {

            if (game == null)
                return "Data: null";
            var sb = new StringBuilder($"Data: key = {game.Key}; name = {game.Name}; description = {game.Description}");
            if (game.Genres != null)
            {
                sb.Append("; genres = ");
                foreach (var genre in game.Genres)
                {
                    sb.Append(genre.Name).Append(", ");
                }
                if (game.Genres.Count > 0)
                    sb.Length = sb.Length - 2;
            }
            if (game.PlatformTypes != null)
            {
                sb.Append("; platforms = ");
                foreach (var platform in game.PlatformTypes)
                {
                    sb.Append(platform.Type).Append(", ");
                }
                if (game.PlatformTypes.Count > 0)
                    sb.Length = sb.Length - 2;
            }
            return sb.ToString();
        }

        public static string generateDataString_CommentDTO(CommentDTO comment)
        {
            if (comment == null)
                return "Data: null";
            var sb = new StringBuilder($"Data: id = {comment.Id}; name = {comment.Name}; body(short) = ");
            int length = sb.Length;
            sb.Append($"{ comment.Body}");
            sb.Length = length + 10;
            sb.Append($"..; parentCommentId = {comment.ParentId}");
            return sb.ToString();
        }
    }
}
