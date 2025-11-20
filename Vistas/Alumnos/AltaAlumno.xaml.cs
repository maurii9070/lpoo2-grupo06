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
using ClasesBase.Repositories;
using ClasesBase.Services;

namespace Vistas.Alumnos
{
    /// <summary>
    /// Interaction logic for AltaAlumno.xaml
    /// </summary>
    public partial class AltaAlumno : Window
    {

        private Alumno oAlumno;
        private AlumnoService alumnoService;


        public AltaAlumno()
        {
            InitializeComponent();
            oAlumno = new Alumno();
            alumnoService = new AlumnoService();
            DataContext = oAlumno;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Validación antes de guardar
            if (!string.IsNullOrEmpty(oAlumno["Alu_DNI"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Apellido"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Nombre"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Email"]))
            {
                MessageBox.Show("Todos los campos deben estar ingresados");
                return;
            }

            // Mostrar confirmación
            MessageBoxResult result = MessageBox.Show(
                "¿Desea guardar este alumno?\n\n" +
                "DNI: " + oAlumno.Alu_DNI + "\n" +
                "Apellido: " + oAlumno.Alu_Apellido + "\n" +
                "Nombre: " + oAlumno.Alu_Nombre + "\n" +
                "Email: " + oAlumno.Alu_Email,
                "Confirmar alta",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    alumnoService.CrearAlumno(oAlumno);

                    MessageBox.Show("Alumno guardado con éxito.", "Alta Alumno", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("El alta fue cancelada.", "Alta Alumno", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
