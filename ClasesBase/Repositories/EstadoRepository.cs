using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Entidades;
using ClasesBase.Database;

namespace ClasesBase.Repositories
{
    public class EstadoRepository
    {
        private const string Table = "Estado";

        // Obtiene todos los estados (sin filtrar)
        public static List<Estado> ObtenerTodos()
        {
            List<Estado> lista = new List<Estado>();
            string sql = "SELECT Est_ID, Est_Nombre, Esty_ID FROM " + Table + " ORDER BY Est_Nombre";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql);

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Estado
                {
                    Est_ID = Convert.ToInt32(row["Est_ID"]),
                    Est_Nombre = row["Est_Nombre"].ToString(),
                    Esty_ID = Convert.ToInt32(row["Esty_ID"])
                });
            }

            return lista;
        }

        // Obtiene estados por tipo (ej: "curso", "inscripcion")
        public DataTable GetByType(string typeName)
        {
            string sql = @"SELECT e.Est_ID, e.Est_Nombre 
                           FROM Estado e 
                           INNER JOIN EstadoType et ON e.Esty_ID = et.Esty_ID 
                           WHERE et.Esty_Nombre = @TypeName 
                           ORDER BY e.Est_Nombre";

            SqlParameter[] p = { new SqlParameter("@TypeName", typeName ?? (object)DBNull.Value) };
            return DatabaseHelper.ExecuteQuery(sql, p);
        }

        // Opcional: si querés una versión con objetos en vez de DataTable
        public static List<Estado> ObtenerPorTipo(string typeName)
        {
            List<Estado> lista = new List<Estado>();
            string sql = @"SELECT e.Est_ID, e.Est_Nombre, e.Esty_ID
                           FROM Estado e 
                           INNER JOIN EstadoType et ON e.Esty_ID = et.Esty_ID 
                           WHERE et.Esty_Nombre = @TypeName 
                           ORDER BY e.Est_Nombre";

            SqlParameter[] p = { new SqlParameter("@TypeName", typeName ?? (object)DBNull.Value) };
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, p);

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Estado
                {
                    Est_ID = Convert.ToInt32(row["Est_ID"]),
                    Est_Nombre = row["Est_Nombre"].ToString(),
                    Esty_ID = Convert.ToInt32(row["Esty_ID"])
                });
            }

            return lista;
        }
    }
}