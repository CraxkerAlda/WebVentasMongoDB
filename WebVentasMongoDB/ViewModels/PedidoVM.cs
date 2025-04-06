using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentasMongoDB.ViewModels
{

    public class PedidoVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IdUsuario { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public List<ItemPedidoVM> Productos { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public string DireccionEnvio { get; set; }
        public string Ciudad { get; set; }
        public string TelefonoContacto { get; set; }
        public string MetodoPago { get; set; }
    }
}

