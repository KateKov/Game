using System;
using System.Web.Mvc;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;

namespace GameStore.Web.Infrastructure.AuthorizeAttribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UserAuthorize : FilterAttribute, IAuthorizationFilter
    {
        public UserRole Roles { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userProvider = filterContext.HttpContext.User as IUserProvider;

            if (userProvider != null && !userProvider.IsInRole(Roles))
            {
                HandleAccessDenied(filterContext);
            }
        }


        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;

            if (url != null)
            {
                var returnUrl = url.AbsolutePath;
                filterContext.Result =
                    new RedirectResult($"~/Account/Login/ReturnUrl={returnUrl}");
            }
        }

        protected virtual void HandleAccessDenied(AuthorizationContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;

            if (url != null)
            {
                var returnUrl = url.AbsolutePath;
                filterContext.Result =
                    new RedirectResult(string.Format($"~/Error/Http401/?ReturnUrl={returnUrl}"));
            }
        }
    }
}