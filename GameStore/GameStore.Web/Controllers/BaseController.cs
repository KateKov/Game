using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;

namespace GameStore.Web.Controllers
{
    public abstract class BaseController : Controller 
    {
        public string CurrentLangCode { get; protected set; }

        protected BaseController(IAuthenticationManager authentication)
        {
            Authentication = authentication;
        }

        protected new UserProvider User
        {
            get { return Authentication.CurrentUser as UserProvider; }
        }

        public IAuthenticationManager Authentication { get; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.RouteData.Values["lang"] != null && requestContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = requestContext.RouteData.Values["lang"] as string ?? "en";
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                Thread.CurrentThread.CurrentCulture.NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalSeparator = ".",
                    NumberDecimalSeparator = "."
                };
            }

            base.Initialize(requestContext);
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Games");
        }
    }
}