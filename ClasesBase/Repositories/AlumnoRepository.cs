using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Database;

namespace ClasesBase.Repositories
{
    public class AlumnoRepository
    {

        // Insertar un alumno 
        public void AddAlumno(Alumno alumno)
        {
            string query = "INSERT INTO Alumno (Alu_DNI, Alu_Apellido, Alu_Nombre, Alu_Email) " +
                           "VALUES (@dni, @apellido, @nombre, @correo)";

            SqlParameter[] parameters = {
                new SqlParameter("@dni", alumno.Alu_DNI),
                new SqlParameter("@apellido", alumno.Alu_Apellido),
                new SqlParameter("@nombre", alumno.Alu_Nombre),
                new SqlParameter("@correo", alumno.Alu_Email)
            };

            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Obtener todos los alumnos
        public List<Alumno> GetAlumnos()
        {
            string query = "SELECT * FROM Alumno";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            List<Alumno> alumnos = new List<Alumno>();
            foreach (DataRow row in dt.Rows)
            {
                Alumno alumno = new Alumno
                {
                    Alu_DNI = row["Alu_DNI"].ToString(),
                    Alu_Apellido = row["Alu_Apellido"].ToString(),
                    Alu_Nombre = row["Alu_Nombre"].ToString(),
                    Alu_Email = row["Alu_Email"].ToString()
                };
                alumnos.Add(alumno);
            }

            return alumnos;
        }

        // Buscar alumno por ID
        public Alumno GetAlumnoById(int id)
        {
            string query = "SELECT * FROM Alumno WHERE Alu_ID = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", id)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            Alumno alumno = new Alumno();
            alumno.Alu_ID = Convert.ToInt32(row["Alu_ID"]);
            alumno.Alu_DNI = row["Alu_DNI"].ToString();
            alumno.Alu_Apellido = row["Alu_Apellido"].ToString();
            alumno.Alu_Nombre = row["Alu_Nombre"].ToString();
            alumno.Alu_Email = row["Alu_Email"].ToString();

            return alumno;
        }

        // Actualizar alumno
        public void UpdateAlumno(Alumno alumno)
        {
            string query = "UPDATE Alumno SET Alu_DNI = @dni, Alu_Apellido = @apellido, " +
                           "Alu_Nombre = @nombre, Alu_Email = @correo WHERE Alu_ID = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@dni", alumno.Alu_DNI),
                new SqlParameter("@apellido", alumno.Alu_Apellido),
                new SqlParameter("@nombre", alumno.Alu_Nombre),
                new SqlParameter("@correo", alumno.Alu_Email),
                new SqlParameter("@id", alumno.Alu_ID)
            };

            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}