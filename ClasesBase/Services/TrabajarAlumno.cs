using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;

namespace ClasesBase.Services
{
    public class TrabajarAlumno
    {
        // Devuelve un alumno por ID
        public Alumno TraerAlumno(int id)
        {
            AlumnoRepository repositoryAlumno = new AlumnoRepository();
            return repositoryAlumno.GetAlumnoById(id);
        }
    }
}
