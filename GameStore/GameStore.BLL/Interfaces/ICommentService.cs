using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        void AddComment(CommentDTO comment);
        List<CommentDTO> GetCommentsByGame(int gameId);
    }
}
