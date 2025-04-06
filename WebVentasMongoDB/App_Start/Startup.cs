using Microsoft.Owin;
using System;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(WebVentasMongoDB.App_Start.Startup))]


namespace WebVentasMongoDB.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Auth/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true,
                CookieHttpOnly = true,
                CookieSecure = CookieSecureOption.SameAsRequest
            });
        }
    }
}
