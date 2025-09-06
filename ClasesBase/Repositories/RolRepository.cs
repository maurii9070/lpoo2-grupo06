using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;

namespace ClasesBase.Repositories
{
    public class RolRepository
    {
        private static readonly List<Rol> _roles = new List<Rol>
        {
            new Rol{Rol_ID=1, Rol_Descripcion="Administrador"},
            new Rol{Rol_ID=2, Rol_Descripcion="Docente"},
            new Rol{Rol_ID=3, Rol_Descripcion="Recepcion"}
        };

        public Rol Obtener(int id)
        {
            return _roles.Find(r => r.Rol_ID == id);
        }
    }
}
