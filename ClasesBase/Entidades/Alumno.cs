using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClasesBase.Entidades
{
    public class Alumno : IDataErrorInfo
    {
        public int Alu_ID { get; set; }
        public string Alu_DNI { get; set; }
        public string Alu_Apellido { get; set; }
        public string Alu_Nombre { get; set; }
        public string Alu_Email { get; set; }


        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string msg_error = null;

                switch (columnName)
                {
                    case "Alu_DNI":
                        msg_error = Validar_DNI();
                        break;
                    case "Alu_Apellido":
                        msg_error = Validar_Apellido();
                        break;
                    case "Alu_Nombre":
                        msg_error = Validar_Nombre();
                        break;
                    case "Alu_Email":
                        msg_error = Validar_Email();
                        break;
                }

                return msg_error;
            }
        }

        // Métodos de validación
        private string Validar_DNI()
        {

            if (String.IsNullOrEmpty(Alu_DNI))
            {
                return "El DNI es obligatorio";
            }
            return null;
        }

        private string Validar_Apellido()
        {
            if (String.IsNullOrEmpty(Alu_Apellido))
            {
                return "El Apellido es obligatorio";
            }
            return null;
        }

        private string Validar_Nombre()
        {
            if (String.IsNullOrEmpty(Alu_Nombre))
            {
                return "El Nombre es obligatorio";
            }
            return null;
        }

        private string Validar_Email()
        {
            if (String.IsNullOrEmpty(Alu_Email))
            {
                return "El Email es obligatorio";
            }
            return null;
        }

    }
}
