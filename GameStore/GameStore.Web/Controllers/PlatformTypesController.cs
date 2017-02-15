using GameStore.BLL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.DAL.Enums;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.PlatformTypes;
using GameStore.Web.ViewModels.Translates;

namespace GameStore.Web.Controllers
{
    [UserAuthorize(Roles = UserRole.Administrator | UserRole.Manager)]
    public class PlatformTypesController : BaseController
    {
        private readonly INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate> _typeService;

        public PlatformTypesController(INamedService<PlatformTypeDTO, PlatformTypeDTOTranslate> publisher, IAuthenticationManager authentication) : base(authentication)
        {
            _typeService = publisher;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var types = _typeService.GetAll(true);
            return View(Mapper.Map<IEnumerable<PlatformTypeViewModel>>(types));
        }

        [HttpGet]
        public ActionResult Details(string name)
        {
            var type = _typeService.GetByName(name);
            return View(Mapper.Map<PlatformTypeViewModel>(type));
        }

        [HttpGet]
        public ActionResult New()
        {
            var type = new CreatePlatformTypeViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Translates = new List<TranslateViewModel>
                {
                     new TranslateViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
                }
            };

            return View(type);
        }

        [HttpPost]
        public ActionResult New(CreatePlatformTypeViewModel createType)
        {
            if (ModelState.IsValid)
            {
                var typeDto = Mapper.Map<PlatformTypeDTO>(createType);
                _typeService.AddEntity(typeDto);
                return RedirectToAction("Index");
            }
            createType.Translates = new List<TranslateViewModel>
            {
                new TranslateViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
            };
            return View("New", createType);
        }

        [HttpGet]
        public ActionResult Update(string name)
        {
            var platformTypeDto = _typeService.GetByName(name);
            var type = Mapper.Map<CreatePlatformTypeViewModel>(platformTypeDto);
            return View(type);
        }

        [HttpPost]
        public ActionResult Update(CreatePlatformTypeViewModel createType)
        {
            if (ModelState.IsValid)
            {
                var typeDto = Mapper.Map<PlatformTypeDTO>(createType);
                _typeService.EditEntity(typeDto);
                return RedirectToAction("Index");
            }

            return View("Update", createType);
        }

        [HttpGet]
        public ActionResult Delete(string name)
        {
            _typeService.DeleteByName(name);
            return RedirectToAction("Index");
        }
    }
}