using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Games;
using GameStore.Web.ViewModels.Genres;
using WebGrease.Css.Extensions;

namespace GameStore.Web.Controllers.api
{
    [RoutePrefix("api/genres")]
    public class GenresController : BaseApiController
    {
        private readonly INamedService<GenreDTO, GenreDTOTranslate> _genreService;
        private readonly IGameService _gameService;

        public GenresController(INamedService<GenreDTO, GenreDTOTranslate> genre, IGameService gameService, IAuthenticationManager authentication) 
            : base(authentication)
        {
            _genreService = genre;
            _gameService = gameService;
        }

        [Route("{name}")]
        [HttpGet]
        public GenreViewModel Get(string name)
        {
            GenreDTO genreDto = _genreService.GetByName(name);
            var genreViewModel = Mapper.Map<GenreViewModel>(genreDto);
            return genreViewModel;
        }

        [Route]
        [HttpGet]
        public IEnumerable<GenreViewModel> Get()
        {
            IEnumerable<GenreDTO> genresDto = _genreService.GetAll(false);
            var genresViewModel = Mapper.Map<IEnumerable<GenreViewModel>>(genresDto);
            return genresViewModel;
        }

        [HttpGet]
        [Route("{name}/games")]
        public IEnumerable<GameViewModel> Games(string name)
        {
            GenreDTO genre = _genreService.GetByName(name);
            var gamesDto = new List<GameDTO>();
            genre.GamesKey.ForEach(x=>_gameService.GetByKey(x));
            var games = Mapper.Map<IEnumerable<GameViewModel>>(gamesDto);

            return games;
        }

        [Route]
        [HttpPost]
        [ApiUserAuthorize(Roles = UserRole.Manager)]
        public HttpResponseMessage Post([FromBody]CreateGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var genreDto = Mapper.Map<GenreDTO>(model);
            _genreService.AddEntity(genreDto);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("{name}")]
        [HttpPut]
        [ApiUserAuthorize(Roles = UserRole.Manager)]
        public HttpResponseMessage Put(string name, [FromBody]CreateGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var genreDto = Mapper.Map<GenreDTO>(model);
            _genreService.EditEntity(genreDto);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{name}")]
        [HttpDelete]
        [ApiUserAuthorize(Roles = UserRole.Manager)]
        public HttpResponseMessage Delete(string name)
        {
            _genreService.DeleteByName(name);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("~/api/games/{key}/genres")]
        public IEnumerable<GenreViewModel> Genres(string key)
        {
            GameDTO gameDto = _gameService.GetByKey(key);
            var genresDto = new List<GenreDTO>();
            gameDto.GenresId.ForEach(x => genresDto.Add(_genreService.GetById(x)));
            var genres = Mapper.Map<IEnumerable<GenreViewModel>>(genresDto);
            return genres;
        }
    }
}