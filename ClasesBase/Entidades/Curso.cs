using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase.Entidades
{
    class Curso
    {
        public string Cur_ID { get; set; }
        public string Cur_Nombre { get; set; }
        public string Cur_Descripcion { get; set; }
        public int Cur_Cupo { get; set; }
        public DateTime Cur_FechaInicio { get; set; }
        public DateTime Cur_FechaFin { get; set; }
        public int Est_ID { get; set; }
        public int Doc_ID { get; set; }
    }
}
