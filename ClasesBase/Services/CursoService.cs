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






        /// <summary>
        /// Obtiene los cursos dictados por un docente. Necesario para el Punto 4.
        /// </summary>
        public DataTable ObtenerCursosPorDocente(int idDocente)
        {
            return _repo.GetCursosPorDocente(idDocente);
        }

        /// <summary>
        /// Actualiza el estado del curso y delega la cancelación de inscripciones al repositorio.
        /// </summary>
        public void ActualizarEstadoCurso(int idCurso, string nuevoEstadoNombre)
        {
            _repo.ActualizarEstadoCurso(idCurso, nuevoEstadoNombre);
        }

        public Curso ObtenerCurso(int id)
        {
            return _repo.GetCursoById(id);
        }

    }
}
