using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces.Services;
using GameStore.DAL.Enums;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Roles;
using GameStore.Web.ViewModels.Translates;

namespace GameStore.Web.Controllers
{
    [UserAuthorize(Roles = UserRole.Administrator)]
    public class RolesController : BaseController
    {
        private readonly INamedService<RoleDTO, RoleDTOTranslate> _roleService;

        public RolesController(INamedService<RoleDTO, RoleDTOTranslate> role, IAuthenticationManager authentication) : base(authentication)
        {
            _roleService = role;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var usersDto = _roleService.GetAll(false);
            var users = Mapper.Map<IEnumerable<RoleViewModel>>(usersDto);
            return View(users);
        }

        [HttpGet]
        public ActionResult Details(string name)
        {
            var role = _roleService.GetByName(name);
            return View(Mapper.Map<RoleViewModel>(role));
        }

        [HttpGet]
        public ActionResult New()
        {
            var role = new CreateRoleViewModel
            {
                Id =Guid.NewGuid().ToString(),
                Translates = new List<TranslateViewModel>
                {
                     new TranslateViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
                }
            };
            return View(role);
        }

        [HttpPost]
        public ActionResult New(CreateRoleViewModel createRole)
        {
            if (ModelState.IsValid)
            {
                var roleDto = Mapper.Map<RoleDTO>(createRole);
                _roleService.AddEntity(roleDto);
                return RedirectToAction("Index");
            }
            createRole.Translates = new List<TranslateViewModel>
            {
                new TranslateViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Language = Language.ru
                }
            };
            return View("New", createRole);
        }

        [HttpGet]
        public ActionResult Update(string name)
        {
            var userDto = _roleService.GetByName(name);
            var user = Mapper.Map<CreateRoleViewModel>(userDto);
            return View(user);
        }

        [HttpPost]
        public ActionResult Update(CreateRoleViewModel createRole)
        {
            if (ModelState.IsValid)
            {
                var roleDto = Mapper.Map<RoleDTO>(createRole);
                _roleService.EditEntity(roleDto);
                return RedirectToAction("Index");
            }

            return View("Update", createRole);
        }

        [HttpGet]
        public ActionResult Delete(string name)
        {
            _roleService.DeleteByName(name);
            return RedirectToAction("Index");
        }
    }
}