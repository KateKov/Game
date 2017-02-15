using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using GameStore.Web.Interfaces;
using GameStore.Web.Infrastructure.Authentication;

namespace GameStore.Web.Controllers.api
{
    public class BaseApiController: ApiController 
    {
        public string CurrentLangCode { get; protected set; }

        protected readonly IAuthenticationManager Authentication;

        public BaseApiController(IAuthenticationManager authentication)
        {
            Authentication = authentication;
        }

        protected new UserProvider User
        {
            get { return Authentication.CurrentUser as UserProvider; }
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            if (controllerContext.RouteData.Values["lang"] != null && controllerContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = controllerContext.RouteData.Values["lang"] as string;
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                Thread.CurrentThread.CurrentCulture.NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalSeparator = ".",
                    NumberDecimalSeparator = "."
                };
            }
            base.Initialize(controllerContext);
        }
    }
}