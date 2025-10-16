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

        public void GuardarUsuario(Usuario u)
        {
            // validaciones de negocio aquí si las necesitas
            _repo.Add(u);
        }

        public void ActualizarUsuario(Usuario u)
        {
            _repo.Update(u);
        }

        public void EliminarUsuario(int id)
        {
            _repo.Delete(id);
        }
    }
}