using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using ClasesBase.Services;
using ClasesBase.Entidades;

namespace Vistas.Inscripciones
{
    public partial class AcreditacionUC : UserControl
    {
        private AlumnoService _alumnoService;
        private InscripcionService _inscripcionService;
        private Alumno _alumnoActual;

        public AcreditacionUC()
        {
            InitializeComponent();
            _alumnoService = new AlumnoService();
            _inscripcionService = new InscripcionService();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtDNI.Text.Trim();
            if (string.IsNullOrEmpty(textoBusqueda))
            {
                MessageBox.Show("Por favor ingrese un DNI o un ID.");
                return;
            }

            _alumnoActual = null;

            int idBusqueda;
            bool esNumero = int.TryParse(textoBusqueda, out idBusqueda);

            if (esNumero)
            {
                // Si es un número, intentamos buscar por ID primero
                _alumnoActual = _alumnoService.ObtenerAlumnoPorID(idBusqueda);
            }

            // Si no es un número, o si la búsqueda por ID falló (devuelve null),
            // intentamos buscar por DNI.
            if (_alumnoActual == null)
            {
                _alumnoActual = _alumnoService.ObtenerAlumnoPorDNI(textoBusqueda);
            }


            if (_alumnoActual != null)
            {
                lblNombreAlumno.Text = "Alumno: " + _alumnoActual.Alu_Apellido + ", " + _alumnoActual.Alu_Nombre;
                CargarInscripciones();
                lblMensaje.Text = "";
            }
            else
            {
                lblNombreAlumno.Text = "";
                dgInscripciones.ItemsSource = null;
                MessageBox.Show("Alumno no encontrado (ni por ID ni por DNI).");
            }

            btnAcreditar.IsEnabled = false;
        }

        private void CargarInscripciones()
        {
            if (_alumnoActual != null)
            {
                DataTable dt = _inscripcionService.ObtenerInscripcionesDeAlumno(_alumnoActual.Alu_ID);
                dgInscripciones.ItemsSource = dt.DefaultView;
            }
        }

        private void dgInscripciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgInscripciones.SelectedItem == null)
            {
                btnAcreditar.IsEnabled = false;
                return;
            }

            DataRowView fila = (DataRowView)dgInscripciones.SelectedItem;

            string estadoCurso = fila["EstadoCurso"].ToString();
            string estadoInscripcion = fila["EstadoInscripcion"].ToString();

            if (estadoCurso.Equals("en_curso", StringComparison.InvariantCultureIgnoreCase) &&
                !estadoInscripcion.Equals("confirmado", StringComparison.InvariantCultureIgnoreCase))
            {
                btnAcreditar.IsEnabled = true;
                lblMensaje.Text = "Listo para acreditar.";
                lblMensaje.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                btnAcreditar.IsEnabled = false;
                if (!estadoCurso.Equals("en_curso", StringComparison.InvariantCultureIgnoreCase))
                {
                    lblMensaje.Text = "No se puede acreditar: El curso no está 'En Curso'.";
                }
                else
                {
                    lblMensaje.Text = "La inscripción ya está confirmada.";
                }
                lblMensaje.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void btnAcreditar_Click(object sender, RoutedEventArgs e)
        {
            if (dgInscripciones.SelectedItem != null)
            {
                try
                {
                    DataRowView fila = (DataRowView)dgInscripciones.SelectedItem;
                    int idInscripcion = Convert.ToInt32(fila["Ins_ID"]);

                    _inscripcionService.AcreditarInscripcion(idInscripcion);

                    MessageBox.Show("¡Acreditación exitosa!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    CargarInscripciones();
                    btnAcreditar.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al acreditar: " + ex.Message);
                }
            }
        }
    }
}