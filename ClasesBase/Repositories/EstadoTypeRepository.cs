using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClasesBase.Entidades; 
namespace ClasesBase.Repositories
{
    public class EstadoTypeRepository
    {
        private static List<EstadoType> _estadoTypes = new List<EstadoType>
        {
            new EstadoType { Esty_ID = 1, Esty_Nombre = "curso" },
            new EstadoType { Esty_ID = 2, Esty_Nombre = "inscripcion" }
        };

        public static List<EstadoType> ObtenerTodos()
        {
            return _estadoTypes;
        }
    }
}
