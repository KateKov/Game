using GameStore.BLL.DTO;
using System.Collections.Generic;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        void AddComment(CommentDTO comment, string gameKey);
        IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey);
    }
}
