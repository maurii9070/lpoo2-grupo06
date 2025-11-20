using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
            // Validar DNI duplicado
            if (repo.ExisteDNI(alumno.Alu_DNI))
            {
                throw new Exception("Ya existe un alumno con el DNI '" + alumno.Alu_DNI + "'");
            }

            // Validar Email duplicado (solo si tiene email)
            if (!string.IsNullOrWhiteSpace(alumno.Alu_Email) && repo.ExisteEmail(alumno.Alu_Email))
            {
                throw new Exception("Ya existe un alumno con el email '" + alumno.Alu_Email + "'");
            }
            
            repo.AddAlumno(alumno);
        }

        public DataTable ObtenerAlumnos()
        {
            return repo.GetAlumnos();
        }

        public Alumno ObtenerAlumnoPorDNI(string dni)
        {
            return repo.GetAlumnoByDNI(dni);
        }

        public Alumno ObtenerAlumnoPorID(int id)
        {
            return repo.GetAlumnoById(id);
        }

        public void ActualizarAlumno(Alumno alumno)
        {
            // Validar DNI duplicado (excluyendo el alumno actual)
            if (repo.ExisteDNI(alumno.Alu_DNI, alumno.Alu_ID))
            {
                throw new Exception("Ya existe otro alumno con el DNI '" + alumno.Alu_DNI + "'");
            }

            // Validar Email duplicado (solo si tiene email)
            if (!string.IsNullOrWhiteSpace(alumno.Alu_Email) && repo.ExisteEmail(alumno.Alu_Email, alumno.Alu_ID))
            {
                throw new Exception("Ya existe otro alumno con el email '" + alumno.Alu_Email + "'");
            }
            
            repo.UpdateAlumno(alumno);
        }

        public bool ExisteDNI(string dni, int? alumnoIdExcluir = null)
        {
            return repo.ExisteDNI(dni, alumnoIdExcluir);
        }

        public bool ExisteEmail(string email, int? alumnoIdExcluir = null)
        {
            return repo.ExisteEmail(email, alumnoIdExcluir);
        }
    }
}
