﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVentasMongoDB.ViewModels
{
    public class CategoriaVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }
    }
}
