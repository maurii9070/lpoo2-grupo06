using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClasesBase.Entidades;

namespace ClasesBase.Repositories
{
    public class CursoRepository
    {
        
            private static List<Curso> _cursos = new List<Curso>();
            private static int _contadorId = 1; // Contador numérico para generar IDs

            public static void AgregarCurso(Curso curso)
            {
                // Convertir el contador numérico a string
                curso.Cur_ID = _contadorId.ToString();
                _cursos.Add(curso);
                _contadorId++; // Incrementar para el próximo curso
            }

            public static List<Curso> ObtenerTodos()
            {
                return _cursos;
            }
        }
    }

