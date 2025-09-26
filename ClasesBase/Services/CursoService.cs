using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;
namespace ClasesBase.Services
{
    public class CursoService
    {
        public void GuardarCurso(Curso curso)
        {
            CursoRepository.AgregarCurso(curso);
        }

        public static List<Curso> ObtenerCursos()
        {
            return CursoRepository.ObtenerTodos();
        }
    }
}
