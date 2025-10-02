using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClasesBase.Entidades;
using ClasesBase.Services;

namespace Vistas.Alumnos
{
    /// <summary>
    /// Interaction logic for EditarAlumno.xaml
    /// </summary>

    public partial class EditarAlumno : Window
    {

        private Alumno oAlumno;
        private AlumnoService alumnoService;
        private TrabajarAlumno trabajarAlumno;


        public EditarAlumno()
        {
            InitializeComponent();
            alumnoService = new AlumnoService();
            trabajarAlumno = new TrabajarAlumno();

        }

        // Buscar alumno por ID
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txtID.Text, out id))
            {
                oAlumno = trabajarAlumno.TraerAlumno(id);
                if (oAlumno != null)
                {
                    DataContext = oAlumno;
                }
                else
                {
                    MessageBox.Show("Alumno no encontrado");
                    oAlumno = new Alumno
                    {
                        Alu_DNI = " ",
                        Alu_Apellido = " ",
                        Alu_Nombre = " ",
                        Alu_Email = " "
                    };
                    DataContext = oAlumno;
                }
            }
            else
            {
                MessageBox.Show("Ingrese un ID");
            }
        }

        // Guardar cambios
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Evita tratar de guardar sin haber buscado un alumno previamente
            if (oAlumno == null)
            {
                MessageBox.Show("Primero debes buscar un alumno");
                return;
            }

            // Validación antes de actualizar
            if (!string.IsNullOrEmpty(oAlumno["Alu_DNI"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Apellido"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Nombre"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Email"]))
            {
                MessageBox.Show("Todos los campos deben estar ingresados");
                return;
            }

            alumnoService.ActualizarAlumno(oAlumno);
            MessageBox.Show("Alumno actualizado");
            this.Close();

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}