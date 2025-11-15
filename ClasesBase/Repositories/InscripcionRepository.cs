using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Database;

namespace ClasesBase.Repositories
{
    public class InscripcionRepository
    {
        public void addInscripcion(Inscripcion inscripcion)
        {
            string query = "INSERT INTO Inscripcion (Ins_Fecha, Cur_ID, Alu_ID, Est_ID)" +
                           "VALUES (@fecha, @curso, @alumno, @estado)";

            SqlParameter[] parameters = {
                new SqlParameter("@fecha", inscripcion.Ins_Fecha),
                new SqlParameter("@curso", inscripcion.Cur_ID),
                new SqlParameter("@alumno", inscripcion.Alu_ID), 
                new SqlParameter("@estado", inscripcion.Est_ID)
            };

            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public DataTable getAll()
        {
            string sql =
                "SELECT " +
                "   i.Ins_ID AS ID, " +
                "   i.Ins_Fecha AS FECHA, " +
                "   i.Cur_ID AS ID_Curso, " +
                "   c.Cur_Nombre AS Curso, " +
                "   i.Alu_ID AS ID_Alumno, " +
                "   a.Alu_Apellido + ', ' + a.Alu_Nombre AS Alumno, " +
                "   e.Est_Nombre AS Estado " +
                "FROM Inscripcion i " +
                "INNER JOIN Estado e ON i.Est_ID = e.Est_ID " +
                "INNER JOIN Alumno a ON i.Alu_ID = a.Alu_ID " +
                "INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID";

            return DatabaseHelper.ExecuteQuery(sql);
        }
        public bool existeInscripcion(int id_curso, int id_alumno) {
            string query = "SELECT * FROM Inscripcion WHERE Cur_ID = @id_curso AND Alu_ID = @id_alumno";

            SqlParameter[] parameters = {
                   new SqlParameter("@id_curso", id_curso),
                   new SqlParameter("@id_alumno", id_alumno)
        };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            return dt.Rows.Count > 0;
        }

        public List<Inscripcion> getInscripciones()
        {
            string query = "SELECT * FROM Inscripcion"; 
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            List<Inscripcion> inscripciones = new List<Inscripcion>();
            foreach (DataRow row in dt.Rows)
            {
                Inscripcion inscripcion = new Inscripcion
                {
                    Ins_ID = Convert.ToInt32(row["Ins_ID"]),
                    Ins_Fecha = Convert.ToDateTime(row["Ins_Fecha"]),
                    Cur_ID= Convert.ToInt32(row ["Cur_ID"]),
                    Alu_ID = Convert.ToInt32(row["Alu_ID"]),
                    Est_ID = Convert.ToInt32(row["Est_ID"])
                };
                inscripciones.Add(inscripcion);
            }

            return inscripciones;
        }

        // MÉTODO NUEVO 1: Trae las inscripciones de un alumno con el estado del curso y de la inscripción
        public DataTable ObtenerInscripcionesPorAlumno(int aluID)
        {
            string sql = @"SELECT 
                             i.Ins_ID, 
                             c.Cur_Nombre, 
                             e_cur.Est_Nombre AS EstadoCurso, 
                             e_ins.Est_Nombre AS EstadoInscripcion
                           FROM Inscripcion i
                           INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID
                           INNER JOIN Estado e_cur ON c.Est_ID = e_cur.Est_ID
                           INNER JOIN Estado e_ins ON i.Est_ID = e_ins.Est_ID
                           WHERE i.Alu_ID = @aluID";

            SqlParameter[] parametros = {
                new SqlParameter("@aluID", aluID)
            };

            return DatabaseHelper.ExecuteQuery(sql, parametros);
        }

        // MÉTODO NUEVO 2: Actualiza el estado de la inscripción buscándo el ID del estado por nombre
        public void ActualizarEstadoInscripcion(int insID, string nuevoEstadoNombre)
        {
            // Usamos una subconsulta para obtener el ID del estado 'Confirmado' dinámicamente
            string sql = @"UPDATE Inscripcion 
                           SET Est_ID = (SELECT Est_ID FROM Estado WHERE Est_Nombre = @nombreEstado)
                           WHERE Ins_ID = @insID";

            SqlParameter[] parametros = {
                new SqlParameter("@insID", insID),
                new SqlParameter("@nombreEstado", nuevoEstadoNombre)
            };

            DatabaseHelper.ExecuteNonQuery(sql, parametros);
        }

        /// <summary>
        /// Busca todas las inscripciones activas (no canceladas) de un alumno por su DNI.
        /// </summary>
        public DataTable getInscripcionesActivasPorDNI(string dni)
        {
            string sql =
                "SELECT " +
                "   i.Ins_ID AS ID, " +
                "   c.Cur_Nombre AS Curso, " +
                "   i.Cur_ID AS ID_Curso " +
                "FROM Inscripcion i " +
                "INNER JOIN Alumno a ON i.Alu_ID = a.Alu_ID " +
                "INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID " +
                "INNER JOIN Estado e ON i.Est_ID = e.Est_ID " +
                "WHERE a.Alu_DNI = @dni AND e.Est_Nombre <> 'Cancelado'"; // <> significa 'distinto de'

            SqlParameter[] parameters = {
                new SqlParameter("@dni", dni)
            };

            return DatabaseHelper.ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// Actualiza el estado de una inscripción a "Cancelado".
        /// </summary>
        public void anularInscripcion(int ins_ID)
        {
            // Primero, buscamos el ID del estado "Cancelado"
            // Esto es más robusto que "hardcodear" un ID (ej. 3)
            string queryEstado = "SELECT Est_ID FROM Estado WHERE Est_Nombre = 'Cancelado'";
            DataTable dt = DatabaseHelper.ExecuteQuery(queryEstado);

            if (dt.Rows.Count == 0)
            {
                // Manejar el error: no existe el estado "Cancelado" en la BD
                throw new Exception("No se encontró el estado 'Cancelado' en la base de datos.");
            }

            int estadoCanceladoID = Convert.ToInt32(dt.Rows[0]["Est_ID"]);

            // Ahora, actualizamos la inscripción
            string queryUpdate = "UPDATE Inscripcion SET Est_ID = @estadoID WHERE Ins_ID = @inscripcionID";

            SqlParameter[] parameters = {
                new SqlParameter("@estadoID", estadoCanceladoID),
                new SqlParameter("@inscripcionID", ins_ID)
            };

            DatabaseHelper.ExecuteNonQuery(queryUpdate, parameters);
        }
    }
}
