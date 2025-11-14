using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;

namespace ClasesBase.Services
{
    public class AlumnoService
    {
        private AlumnoRepository repo;

        public AlumnoService()
        {
            repo = new AlumnoRepository();
        }

        public void CrearAlumno(Alumno alumno)
        {
            repo.AddAlumno(alumno);
        }

        public List<Alumno> ObtenerAlumnos()
        {
            return repo.GetAlumnos();
        }
        public Alumno ObtenerAlumnoPorDNI(string dni)
        {
            return repo.GetAlumnoByDNI(dni);
        }

        public void ActualizarAlumno(Alumno alumno)
        {
            repo.UpdateAlumno(alumno);
        }
    }
}
