using System;
using System.Collections.ObjectModel;
using ClasesBase.Entidades;
using ClasesBase.Repositories;

namespace ClasesBase.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _repo = new UsuarioRepository();

        public ObservableCollection<Usuario> ObtenerUsuarios()
        {
            return _repo.GetAll();
        }

        public bool ExisteUsuario(string nombreUsuario, int? usuarioIdExcluir = null)
        {
            return _repo.ExisteUsuario(nombreUsuario, usuarioIdExcluir);
        }

        public void GuardarUsuario(Usuario u)
        {
            // Validar que no exista el usuario
            if (_repo.ExisteUsuario(u.Usu_NombreUsuario))
            {
                throw new Exception("Ya existe un usuario con el nombre '" + u.Usu_NombreUsuario + "'");
            }
            
            _repo.Add(u);
        }

        public void ActualizarUsuario(Usuario u)
        {
            // Validar que no exista otro usuario con el mismo nombre
            if (_repo.ExisteUsuario(u.Usu_NombreUsuario, u.Usu_ID))
            {
                throw new Exception("Ya existe otro usuario con el nombre '" + u.Usu_NombreUsuario + "'");
            }
            
            _repo.Update(u);
        }

        public void EliminarUsuario(int id)
        {
            _repo.Delete(id);
        }
    }
}