using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using System.Data;
using System.Data.SqlClient;
using ClasesBase.Database;

namespace ClasesBase.Repositories
{
    public class EstadoRepository
    {
       private static List<Estado> _estados = new List<Estado>
        {
            // Estados de Curso (Esty_ID = 1)
            new Estado { Est_ID = 1, Est_Nombre = "Programado", Esty_ID = 1 },
            new Estado { Est_ID = 2, Est_Nombre = "En Curso", Esty_ID = 1 },
            new Estado { Est_ID = 3, Est_Nombre = "Finalizado", Esty_ID = 1 },
            new Estado { Est_ID = 4, Est_Nombre = "Cancelado", Esty_ID = 1 },
            
            // Estados de Inscripción (Esty_ID = 2)
            new Estado { Est_ID = 5, Est_Nombre = "Inscripto", Esty_ID = 2 },
            new Estado { Est_ID = 6, Est_Nombre = "Confirmado", Esty_ID = 2 },
            new Estado { Est_ID = 7, Est_Nombre = "Cancelado", Esty_ID = 2 }
        };

        public static List<Estado> ObtenerEstadosCurso()
        {
            return _estados.FindAll(e => e.Esty_ID == 1); // 1 = Estados de curso
        }

        public static List<Estado> ObtenerTodos()
        {
            return _estados;
        }

        public DataTable GetByType(string typeName)
        {
            string sql = "SELECT e.Est_ID, e.Est_Nombre " +
                         "FROM Estado e " +
                         "INNER JOIN EstadoType et ON e.Esty_ID = et.Esty_ID " +
                         "WHERE et.Esty_Nombre = @TypeName " +
                         "ORDER BY e.Est_Nombre";
            SqlParameter[] p = { new SqlParameter("@TypeName", typeName) };
            return DatabaseHelper.ExecuteQuery(sql, p);
        }
    
    }
}
