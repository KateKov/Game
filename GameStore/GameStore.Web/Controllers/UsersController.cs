using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Users;
using GameStore.Web.ViewModels.Roles;

namespace GameStore.Web.Controllers
{
    [UserAuthorize(Roles = UserRole.Administrator)]
    public class UsersController : BaseController
    {
        private readonly IGameService _service;
        private readonly IUserService _userService;
        private readonly INamedService<RoleDTO, RoleDTOTranslate> _roleService;

        public UsersController(IGameService service, IUserService user, INamedService<RoleDTO, RoleDTOTranslate> roleService, IAuthenticationManager authentication) : base(authentication)
        {
            _service = service;
            _userService = user;
            _roleService = roleService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var usersDto = _userService.GetAll(true);
            var users= Mapper.Map<IEnumerable<UserViewModel>>(usersDto);
            return View(users);
        }

        [HttpGet]
        public ActionResult Details(string name)
        {
            var user = _userService.GetUserByName(name);
            return View(Mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        public ActionResult New()
        {
            var user = new CreateUserViewModel
            {
                CreateDate = DateTime.UtcNow
            };
            user.AllRoles = Mapper.Map<IEnumerable<RoleViewModel>>(_roleService.GetAll(true)).ToList();
            return View(user);
        }

        [HttpPost]
        public ActionResult New(CreateUserViewModel createUser)
        {
            if (ModelState.IsValid)
            {
                var userDto = Mapper.Map<UserDTO>(createUser);
                _userService.Register(userDto);
                return RedirectToAction("Index");
            }

            var allRoles = _roleService.GetAll(true);
            createUser.AllRoles = Mapper.Map<IEnumerable<RoleViewModel>>(allRoles).ToList();

            return View("New", createUser);
        }

        [HttpGet]
        public ActionResult Update(string name)
        {
            var userDto = _userService.GetUserByName(name);
            var user = Mapper.Map<CreateUserViewModel>(userDto);
            user.AllRoles = Mapper.Map<IEnumerable<RoleViewModel>>(_roleService.GetAll(true)).ToList();
            return View(user);
        }

        [HttpPost]
        public ActionResult Update(CreateUserViewModel createUser)
        {
            if (ModelState.IsValid)
            {
                var userDto = Mapper.Map<UserDTO>(createUser);
                _userService.Edit(userDto);
                return RedirectToAction("Index");
            }
            createUser.AllRoles = Mapper.Map<IEnumerable<RoleViewModel>>(_roleService.GetAll(true)).ToList();
            return View("Update", createUser);
        }

        [HttpGet]
        public ActionResult Delete(string name)
        {
            var user = _userService.GetUserByName(name);
           _service.DeleteById(user.Id);
            return RedirectToAction("Index");
        }
    }
}