using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Infraestructura
{
    public interface IAuthServices
    {
        AuthVM Login(string email, string password);
        bool UpdatePassword(string userName, string oldPassword, string newPassword);
        List<RolVM> GetAllRoles();
        bool InsertUser(AuthVM authVM, string idRol);


    }
}
