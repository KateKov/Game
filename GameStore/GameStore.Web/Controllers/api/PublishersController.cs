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
using GameStore.Web.ViewModels.Publishers;

namespace GameStore.Web.Controllers.api
{
    [RoutePrefix("api/publishers")]
    public class PublishersController : BaseApiController
    {
        private readonly INamedService<PublisherDTO, PublisherDTOTranslate> _publisherService;
        private readonly IGameService _gameService;

        public PublishersController(IGameService service, INamedService<PublisherDTO, PublisherDTOTranslate> publisher, IAuthenticationManager authentication) 
            : base(authentication)
        {
            _gameService = service;
            _publisherService = publisher;
        }

        [Route("{name}")]
        [HttpGet]
        public PublisherViewModel Get(string name)
        {
            PublisherDTO publisherDto = _publisherService.GetByName(name);
            var publisherViewModel = Mapper.Map<PublisherViewModel>(publisherDto);
            return publisherViewModel;
        }

        [Route]
        [HttpGet]
        public IEnumerable<PublisherViewModel> Get()
        {
            IEnumerable<PublisherDTO> publishersDto = _publisherService.GetAll(false);
            var publishersViewModel = Mapper.Map<IEnumerable<PublisherViewModel>>(publishersDto);
            return publishersViewModel;
        }

        [HttpGet]
        [Route("{name}/games")]
        public IEnumerable<GameViewModel> Games(string name)
        {
            PublisherDTO publisher = _publisherService.GetByName(name);
            var gamesDto = new List<GameDTO>();
            publisher.GamesKey.ForEach(x => _gameService.GetByKey(x));
            var games = Mapper.Map<IEnumerable<GameViewModel>>(gamesDto);

            return games;
        }

        [Route]
        [HttpPost]
        [ApiUserAuthorize(Roles = UserRole.Manager)]
        public HttpResponseMessage Post([FromBody]CreatePublisherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var publisherDto = Mapper.Map<PublisherDTO>(model);
            _publisherService.AddEntity(publisherDto);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("{name}")]
        [HttpPut]
        [ApiUserAuthorize(Roles = UserRole.Manager)]
        public HttpResponseMessage Put(string name, [FromBody]CreatePublisherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var publisherDto = Mapper.Map<PublisherDTO>(model);
            _publisherService.EditEntity(publisherDto);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{name}")]
        [HttpDelete]
        [ApiUserAuthorize(Roles = UserRole.Manager)]
        public HttpResponseMessage Delete(string name)
        {
            _publisherService.DeleteByName(name);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
