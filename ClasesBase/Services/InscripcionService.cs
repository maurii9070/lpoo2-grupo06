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

    }
}
