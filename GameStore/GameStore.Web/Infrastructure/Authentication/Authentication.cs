using System;
using System.Web;
using System.Web.Security;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Interfaces;

namespace GameStore.Web.Infrastructure.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IUserService _userService;
        private IUserProvider _currentUser;

        public AuthenticationManager(IUserService userService)
        {
            _userService = userService;
        }

        public HttpContext HttpContext { get; set; }

        public IUserProvider CurrentUser
        {
            get
            {
                HttpCookie authCookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

                if (!string.IsNullOrEmpty(authCookie?.Value)) 
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (ticket != null)
                    {
                        UserDTO user = _userService.GetUserByName(ticket.Name);
                        var syncUser = Mapper.Map<UserModel>(user);
                        _currentUser = new UserProvider(syncUser);
                    }
                }
                else
                {
                    _currentUser = new UserProvider();
                }

                return _currentUser;
            }
        }

        public bool Login(string login, string password, bool isPersistent)
        {
            var userDto = _userService.Login(login, password);

            if (userDto != null)
            {
                CreateCookie(userDto.Username, isPersistent);         
                return true;
            }

            return false;
        }

        private void CreateCookie(string userName, bool isPersistent = false) 
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.UtcNow,
                  DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName) 
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }

            HttpContext.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
        }
    }
}