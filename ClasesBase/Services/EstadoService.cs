using System;
using System.Collections.Generic;
using ClasesBase.Entidades;
using ClasesBase.Repositories;
using System.Data;

namespace ClasesBase.Services
{
    public class EstadoService
    {
        // Métodos estáticos son aceptables si no tienen estado
        public static List<EstadoType> ObtenerTiposEstado()
        {
            return EstadoTypeRepository.ObtenerTodos();
        }

        public static List<Estado> ObtenerEstadosCurso()
        {
            return EstadoRepository.ObtenerPorTipo("curso");
        }
        public static List<Estado> ObtenerEstadosInscripcion() {
            return EstadoRepository.ObtenerPorTipo("inscripcion");
        }
        
    }
}