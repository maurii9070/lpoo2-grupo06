using System;
using System.Data; // Para DataTable
using System.Windows;
using System.Windows.Controls;
using ClasesBase.Services; // Usamos el Service directamente

namespace Vistas.Alumnos
{
    public partial class GestionAlumnoView : UserControl
    {
        private AlumnoService _service;

        public GestionAlumnoView()
        {
            InitializeComponent();
            _service = new AlumnoService();
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            // Obtenemos el DataTable desde el servicio
            DataTable dt = _service.ObtenerAlumnos();
            dgAlumnos.ItemsSource = dt.DefaultView;
        }

        private void btnAltaAlumno_Click(object sender, RoutedEventArgs e)
        {
            AltaAlumno alta = new AltaAlumno();
            // Usamos ShowDialog para esperar a que cierre
            if (alta.ShowDialog() == true)
            {
                // Si se guardó correctamente, refrescamos
                CargarGrilla();
            }
            // Si canceló o cerró, también podríamos refrescar por seguridad:
            else
            {
                CargarGrilla();
            }
        }

        private void btnEditarAlumno_Click(object sender, RoutedEventArgs e)
        {
            EditarAlumno editar = new EditarAlumno();
            // Usamos ShowDialog para esperar a que cierre y refrescar la lista
            if (editar.ShowDialog() == true)
            {
                CargarGrilla();
            }
            else
            {
                CargarGrilla();
            }
        }
    }
}