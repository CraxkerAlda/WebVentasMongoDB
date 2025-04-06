using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVentasMongoDB.ViewModels
{
    public class ProductoVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoriaId { get; set; }

        public string ImagenBase64 { get; set; }

        [BsonIgnore]
        public string NombreCategoria { get; set; }

        public string ImagenMimeType { get; set; } // ejemplo: image/png, image/jpeg

    }
}
