using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using WebVentasMongoDB.Models;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Services
{
    public class ProductoServices
    {
        private readonly MongoDBContext _context;

        public ProductoServices()
        {
            _context = new MongoDBContext();
        }

        public List<ProductoVM> GetAll()
        {
            var productos = _context.Productos.Find(_ => true).ToList();
            var categorias = _context.Categorias.Find(_ => true).ToList();

            foreach (var p in productos)
            {
                var categoria = categorias.FirstOrDefault(c => c.Id == p.CategoriaId);
                p.NombreCategoria = categoria?.Nombre ?? "Sin categoría";
            }

            return productos;
        }

        public void Create(ProductoVM producto) =>
            _context.Productos.InsertOne(producto);

        public ProductoVM GetById(string id) =>
            _context.Productos.Find(p => p.Id == id).FirstOrDefault();

        public void Update(ProductoVM producto) =>
            _context.Productos.ReplaceOne(p => p.Id == producto.Id, producto);

        public void Delete(string id) =>
            _context.Productos.DeleteOne(p => p.Id == id);

        public List<CategoriaVM> GetCategorias() =>
        _context.Categorias.Find(_ => true).ToList();

    }
}
