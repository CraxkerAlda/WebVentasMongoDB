using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentasMongoDB.ViewModels
{
    public class RolVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")] 
        public string Nombre { get; set; }

        [BsonIgnoreIfNull]
        public List<AuthVM> AuthVMs { get; set; }
    }

}