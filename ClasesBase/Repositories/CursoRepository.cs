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

            public DataTable GetAll()
            {
                string sql = "SELECT c.Cur_ID, " +
                             "c.Cur_Nombre, " +
                             "c.Cur_Descripcion, " +
                             "c.Cur_Cupo, " +
                             "c.Cur_FechaInicio, " +
                             "c.Cur_FechaFin, " +
                             "e.Est_Nombre AS Estado, " +
                             "d.Doc_Apellido + ', ' + d.Doc_Nombre AS Docente " +
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
        }
    }

