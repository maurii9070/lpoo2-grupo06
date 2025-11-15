using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;
namespace ClasesBase.Services
{
    public class CursoService
    {

        private readonly CursoRepository _repo = new CursoRepository();

        public DataTable ObtenerCursos()
        {
            return _repo.GetAll();   // _repo es CursoRepository
        }
        public List<Curso> getCursosProgramados() {
            return _repo.ObtenerCursosProgramados();
        }
        public void GuardarCurso(Curso curso)
        {
            // faltarian validaciones
            _repo.Add(curso);
        }

        public void ActualizarCurso(Curso curso)
        {
            // Aquí irían validaciones de negocio para la actualización
            _repo.Update(curso);
        }
    }
}
