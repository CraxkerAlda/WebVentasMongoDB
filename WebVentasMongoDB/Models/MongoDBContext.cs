using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Models
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBConnection"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("VentasBD");
        }

        public IMongoCollection<AuthVM> Usuarios => _database.GetCollection<AuthVM>("usuarios");
        public IMongoCollection<RolVM> Roles => _database.GetCollection<RolVM>("roles");
        public IMongoCollection<UsuarioRolVM> UsuariosRoles => _database.GetCollection<UsuarioRolVM>("usuarios_roles");
        public IMongoCollection<ProductoVM> Productos => _database.GetCollection<ProductoVM>("productos");
        public IMongoCollection<CategoriaVM> Categorias => _database.GetCollection<CategoriaVM>("categorias");
        public IMongoCollection<PedidoVM> Pedidos => _database.GetCollection<PedidoVM>("pedidos");
        public IMongoCollection<ItemPedidoVM> ItemPedido => _database.GetCollection<ItemPedidoVM>("detalle_pedidos");

    }
}