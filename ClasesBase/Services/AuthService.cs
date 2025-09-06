using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;

namespace ClasesBase.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuRepo = new UsuarioRepository();
        private readonly RolRepository _rolRepo = new RolRepository();

        public AuthService() { }

        public Usuario Autenticar(string usuario, string clave)
        {
            Usuario u = _usuRepo.ObtenerPorUsuarioYClave(usuario, clave);
            return u;
        }
    }
}
