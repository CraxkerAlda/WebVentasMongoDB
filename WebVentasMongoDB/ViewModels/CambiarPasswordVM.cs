using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentasMongoDB.ViewModels
{
    public class CambiarPasswordVM
    {
        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; }
        public string ConfirmarPassword { get; set; }
    }
}