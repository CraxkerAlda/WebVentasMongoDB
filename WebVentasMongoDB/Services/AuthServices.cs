using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using WebVentasMongoDB.Models;
using WebVentasMongoDB.ViewModels;

namespace WebVentasMongoDB.Services
{
    public class AuthServices
    {
        private readonly MongoDBContext _context;

        public AuthServices()
        {
            _context = new MongoDBContext();
        }

        public AuthVM Login(string email, string password)
        {
            email = email?.Trim().ToLower() ?? "";
            password = password?.Trim() ?? "";

            var usuario = _context.Usuarios
                .Find(u => u.Email.ToLower() == email && u.Password == password)
                .FirstOrDefault();


            if (usuario != null)
            {

                var relaciones = _context.UsuariosRoles
                    .Find(r => r.IdUsuario == usuario.Id).ToList();

                foreach (var rel in relaciones)
                {
                    var rol = _context.Roles.Find(r => r.Id == rel.IdRol).FirstOrDefault();
                    if (rol != null)
                        rel.RolVM = rol;
                }

                usuario.UsuarioRolVMs = relaciones;
            }

            return usuario;
        }

        public bool UpdatePassword(string userName, string oldPassword, string newPassword)
        {
            var filter = Builders<AuthVM>.Filter.Eq(u => u.Email, userName) &
                         Builders<AuthVM>.Filter.Eq(u => u.Password, oldPassword);
            var update = Builders<AuthVM>.Update.Set(u => u.Password, newPassword);

            var result = _context.Usuarios.UpdateOne(filter, update);
            return result.ModifiedCount > 0;
        }

        public List<RolVM> GetAllRoles()
        {
            return _context.Roles.Find(_ => true).ToList();
        }


        public bool InsertUser(AuthVM authVM, string idRol)
        {
            try
            {
                _context.Usuarios.InsertOne(authVM);

                var insertedUser = _context.Usuarios
                    .Find(u => u.Email == authVM.Email)
                    .FirstOrDefault();

                if (insertedUser == null)
                    return false;

                UsuarioRolVM usuarioRol = new UsuarioRolVM
                {
                    IdUsuario = insertedUser.Id,
                    IdRol = idRol
                };

                _context.UsuariosRoles.InsertOne(usuarioRol);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public AuthVM GetByEmail(string email)
        {
            return _context.Usuarios.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }


    }
}
