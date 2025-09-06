using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;

namespace ClasesBase.Repositories
{
    public class UsuarioRepository
    {
        private static readonly List<Usuario> _usuarios = new List<Usuario>
        {
            new Usuario{ Usu_ID=1, Usu_NombreUsuario="admin",
                         Usu_Contrasenia="1234", Usu_ApellidoNombre="Admin, Root", Rol_ID=1 },
            new Usuario{ Usu_ID=2, Usu_NombreUsuario="docente",
                         Usu_Contrasenia="1234", Usu_ApellidoNombre="Perez, Juan", Rol_ID=2 },
            new Usuario{ Usu_ID=3, Usu_NombreUsuario="recepcion",
                         Usu_Contrasenia="1234", Usu_ApellidoNombre="Gomez, Ana", Rol_ID=3 }
        };

        public Usuario ObtenerPorUsuarioYClave(string usuario, string clave)
        {
            return _usuarios
                   .FirstOrDefault(u => u.Usu_NombreUsuario == usuario &&
                                        u.Usu_Contrasenia == clave);
        }
    }
}
