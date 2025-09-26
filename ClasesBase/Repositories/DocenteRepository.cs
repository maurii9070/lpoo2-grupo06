using System.Data;
using System.Data.SqlClient;
using ClasesBase.Database;
using ClasesBase.Entidades;

namespace ClasesBase.Repositories
{
    public class DocenteRepository
    {
        private const string TableName = "Docente";

        // Obtener todos los docentes
        public DataTable GetAll()
        {
            string sql = "SELECT Doc_ID, " +
                         "Doc_DNI, " +
                         "Doc_Apellido + ', ' + Doc_Nombre AS Display, " +
                         "Doc_Email " +
                         "FROM " + TableName + " " +
                         "ORDER BY Doc_Apellido, Doc_Nombre";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        // Agregar Docente
        public void Add(Docente doc)
        {
            string sql = "INSERT INTO " + TableName +
                         " (Doc_DNI, Doc_Apellido, Doc_Nombre, Doc_Email) " +
                         "VALUES (@DNI, @Apellido, @Nombre, @Email)";

            SqlParameter[] p =
            {
                new SqlParameter("@DNI",      doc.Doc_DNI),
                new SqlParameter("@Apellido", doc.Doc_Apellido),
                new SqlParameter("@Nombre",   doc.Doc_Nombre),
                new SqlParameter("@Email",    doc.Doc_Email)
            };

            DatabaseHelper.ExecuteNonQuery(sql, p);
        }
    }
}