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
        // Eliminado: private TrabajarAlumno trabajarAlumno;

        public EditarAlumno()
        {
            InitializeComponent();
            alumnoService = new AlumnoService();
            // trabajarAlumno = new TrabajarAlumno();
        }

        // Buscar alumno por ID o DNI
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string entrada = txtBusqueda.Text; // Antes txtID

            if (string.IsNullOrEmpty(entrada))
            {
                MessageBox.Show("Ingrese un ID o DNI para buscar");
                return;
            }

            oAlumno = null;

            // 1. Intentamos buscar por ID (si es número)
            int id;
            if (int.TryParse(entrada, out id))
            {
                oAlumno = alumnoService.ObtenerAlumnoPorID(id);
            }

            // 2. Si no encontró por ID (o no era número), buscamos por DNI
            if (oAlumno == null)
            {
                oAlumno = alumnoService.ObtenerAlumnoPorDNI(entrada);
            }

            // 3. Resultado
            if (oAlumno != null)
            {
                DataContext = oAlumno;
            }
            else
            {
                MessageBox.Show("Alumno no encontrado");
                // Limpiamos creando un alumno vacío para que se borren los campos visualmente
                oAlumno = new Alumno();
                DataContext = oAlumno;
            }
        }

        // Guardar cambios
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Evita tratar de guardar sin haber buscado un alumno válido previamente
            if (oAlumno == null || oAlumno.Alu_ID == 0)
            {
                MessageBox.Show("Primero debes buscar un alumno válido");
                return;
            }

            // Validación antes de actualizar (Mantenemos tu lógica original)
            // Verifica si el indexer de validación retorna algún mensaje de error
            if (!string.IsNullOrEmpty(oAlumno["Alu_DNI"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Apellido"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Nombre"]) ||
                !string.IsNullOrEmpty(oAlumno["Alu_Email"]))
            {
                MessageBox.Show("Todos los campos deben estar ingresados correctamente");
                return;
            }

            try
            {
                alumnoService.ActualizarAlumno(oAlumno);
                MessageBox.Show("Alumno actualizado");

                // Importante: Establecemos DialogResult true para que la ventana padre sepa que hubo cambios
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}