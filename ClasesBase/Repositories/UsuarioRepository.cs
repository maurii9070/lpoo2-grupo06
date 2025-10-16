using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ClasesBase.Database;

namespace ClasesBase.Repositories
{
    public class UsuarioRepository
    {
        private static readonly List<Usuario> _usuarios = new List<Usuario>
        {
            new Usuario{ Usu_ID=1, Usu_NombreUsuario="admin",
                         Usu_Contrasenia="1234", Usu_ApellidoNombre="Admin, Root", Rol_ID=1 },
            new Usuario{ Usu_ID=2, Usu_NombreUsuario="docente",
                         Usu_Contrasenia="1234", Usu_ApellidoNombre="Perez, Juan", Rol_ID=2 },
            new Usuario{ Usu_ID=3, Usu_NombreUsuario="recepcion",
                         Usu_Contrasenia="1234", Usu_ApellidoNombre="Gomez, Ana", Rol_ID=3 }
        };

        public Usuario ObtenerPorUsuarioYClave(string usuario, string clave)
        {
            return _usuarios
                   .FirstOrDefault(u => u.Usu_NombreUsuario == usuario &&
                                        u.Usu_Contrasenia == clave);
        }

        private const string Table = "Usuario";

        public ObservableCollection<Usuario> GetAll()
        {
            ObservableCollection<Usuario> list = new ObservableCollection<Usuario>();

            string sql = "SELECT Usu_ID, Usu_NombreUsuario, Usu_Contrasenia, " +
                         "Usu_ApellidoNombre, Rol_ID FROM " + Table +
                         " ORDER BY Usu_ApellidoNombre, Usu_NombreUsuario";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql);

            foreach (DataRow r in dt.Rows)
            {
                Usuario u = new Usuario
                {
                    Usu_ID = Convert.ToInt32(r["Usu_ID"]),
                    Usu_NombreUsuario = r["Usu_NombreUsuario"].ToString(),
                    Usu_Contrasenia = r["Usu_Contrasenia"].ToString(),
                    Usu_ApellidoNombre = r["Usu_ApellidoNombre"].ToString(),
                    Rol_ID = Convert.ToInt32(r["Rol_ID"])
                };
                list.Add(u);
            }
            return list;
        }

        public void Add(Usuario u)
        {
            string sql = "INSERT INTO " + Table + " (Usu_NombreUsuario, Usu_Contrasenia, " +
                         "Usu_ApellidoNombre, Rol_ID) VALUES (@nom,@pass,@ape,@rol)";
            SqlParameter[] p = {
                new SqlParameter("@nom",  u.Usu_NombreUsuario),
                new SqlParameter("@pass", u.Usu_Contrasenia),
                new SqlParameter("@ape",  u.Usu_ApellidoNombre),
                new SqlParameter("@rol",  u.Rol_ID)
            };
            DatabaseHelper.ExecuteNonQuery(sql, p);
        }

        public void Update(Usuario u)
        {
            string sql = "UPDATE " + Table + " SET Usu_NombreUsuario=@nom, " +
                         "Usu_Contrasenia=@pass, Usu_ApellidoNombre=@ape, Rol_ID=@rol " +
                         "WHERE Usu_ID=@id";
            SqlParameter[] p = {
                new SqlParameter("@nom",  u.Usu_NombreUsuario),
                new SqlParameter("@pass", u.Usu_Contrasenia),
                new SqlParameter("@ape",  u.Usu_ApellidoNombre),
                new SqlParameter("@rol",  u.Rol_ID),
                new SqlParameter("@id",   u.Usu_ID)
            };
            DatabaseHelper.ExecuteNonQuery(sql, p);
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM " + Table + " WHERE Usu_ID=@id";
            SqlParameter[] p = { new SqlParameter("@id", id) };
            DatabaseHelper.ExecuteNonQuery(sql, p);
        }
    }
    
}
