using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using AutoMapper;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.ViewModels.Publishers;
using GameStore.Web.ViewModels.Translates;
using GameStore.Web.Interfaces;

namespace GameStore.Web.Controllers
{
    [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
    public class PublishersController : BaseController
    {
        private readonly INamedService<PublisherDTO, PublisherDTOTranslate> _publisherService;

        public PublishersController(INamedService<PublisherDTO, PublisherDTOTranslate> publisher, IAuthenticationManager authentication) : base(authentication)
        {
            _publisherService = publisher;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var publishers = _publisherService.GetAll(true);

            return View(Mapper.Map<IEnumerable<PublisherViewModel>>(publishers));
        }

        [HttpGet]
        public ActionResult Details(string name)
        {
            var publisher = _publisherService.GetByName(name);
            return View(Mapper.Map<PublisherViewModel>(publisher));
        }

        [HttpGet]
        public ActionResult New()
        {
            var publisher = new CreatePublisherViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Translates = new List<TranslateViewModelDescription>
                {
                    new TranslateViewModelDescription
                    {
                        Id = Guid.NewGuid().ToString(),
                        Language = Language.ru
                    }
                }
            };

            return View(publisher);
        }

        [HttpPost]
        public ActionResult New(CreatePublisherViewModel createPublisher)
        {
            if (ModelState.IsValid)
            {
                var publisherDto = Mapper.Map<PublisherDTO>(createPublisher);
                _publisherService.AddEntity(publisherDto);
                return RedirectToAction("Index");
            }
            createPublisher.Translates = new List<TranslateViewModelDescription>
            {
                new TranslateViewModelDescription
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
            };
            return View("New", createPublisher);
        }

        [HttpGet]
        public ActionResult Update(string name)
        {
            var publisherDto = _publisherService.GetByName(name);
            var publisher = Mapper.Map<CreatePublisherViewModel>(publisherDto);
            return View(publisher);
        }

        [HttpPost]
        public ActionResult Update(CreatePublisherViewModel createPublisher)
        {
            if (ModelState.IsValid)
            {
                var publisherDto = Mapper.Map<PublisherDTO>(createPublisher);
                _publisherService.EditEntity(publisherDto);
                return RedirectToAction("Index");
            }

            return View("Update", createPublisher);
        }

        [HttpGet]
        public ActionResult Delete(string name)
        {
            _publisherService.DeleteByName(name);
            return RedirectToAction("Index");
        }
    }
}