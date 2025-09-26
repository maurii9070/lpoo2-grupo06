using System.Data;
using System.Data.SqlClient;

namespace ClasesBase.Database
{
    public static class DatabaseHelper
    {

        private static readonly string _connectionString =
                                                            @"Data Source=.\SQLEXPRESS;" +
                                                            @"AttachDbFilename=|DataDirectory|\App_Data\instituto.mdf;" +
                                                            @"Integrated Security=True;User Instance=True";

        // USAR PARA TRAER DATOS (EJ. SELECT * .....)
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // USAR PARA MANDAR CAMBIOS (EJ. INSERT ......)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    cnn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
