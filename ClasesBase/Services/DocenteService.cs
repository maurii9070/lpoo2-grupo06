using System;
using System.Data;
using ClasesBase.Repositories;
using ClasesBase.Entidades;

namespace ClasesBase.Services
{
    public class DocenteService
    {
        private readonly DocenteRepository _repo = new DocenteRepository();

        public void GuardarDocente(Docente docente)
        {
            // Validar DNI duplicado
            if (_repo.ExisteDNI(docente.Doc_DNI))
            {
                throw new Exception("Ya existe un docente con el DNI '" + docente.Doc_DNI + "'");
            }

            // Validar Email duplicado (solo si tiene email)
            if (!string.IsNullOrWhiteSpace(docente.Doc_Email) && _repo.ExisteEmail(docente.Doc_Email))
            {
                throw new Exception("Ya existe un docente con el email '" + docente.Doc_Email + "'");
            }
            
            _repo.Add(docente);
        }

        public DataTable ObtenerDocentes()
        {
            return _repo.GetAll();
        }

        public void AgregarDocente(Docente docente)
        {
            _repo.Add(docente);
        }

        public bool ExisteDNI(string dni, int? docenteIdExcluir = null)
        {
            return _repo.ExisteDNI(dni, docenteIdExcluir);
        }

        public bool ExisteEmail(string email, int? docenteIdExcluir = null)
        {
            return _repo.ExisteEmail(email, docenteIdExcluir);
        }
    }
}