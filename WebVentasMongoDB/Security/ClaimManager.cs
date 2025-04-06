using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Security
{
    public class ClaimManager
    {
        public ClaimsIdentity CreateIdentity(AuthVM authVM)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authVM.Id ?? ""),
                new Claim(ClaimTypes.Name, authVM.NombreDeUsuario ?? authVM.Email ?? ""),
                new Claim(ClaimTypes.Email, authVM.Email ?? "")
            };

            if (authVM.UsuarioRolVMs != null && authVM.UsuarioRolVMs.Any())
            {
                foreach (var rel in authVM.UsuarioRolVMs)
                {
                    if (rel != null && rel.RolVM != null && !string.IsNullOrEmpty(rel.RolVM.Nombre))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rel.RolVM.Nombre.Trim())); // ✅ Esto es lo que falta
                    }
                }
            }

            return new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        }


        public void SignIn(AuthVM authVM, bool rememberMe)
        {
            var identity = CreateIdentity(authVM);
            var authManager = HttpContext.Current.GetOwinContext().Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        public void SignOut()
        {
            var authManager = HttpContext.Current.GetOwinContext().Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
