using System.Linq;
using System.Web.Mvc;
using WebVentasMongoDB.Infraestructura;
using WebVentasMongoDB.Security;
using WebVentasMongoDB.Services;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthServices _authServices;
        private readonly ClaimManager _claimManager;

        public AuthController()
        {
            _authServices = new AuthServices();
            _claimManager = new ClaimManager();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthVM authVM, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Datos inválidos";
                return View(authVM);
            }

            authVM.Email = (authVM.NombreDeUsuario ?? "").Trim().ToLower();
            authVM.Password = (authVM.Password ?? "").Trim();

            var usuario = _authServices.Login(authVM.Email, authVM.Password);
            if (usuario != null)
            {
                _claimManager.SignIn(usuario, true);
                return RedirectToLocal(returnUrl);
            }

            ViewBag.Message = "Correo o contraseña incorrectos";
            return View(authVM);
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AuthVM authVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Completa todos los campos correctamente.";
                return View(authVM);
            }

            var clienteRol = _authServices.GetAllRoles()
                .FirstOrDefault(r => r.Nombre.ToLower() == "cliente");

            if (clienteRol == null)
            {
                ViewBag.Message = "No se encontró el rol de cliente.";
                return View(authVM);
            }

            var registrado = _authServices.InsertUser(authVM, clienteRol.Id);
            if (registrado)
            {
                TempData["Success"] = "Usuario registrado correctamente. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Ocurrió un error al registrar el usuario.";
            return View(authVM);
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Panel", "Auth");
        }

        public ActionResult LogOut()
        {
            _claimManager.SignOut();
            return RedirectToAction("Login");
        }



        [Authorize]
        public ActionResult Panel()
        {
            var email = System.Security.Claims.ClaimsPrincipal.Current
                .Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            var usuario = _authServices.GetByEmail(email);
            return View(usuario); 
        }



        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Account()
        {
            ViewBag.Roles = new SelectList(_authServices.GetAllRoles(), "Id", "Nombre");
            return View(new AuthVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Account(AuthVM authVM, string Roles)
        {
            if (string.IsNullOrEmpty(Roles))
            {
                ViewBag.Message = "Debes seleccionar un rol.";
                ViewBag.Roles = new SelectList(_authServices.GetAllRoles(), "Id", "Nombre");
                return View(authVM);
            }

            var resultado = _authServices.InsertUser(authVM, Roles);
            if (resultado)
            {
                TempData["Success"] = "Usuario registrado correctamente.";
                return RedirectToAction("Panel");
            }

            ViewBag.Message = "Hubo un error al registrar el usuario.";
            ViewBag.Roles = new SelectList(_authServices.GetAllRoles(), "Id", "Nombre");
            return View(authVM);
        }


        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangePassword(CambiarPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor completa todos los campos.";
                return View(model);
            }

            if (model.PasswordNueva != model.ConfirmarPassword)
            {
                TempData["Error"] = "La nueva contraseña y la confirmación no coinciden.";
                return View(model);
            }

            var userEmail = System.Security.Claims.ClaimsPrincipal.Current
                .Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["Error"] = "No se pudo identificar al usuario.";
                return View(model);
            }

            var actualizado = _authServices.UpdatePassword(userEmail, model.PasswordActual, model.PasswordNueva);
            if (actualizado)
            {
                _claimManager.SignOut(); 
                TempData["Success"] = "Contraseña actualizada correctamente. Por favor, vuelve a iniciar sesión.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Contraseña actual incorrecta o error al actualizar.";
            return View(model);
        }

    }
}
