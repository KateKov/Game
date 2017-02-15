using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.ViewModels.Comments;

namespace GameStore.Web.Controllers.api
{
    [RoutePrefix("api/games")]
    public class CommentsController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService, IAuthenticationManager authentication)
            : base(authentication)
        {
            _commentService = commentService;
        }

        [Route("{gamekey}/comments/{commentId}")]
        [HttpGet]
        public CommentViewModel Get(string gameKey, string commentId)
        {
            CommentDTO commentDto = _commentService.GetById(commentId);
            var commentViewModel = Mapper.Map<CommentViewModel>(commentDto);
            return commentViewModel;
        }

        [HttpGet]
        [Route("{gamekey}/comments")]
        public IEnumerable<CommentViewModel> Get(string gamekey)
        {
            IEnumerable<CommentDTO> commentsDto = _commentService.GetCommentsByGameKey(gamekey);
            var commentsViewModel = Mapper.Map<IEnumerable<CommentViewModel>>(commentsDto);
            return commentsViewModel;
        }

        [Route("{gamekey}/comments")]
        [HttpPost]
        public HttpResponseMessage Post(string gamekey, [FromBody] CommentViewModel model)
        {
            if (!User.IsInRole("Admin"))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Access denied");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.GameKey = gamekey;
            var commentDto = Mapper.Map<CommentDTO>(model);
            _commentService.AddEntity(commentDto, gamekey);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("{gamekey}/comments/{commentId}")]
        [ApiUserAuthorize(Roles = UserRole.Moderator)]
        [HttpPut]
        public HttpResponseMessage Put(string gamekey, string commentId, [FromBody] CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            model.GameKey = gamekey;
            var commentDto = Mapper.Map<CommentDTO>(model);
            _commentService.EditEntity(commentDto, gamekey);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{gamekey}/comments/{commentId}")]
        [ApiUserAuthorize(Roles = UserRole.Moderator)]
        [HttpDelete]
        public HttpResponseMessage Delete(string gamekey, string commentId)
        {
            _commentService.DeleteById(commentId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}