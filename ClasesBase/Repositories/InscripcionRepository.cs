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
    class InscripcionRepository
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

    }
}
