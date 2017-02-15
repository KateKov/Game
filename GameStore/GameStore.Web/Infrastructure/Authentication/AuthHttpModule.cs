using System;
using System.Web;
using System.Web.Mvc;
using GameStore.Web.Interfaces;

namespace GameStore.Web.Infrastructure.Authentication
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
        }

        private void Authenticate(object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            HttpContext context = app.Context;

            var auth = DependencyResolver.Current.GetService<IAuthenticationManager>();

            auth.HttpContext = context;
            context.User = auth.CurrentUser;
        }
  
        public void Dispose()
        {
        }
    }
}