using System;
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
                         "Doc_Apellido + ', ' + Doc_Nombre AS NombreCompleto, " + //Modicamos Display por NombreCompleto
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

        // Verificar si existe un docente con el mismo DNI
        public bool ExisteDNI(string dni, int? docenteIdExcluir = null)
        {
            string sql = "SELECT COUNT(*) FROM " + TableName + " WHERE Doc_DNI = @dni";
            
            if (docenteIdExcluir.HasValue)
            {
                sql += " AND Doc_ID != @id";
            }

            SqlParameter[] parametros;
            if (docenteIdExcluir.HasValue)
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@dni", dni ?? (object)DBNull.Value),
                    new SqlParameter("@id", docenteIdExcluir.Value)
                };
            }
            else
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@dni", dni ?? (object)DBNull.Value)
                };
            }

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parametros);
            
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }
            
            return false;
        }

        // Verificar si existe un docente con el mismo Email
        public bool ExisteEmail(string email, int? docenteIdExcluir = null)
        {
            // Si el email está vacío, no validamos
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string sql = "SELECT COUNT(*) FROM " + TableName + " WHERE Doc_Email = @email";
            
            if (docenteIdExcluir.HasValue)
            {
                sql += " AND Doc_ID != @id";
            }

            SqlParameter[] parametros;
            if (docenteIdExcluir.HasValue)
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@email", email),
                    new SqlParameter("@id", docenteIdExcluir.Value)
                };
            }
            else
            {
                parametros = new SqlParameter[] {
                    new SqlParameter("@email", email)
                };
            }

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parametros);
            
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }
            
            return false;
        }
    }
}