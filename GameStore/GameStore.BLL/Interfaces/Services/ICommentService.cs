using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService : IService<CommentDTO>
    {
        void AddEntity(CommentDTO commentDto, string gameKey);
        void EditEntity(CommentDTO commentDto, string gameKey);
        IEnumerable<CommentDTO> GetCommentsByGameKey(string gameKey);
    }
}
