using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;
namespace ClasesBase.Services
{
    public class EstadoService
    {
        public static List<Estado> ObtenerEstadosCurso()
        {
            return EstadoRepository.ObtenerEstadosCurso();
        }

        public static List<EstadoType> ObtenerTiposEstado()
        {
            return EstadoTypeRepository.ObtenerTodos();
        }
    }
}
