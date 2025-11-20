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
        public DataTable GetAlumnos()
        {
            string query = "SELECT Alu_ID, " + 
                           "Alu_DNI, " + 
                           "Alu_Apellido, " + 
                           "Alu_Nombre, " + 
                           "Alu_Email, " +
                           "Alu_Apellido + ', ' + Alu_Nombre AS Display " +
                           "FROM Alumno " + 
                           "ORDER BY Alu_Apellido, Alu_Nombre";

            return DatabaseHelper.ExecuteQuery(query);
        }

        //Obtener alumno por DNI
        public Alumno GetAlumnoByDNI(string dni) 
        {
            string query = "SELECT * FROM Alumno WHERE Alu_DNI= @dni";
            SqlParameter[] parameters = {
                 new SqlParameter("@dni", dni)
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

        // Verificar si existe un alumno con el mismo DNI
        public bool ExisteDNI(string dni, int? alumnoIdExcluir = null)
        {
            string query = "SELECT COUNT(*) FROM Alumno WHERE Alu_DNI = @dni";
            
            if (alumnoIdExcluir.HasValue)
            {
                query += " AND Alu_ID != @id";
            }

            SqlParameter[] parametros;
            if (alumnoIdExcluir.HasValue)
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@dni", dni ?? (object)DBNull.Value),
                    new SqlParameter("@id", alumnoIdExcluir.Value)
                };
            }
            else
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@dni", dni ?? (object)DBNull.Value)
                };
            }

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parametros);
            
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }
            
            return false;
        }

        // Verificar si existe un alumno con el mismo Email
        public bool ExisteEmail(string email, int? alumnoIdExcluir = null)
        {
            // Si el email está vacío, no validamos
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string query = "SELECT COUNT(*) FROM Alumno WHERE Alu_Email = @email";
            
            if (alumnoIdExcluir.HasValue)
            {
                query += " AND Alu_ID != @id";
            }

            SqlParameter[] parametros;
            if (alumnoIdExcluir.HasValue)
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@email", email),
                    new SqlParameter("@id", alumnoIdExcluir.Value)
                };
            }
            else
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@email", email)
                };
            }

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parametros);
            
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }
            
            return false;
        }
    }
}