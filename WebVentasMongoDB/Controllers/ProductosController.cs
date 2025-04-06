using System;
using System.IO;
using System.Web.Mvc;
using WebVentasMongoDB.Services;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ProductoServices _productoServices;

        public ProductosController()
        {
            _productoServices = new ProductoServices();
        }

        // GET: Productos
        [AllowAnonymous]
        public ActionResult Index()
        {
            var productos = _productoServices.GetAll();
            return View(productos);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_productoServices.GetCategorias(), "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create(ProductoVM producto)
        {
            if (Request.Files.Count > 0 && Request.Files[0]?.ContentLength > 0)
            {
                var file = Request.Files[0];
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    producto.ImagenBase64 = Convert.ToBase64String(bytes);
                    producto.ImagenMimeType = file.ContentType;
                }
            }

            if (ModelState.IsValid)
            {
                _productoServices.Create(producto);
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(_productoServices.GetCategorias(), "Id", "Nombre");
            return View(producto);
        }

        // GET: Productos/Edit/id
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(string id)
        {
            var producto = _productoServices.GetById(id);
            ViewBag.Categorias = new SelectList(_productoServices.GetCategorias(), "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(ProductoVM producto)
        {
            var existente = _productoServices.GetById(producto.Id);

            if (Request.Files.Count > 0 && Request.Files[0]?.ContentLength > 0)
            {
                var file = Request.Files[0];
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    producto.ImagenBase64 = Convert.ToBase64String(bytes);
                    producto.ImagenMimeType = file.ContentType;
                }
            }
            else
            {
                producto.ImagenBase64 = existente.ImagenBase64;
                producto.ImagenMimeType = existente.ImagenMimeType;
            }

            if (ModelState.IsValid)
            {
                _productoServices.Update(producto);
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(_productoServices.GetCategorias(), "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Delete/id
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(string id)
        {
            var producto = _productoServices.GetById(id);
            return View(producto);
        }

        // POST: Productos/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(string id)
        {
            _productoServices.Delete(id);
            return RedirectToAction("Index");
        }

        // GET: Productos/Details/id
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            var producto = _productoServices.GetById(id);
            return View(producto);
        }
    }
}
