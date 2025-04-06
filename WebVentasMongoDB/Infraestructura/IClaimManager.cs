using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Infraestructura
{
    public interface IClaimManager
    {

        ClaimsIdentity CreateIdentity(AuthVM authVM);
        void SignIn(AuthVM authVM, bool rememberMe);
        void SignOut();

    }
}
