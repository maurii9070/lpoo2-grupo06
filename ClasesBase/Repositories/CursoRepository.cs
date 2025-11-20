using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Database;
using System.Data.SqlClient;

namespace ClasesBase.Repositories
{
    public class CursoRepository
    {
        
            private static List<Curso> _cursos = new List<Curso>();


            public static List<Curso> ObtenerTodos()
            {
                return _cursos;
            }
/*            public List<Curso> ObtenerCursosProgramados() {

                List<Curso> cursos = new List<Curso>();
                string sql= "SELECT c.Cur_ID, c.Cur_Nombre, c.Cur_Descripcion, c.Cur_Cupo, c.Cur_FechaInicio, c.Cur_FechaFin, c.Est_ID, c.Doc_ID" +
                            "FROM Curso c " +
                            "INNER JOIN Estado e ON c.Est_ID = e.Est_ID "+
                            "WHERE e.Est_Nombre = 'Programado'";


                DataTable dt = DatabaseHelper.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    cursos.Add(new Curso
                    {
                        Cur_ID = Convert.ToInt32(row["Cur_ID"]),
                        Cur_Nombre= row["Cur_Nombre"].ToString(),
                        Cur_Descripcion = row["Cur_Descripcion"].ToString(),
                        Cur_Cupo = Convert.ToInt32(row["Cur_Cupo"]),
                        Cur_FechaInicio = Convert.ToDateTime(row["Cur_FechaInicio"]),
                        Cur_FechaFin = Convert.ToDateTime(row["Cur_FechaFin"]),
                        Est_ID = Convert.ToInt32(row["Est_ID"]),
                        Doc_ID=Convert.ToInt32(row["Doc_ID"])
                    });
                }

                return cursos;
            
            }*/
            public List<Curso> ObtenerCursosProgramados()
            {
                List<Curso> cursos = new List<Curso>();

                string sql = "SELECT c.Cur_ID, c.Cur_Nombre, c.Cur_Descripcion, c.Cur_Cupo, c.Cur_FechaInicio, c.Cur_FechaFin, c.Est_ID, c.Doc_ID " +
                             "FROM Curso c " +
                             "INNER JOIN Estado e ON c.Est_ID = e.Est_ID " +
                             "WHERE e.Est_Nombre = 'programado'";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    cursos.Add(new Curso
                    {
                        Cur_ID = Convert.ToInt32(row["Cur_ID"]),
                        Cur_Nombre = row["Cur_Nombre"].ToString(),
                        Cur_Descripcion = row["Cur_Descripcion"].ToString(),
                        Cur_Cupo = Convert.ToInt32(row["Cur_Cupo"]),
                        Cur_FechaInicio = Convert.ToDateTime(row["Cur_FechaInicio"]),
                        Cur_FechaFin = Convert.ToDateTime(row["Cur_FechaFin"]),
                        Est_ID = Convert.ToInt32(row["Est_ID"]),
                        Doc_ID = Convert.ToInt32(row["Doc_ID"])
                    });
                }

                return cursos;
            }

            public bool CursoExistente(string nombre)
            {
                string sql = "SELECT TOP 1 1 " +
                             "FROM Curso c " +
                             "WHERE c.Cur_Nombre = @nombre";

                SqlParameter[] p = {
                    new SqlParameter("@nombre", nombre)
                };

                DataTable resultado = DatabaseHelper.ExecuteQuery(sql, p);
                return resultado.Rows.Count > 0;
            }

            public DataTable GetAll()
            {
                // CORRECCIÓN: Nos aseguramos que la consulta SQL
                // seleccione los IDs de estado y docente.
                string sql = "SELECT c.Cur_ID, " +
                             "c.Cur_Nombre, " +
                             "c.Cur_Descripcion, " +
                             "c.Cur_Cupo, " +
                             "c.Cur_FechaInicio, " +
                             "c.Cur_FechaFin, " +
                             "e.Est_Nombre AS Estado, " +
                             "d.Doc_Apellido + ', ' + d.Doc_Nombre AS Docente, " +
                    // --- ESTAS DOS LÍNEAS SON CRUCIALES ---
                             "c.Est_ID, " +
                             "c.Doc_ID " +
                    // ------------------------------------
                             "FROM Curso c " +
                             "INNER JOIN Estado e ON c.Est_ID = e.Est_ID " +
                             "INNER JOIN Docente d ON c.Doc_ID = d.Doc_ID " +
                             "ORDER BY c.Cur_Nombre";
                return DatabaseHelper.ExecuteQuery(sql);
            }

            public void Add(Curso c)
            {
                string sql = "INSERT INTO Curso " +
                             "(Cur_Nombre, Cur_Descripcion, Cur_Cupo, " +
                             "Cur_FechaInicio, Cur_FechaFin, Est_ID, Doc_ID) " +
                             "VALUES " +
                             "(@Nom,@Desc,@Cupo,@FIni,@FFin,@Est,@Doc)";
                SqlParameter[] p =
            {
                new SqlParameter("@Nom",  c.Cur_Nombre),
                new SqlParameter("@Desc", c.Cur_Descripcion),
                new SqlParameter("@Cupo", c.Cur_Cupo),
                new SqlParameter("@FIni", c.Cur_FechaInicio),
                new SqlParameter("@FFin", c.Cur_FechaFin),
                new SqlParameter("@Est",  c.Est_ID),
                new SqlParameter("@Doc",  c.Doc_ID)
            };
                DatabaseHelper.ExecuteNonQuery(sql, p);
            }

            public void Update(Curso c)
            {
                string sql = "UPDATE Curso " +
                             "SET Cur_Nombre = @Nom, " +
                             "Cur_Descripcion = @Desc, " +
                             "Cur_Cupo = @Cupo, " +
                    // --- LÍNEAS NUEVAS ---
                             "Cur_FechaInicio = @FIni, " +
                             "Cur_FechaFin = @FFin, " +
                    // --- FIN LÍNEAS NUEVAS ---
                             "Est_ID = @Est, " +
                             "Doc_ID = @Doc " +
                             "WHERE Cur_ID = @ID";

                SqlParameter[] p =
                {
                    new SqlParameter("@Nom",  c.Cur_Nombre),
                    new SqlParameter("@Desc", c.Cur_Descripcion),
                    new SqlParameter("@Cupo", c.Cur_Cupo),
                    // --- LÍNEAS NUEVAS ---
                    new SqlParameter("@FIni", c.Cur_FechaInicio),
                    new SqlParameter("@FFin", c.Cur_FechaFin),
                    // --- FIN LÍNEAS NUEVAS ---
                    new SqlParameter("@Est",  c.Est_ID),
                    new SqlParameter("@Doc",  c.Doc_ID),
                    new SqlParameter("@ID",   c.Cur_ID) // Importante
                };
                DatabaseHelper.ExecuteNonQuery(sql, p);
            }















            // Dentro de ClasesBase.Repositories.CursoRepository

            /// <summary>
            /// Obtiene los cursos dictados por un docente, con su estado en texto.
            /// </summary>
            public DataTable GetCursosPorDocente(int idDocente)
            {
                string sql =
                    "SELECT " +
                    "   c.Cur_ID AS ID_Curso, " +
                    "   c.Cur_Nombre AS Curso, " +
                    "   e.Est_Nombre AS Estado " +
                    "FROM Curso c " +
                    "INNER JOIN Estado e ON c.Est_ID = e.Est_ID " +
                    "WHERE c.Doc_ID = @idDocente " +
                    "ORDER BY c.Cur_Nombre";

                SqlParameter[] parameters = {
        new SqlParameter("@idDocente", idDocente)
    };
                return DatabaseHelper.ExecuteQuery(sql, parameters);
            }

            /// <summary>
            /// Actualiza el estado de un curso. Si el estado es 'Cancelado', también cancela las inscripciones.
            /// </summary>
            public void ActualizarEstadoCurso(int idCurso, string nuevoEstadoNombre)
            {
                // 1. Obtener el ID del nuevo estado (usando tu método de búsqueda de estado)
                string queryEstado = "SELECT Est_ID FROM Estado WHERE Est_Nombre = @nombreEstado";
                SqlParameter[] estadoParams = { new SqlParameter("@nombreEstado", nuevoEstadoNombre) };
                DataTable dt = DatabaseHelper.ExecuteQuery(queryEstado, estadoParams);

                if (dt.Rows.Count == 0)
                {
                    throw new Exception("No se encontró el estado '" + nuevoEstadoNombre + "' en la base de datos.");
                }
                int nuevoEstadoID = Convert.ToInt32(dt.Rows[0]["Est_ID"]);

                SqlParameter[] updateParams = {
        new SqlParameter("@estadoID", nuevoEstadoID),
        new SqlParameter("@cursoID", idCurso)
    };

                // 2. Actualizar el curso
                string queryUpdateCurso = "UPDATE Curso SET Est_ID = @estadoID WHERE Cur_ID = @cursoID";
                DatabaseHelper.ExecuteNonQuery(queryUpdateCurso, updateParams);

                // 3. REGLA DE NEGOCIO: Actualizar las inscripciones si el curso fue CANCELADO.
                if (nuevoEstadoNombre == "Cancelado")
                {
                    string queryUpdateInscripciones = "UPDATE Inscripcion SET Est_ID = @estadoID WHERE Cur_ID = @cursoID";
                    DatabaseHelper.ExecuteNonQuery(queryUpdateInscripciones, updateParams);
                }
            }

            public Curso GetCursoById(int id)
            {
                string sql = "SELECT * FROM Curso WHERE Cur_ID = @id";
                SqlParameter[] p = { new SqlParameter("@id", id) };

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, p);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new Curso
                    {
                        Cur_ID = Convert.ToInt32(row["Cur_ID"]),
                        Cur_Nombre = row["Cur_Nombre"].ToString(),
                        Cur_Descripcion = row["Cur_Descripcion"].ToString(),
                        Cur_Cupo = Convert.ToInt32(row["Cur_Cupo"]),
                        Cur_FechaInicio = Convert.ToDateTime(row["Cur_FechaInicio"]),
                        Cur_FechaFin = Convert.ToDateTime(row["Cur_FechaFin"]),
                        Est_ID = Convert.ToInt32(row["Est_ID"]),
                        Doc_ID = Convert.ToInt32(row["Doc_ID"])
                    };
                }
                return null;
            }


        }
    }

