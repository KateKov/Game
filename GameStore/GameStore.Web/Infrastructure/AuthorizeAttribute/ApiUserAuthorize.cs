using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using GameStore.Web.Infrastructure.Authentication;
using GameStore.Web.Interfaces;

namespace GameStore.Web.Infrastructure.AuthorizeAttribute
{
    public class ApiUserAuthorize :  System.Web.Http.AuthorizeAttribute // todo you have infrastructure folder in you project. Please move all attributes to this folder
    {
        public new UserRole Roles { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            IPrincipal userPrincipal = HttpContext.Current.User;

            if (userPrincipal.Identity.IsAuthenticated)
            {
                var userProvider = userPrincipal as IUserProvider;

                if (userProvider != null && !userProvider.IsInRole(Roles))
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}