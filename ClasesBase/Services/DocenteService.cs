using System.Data;
using ClasesBase.Repositories;
using ClasesBase.Entidades;

namespace ClasesBase.Services
{
    public class DocenteService
    {
        private readonly DocenteRepository _repo = new DocenteRepository();

        public void GuardarDocente(Docente docente)
        {
            // validaciones aquí si querés
            _repo.Add(docente);
        }

        public DataTable ObtenerDocentes()
        {
            return _repo.GetAll();
        }

        public void AgregarDocente(Docente docente)
        {
            _repo.Add(docente);
        }
    }
}