using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;
using ClasesBase.Repositories;
using System.Data;

namespace ClasesBase.Services
{
    public class EstadoService
    {
        private readonly EstadoRepository _repo = new EstadoRepository();

        public static List<Estado> ObtenerEstadosCurso()
        {
            return EstadoRepository.ObtenerEstadosCurso();
        }

        public static List<EstadoType> ObtenerTiposEstado()
        {
            return EstadoTypeRepository.ObtenerTodos();
        }

        public DataTable ObtenerEstadosDeCurso()
        {
            return _repo.GetByType("curso");   // mismo nombre que figura en EstadoType
        }
    }
}
