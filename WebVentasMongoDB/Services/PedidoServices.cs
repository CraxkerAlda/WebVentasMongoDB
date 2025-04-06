using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using WebVentasMongoDB.Models;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Services
{
    public class PedidoServices
    {
        private readonly MongoDBContext _context;

        public PedidoServices()
        {
            _context = new MongoDBContext();
        }

        public void Create(PedidoVM pedido)
        {
            _context.Pedidos.InsertOne(pedido);
        }

        public List<PedidoVM> GetAll()
        {
            return _context.Pedidos.Find(_ => true).ToList();
        }

        public PedidoVM GetById(string id)
        {
            return _context.Pedidos.Find(p => p.Id == id).FirstOrDefault();
        }

        public List<PedidoVM> GetByUsuario(string idUsuario)
        {
            return _context.Pedidos
                .Find(p => p.IdUsuario == idUsuario)
                .SortByDescending(p => p.FechaPedido)
                .ToList();
        }

        public void Update(PedidoVM pedido)
        {
            _context.Pedidos.ReplaceOne(p => p.Id == pedido.Id, pedido);
        }

        public void Delete(string id)
        {
            _context.Pedidos.DeleteOne(p => p.Id == id);
        }

    }
}
