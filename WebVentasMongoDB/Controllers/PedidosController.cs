using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebVentasMongoDB.Services;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Controllers
{
    [Authorize(Roles = "Cliente,Administrador")]
    public class PedidosController : Controller
    {
        private readonly PedidoServices _pedidoServices;
        private readonly ProductoServices _productoServices;

        public PedidosController()
        {
            _pedidoServices = new PedidoServices();
            _productoServices = new ProductoServices();
        }

        // GET: Pedidos
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var pedidos = _pedidoServices.GetAll();
            return View(pedidos);
        }


        // POST: Pedidos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult Create(PedidoVM pedido, string ProductosJson)
        {
            if (!string.IsNullOrEmpty(ProductosJson))
            {
                pedido.Productos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemPedidoVM>>(ProductosJson);
            }

            if (pedido.Productos == null || pedido.Productos.Count == 0)
            {
                ModelState.AddModelError("", "Debes agregar al menos un producto al pedido.");
            }

            if (string.IsNullOrWhiteSpace(pedido.DireccionEnvio))
            {
                ModelState.AddModelError("DireccionEnvio", "La dirección de envío es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(pedido.Ciudad))
            {
                ModelState.AddModelError("Ciudad", "La ciudad es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(pedido.TelefonoContacto))
            {
                ModelState.AddModelError("TelefonoContacto", "El teléfono es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(pedido.MetodoPago))
            {
                ModelState.AddModelError("MetodoPago", "Selecciona un método de pago.");
            }

            if (ModelState.IsValid)
            {
                pedido.IdUsuario = User.Identity.Name;
                pedido.Total = pedido.Productos.Sum(p => p.Subtotal);
                _pedidoServices.Create(pedido);
                return RedirectToAction("MisPedidos");
            }

            ViewBag.Productos = new SelectList(_productoServices.GetAll(), "Id", "Nombre");
            ViewBag.ProductosData = _productoServices.GetAll();
            return View(pedido);
        }


        [Authorize(Roles = "Cliente")]
        public ActionResult Create()
        {
            var productos = _productoServices.GetAll();

            ViewBag.Productos = new SelectList(productos, "Id", "Nombre");

            ViewBag.ProductosData = productos.Select(p => new {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio
            });

            return View(new PedidoVM());
        }


        // GET: Pedidos/MisPedidos
        [Authorize(Roles = "Cliente")]
        public ActionResult MisPedidos()
        {
            var pedidos = _pedidoServices.GetByUsuario(User.Identity.Name);
            return View(pedidos);
        }

        // GET: Pedidos/Details
        [Authorize(Roles = "Cliente,Administrador")]
        public ActionResult Details(string id)
        {
            var pedido = _pedidoServices.GetById(id);
            if (pedido == null)
            {
                return View((PedidoVM)null);
            }

            return View(pedido);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult ActualizarEstado(string id, string nuevoEstado)
        {
            var pedido = _pedidoServices.GetById(id);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                _pedidoServices.Update(pedido);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(string id)
        {
            _pedidoServices.Delete(id);
            TempData["Success"] = "Pedido eliminado correctamente.";
            return RedirectToAction("Index");
        }


    }
}
