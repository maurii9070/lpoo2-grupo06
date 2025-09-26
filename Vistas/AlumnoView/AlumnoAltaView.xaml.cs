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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClasesBase.Entidades;
using ClasesBase.Services;
namespace Vistas
{
    /// <summary>
    /// Interaction logic for AlumnoAltaView.xaml
    /// </summary>
    public partial class AlumnoAltaView : UserControl
    {
        private AlumnoService service;

        public AlumnoAltaView()
        {
            InitializeComponent();
            service = new AlumnoService();
        }
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Creamos el objeto Alumno pero todavía no lo guardamos
            Alumno oAlumno = new Alumno();
            oAlumno.Alu_DNI = txtDNI.Text;
            oAlumno.Alu_Apellido = txtApellido.Text;
            oAlumno.Alu_Nombre = txtNombre.Text;
            oAlumno.Alu_Email = txtEmail.Text;

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
                // Solo si confirma lo guardamos
                service.CrearAlumno(oAlumno);

                MessageBox.Show("Alumno guardado con éxito.",
                                "Alta Alumno",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
    
                // limpiar
                txtDNI.Clear();
                txtApellido.Clear();
                txtNombre.Clear();
                txtEmail.Clear();
            }
            else
            {
                // Si cancela
                MessageBox.Show("El alta fue cancelada.",
                                "Alta Alumno",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
            }
        }
    }
}
