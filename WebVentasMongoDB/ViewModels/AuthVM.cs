using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebVentasMongoDB.ViewModels
{
    public class AuthVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombreDeUsuario")]
        public string NombreDeUsuario { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonIgnoreIfNull]
        public List<UsuarioRolVM> UsuarioRolVMs { get; set; }
    }
}
