using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Repositories;
using ClasesBase.Entidades;
using System.Data;
namespace ClasesBase.Services
{
    public class InscripcionService
    {
        private readonly InscripcionRepository inscripcionRepository = new InscripcionRepository();
        private readonly CursoService cursoService = new CursoService();
        public void crearInscripcion(Inscripcion inscripcion)
        {
            inscripcionRepository.addInscripcion(inscripcion);
        }
        public bool existeInscripcion(int id_curso, int id_alumno)
        {
            return inscripcionRepository.existeInscripcion(id_curso, id_alumno);
        }

        public DataTable obtenerInscripciones()
        {
            return inscripcionRepository.getAll();
        }

        public DataTable ObtenerInscripcionesDeAlumno(int aluID)
        {
            return inscripcionRepository.ObtenerInscripcionesPorAlumno(aluID);
        }

        public void AcreditarInscripcion(int insID)
        {
            inscripcionRepository.ActualizarEstadoInscripcion(insID, "confirmado");
        }

        public DataTable GetResultadosPorAlumno(int id_alumno)
        {
            return inscripcionRepository.GetResultadosPorAlumno(id_alumno);
        }

        public void AnularInscripcion(int idInscripcion, int idCurso)
        {
            // 1. Buscamos el curso para ver su estado
            Curso curso = cursoService.ObtenerCurso(idCurso);

            if (curso != null)
            {
                // 2. Buscamos cuál es el ID del estado "programado"
                // (Reutilizamos tu lógica de EstadoService para no hardcodear IDs)
                var estados = EstadoService.ObtenerEstadosCurso();
                var estadoProgramado = estados.FirstOrDefault(e => e.Est_Nombre.Equals("programado", StringComparison.InvariantCultureIgnoreCase));

                if (estadoProgramado != null)
                {
                    // 3. REGLA DE NEGOCIO:
                    // Si el curso está "programado", devolvemos el cupo.
                    if (curso.Est_ID == estadoProgramado.Est_ID)
                    {
                        curso.Cur_Cupo = curso.Cur_Cupo + 1;
                        cursoService.ActualizarCurso(curso);
                    }
                }
            }

            // 4. Finalmente, anulamos la inscripción en la BD
            inscripcionRepository.anularInscripcion(idInscripcion);
        }

    }
}
