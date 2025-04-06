using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVentasMongoDB.ViewModels
{
    public class UsuarioRolVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUsuario { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRol { get; set; }

        [BsonIgnoreIfNull]
        public RolVM RolVM { get; set; }

        [BsonIgnoreIfNull]
        public AuthVM AuthVM { get; set; }
    }
}