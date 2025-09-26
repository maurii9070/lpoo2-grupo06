using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;

namespace ClasesBase.Repositories
{
    public class AlumnoRepository
    {
        private static List<Alumno> listaAlumnos = new List<Alumno>();

        public void AddAlumno(Alumno alumno)
        {
            listaAlumnos.Add(alumno);
        }

        public List<Alumno> GetAlumnos()
        {
            return new List<Alumno>(listaAlumnos); // devolvemos copia
        }
    }
}
