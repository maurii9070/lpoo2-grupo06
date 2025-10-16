using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Entidades;
using ClasesBase.Database;

namespace ClasesBase.Repositories
{
    public class EstadoTypeRepository
    {
        private const string Table = "EstadoType";

        public static List<EstadoType> ObtenerTodos()
        {
            List<EstadoType> lista = new List<EstadoType>();

            string sql = "SELECT Esty_ID, Esty_Nombre FROM " + Table + " ORDER BY Esty_Nombre";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql);

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new EstadoType
                {
                    Esty_ID = Convert.ToInt32(row["Esty_ID"]),
                    Esty_Nombre = row["Esty_Nombre"].ToString()
                });
            }

            return lista;
        }
    }
}