using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Accounts;
using GameStore.Web.ViewModels.Users;

namespace GameStore.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _service;

        public AccountController(IUserService service, IAuthenticationManager authentication) :base(authentication)
        {
            _service = service;
        }

        [HttpGet]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_service.IsBanned(model.Username))
                {
                    return PartialView("Ban");
                }
                bool isLogin = Authentication.Login(model.Username, model.Password, model.RememberMe);
                
                if (isLogin)
                {
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", GlobalRes.LoginError);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        public ViewResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                if (_service.IsExistByEmail(model.Email) || _service.IsExistByUsername(model.Username))
                {
                    ModelState.AddModelError("", GlobalRes.RegisterError);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var registerUserDto = Mapper.Map<UserDTO>(model);
            _service.Register(registerUserDto);
            return RedirectToAction("Login", new { returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(string returnUrl)
        {
            Authentication.LogOut();

            return RedirectToLocal(returnUrl);
        }

        public PartialViewResult LoginPartial(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("Login");
        }

        [HttpGet]
        [UserAuthorize(Roles = UserRole.Manager)]
        public ActionResult ManagerProfile()
        {
            var user =_service.GetUserByName(User.Identity.Name);
            return View(Mapper.Map<UserViewModel>(user));
        }

        public ActionResult ManagerProfile(UserViewModel user)
        {
            var userDto = Mapper.Map<UserDTO>(user);
            _service.ChangeNotificationMethod(userDto.Username, userDto.Method);
            return RedirectToAction("Index", "Games");
        }

    }
}