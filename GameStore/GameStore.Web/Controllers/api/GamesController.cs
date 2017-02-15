using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Games;
using GameStore.Web.Infrastructure.AuthorizeAttribute;

namespace GameStore.Web.Controllers.api
{
    [RoutePrefix("api/games")]
    public class GamesController : BaseApiController
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameservice, IAuthenticationManager authentication)
            : base(authentication)
        {
            _gameService = gameservice;
        }

        [Route("{key}")]
        public GameViewModel Get(string key)
        {
            GameDTO gameDto = _gameService.GetByKey(key);
            var gameViewModel = Mapper.Map<GameViewModel>(gameDto);
            return gameViewModel;
        }

        [HttpGet]
        [Route]
        public GameFilteringViewModel Get(
            [FromUri] FilterViewModel filter = null)
        {
            var filterDto = filter == null
                ? new FilterDTO()
                : Mapper.Map<FilterDTO>(filter);

            var isWithDeleted = false;
            if (User.Identity.IsAuthenticated)
            {
                isWithDeleted = User.IsInRole("Administrator") ||
                                User.IsInRole("Manager")
                                || User.IsInRole("Moderator");
            }

            FilterResultDTO gameFilterResult = _gameService.GetAllByFilter(filterDto, isWithDeleted, 1, PageEnum.All);

            var gamesDetailsViewModel = Mapper.Map<IEnumerable<GameViewModel>>(gameFilterResult.Games);

            var gameListViewModel = new GameFilteringViewModel
            {
                Games = gamesDetailsViewModel,
                Filter = filter,
            };

            return gameListViewModel;
        }

        [ApiUserAuthorize(Roles = UserRole.Manager)]
        [Route]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CreateGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var gameDto = Mapper.Map<GameDTO>(model);
           _gameService.AddEntity(gameDto);
            return Request.CreateResponse(HttpStatusCode.Created);
        }


        [ApiUserAuthorize(Roles = UserRole.Manager)]
        [Route("{key}")]
        [HttpPut]
        public HttpResponseMessage Put(string key, [FromBody] CreateGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var gameDto = Mapper.Map<GameDTO>(model);
            _gameService.EditEntity(gameDto);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ApiUserAuthorize(Roles = UserRole.Manager)]
        [Route("{key}")]
        [HttpDelete]
        public HttpResponseMessage Delete(string key)
        {
            _gameService.DeleteByKey(key);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}