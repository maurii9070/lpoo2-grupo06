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
using ClasesBase.Repositories;
using ClasesBase.Entidades;
using System.Data;

namespace Vistas.Inscripciones
{
    /// <summary>
    /// Interaction logic for AnularInscripcionView.xaml
    /// </summary>
    public partial class AnularInscripcionView : Window
    {
        private AlumnoRepository _alumnoRepo;
        private InscripcionRepository _inscripcionRepo;
        private Alumno _alumnoEncontrado;

        public AnularInscripcionView()
        {
            InitializeComponent();
            _alumnoRepo = new AlumnoRepository();
            _inscripcionRepo = new InscripcionRepository();
        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Tomamos el valor del único TextBox
            string input = txtDNI_o_ID.Text;

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Por favor, ingrese un DNI o un ID.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LimpiarCampos();

            // --- INICIO DE LA SECUENCIA "OR" ---

            // 2. Primero, intentamos buscar por DNI (como string)
            _alumnoEncontrado = _alumnoRepo.GetAlumnoByDNI(input);

            // 3. Si no se encontró (es null), intentamos buscar por ID
            if (_alumnoEncontrado == null)
            {
                // Corrección para VS2010: Declarar 'id' primero
                int id;

                // Intentamos convertir el input a número
                if (int.TryParse(input, out id))
                {
                    // Si se pudo convertir, buscamos por ese ID
                    _alumnoEncontrado = _alumnoRepo.GetAlumnoById(id);
                }
            }

            // --- FIN DE LA SECUENCIA "OR" ---

            // 4. Validamos el resultado final
            if (_alumnoEncontrado == null)
            {
                txtNombreAlumno.Text = "Alumno no encontrado (se buscó por DNI e ID).";
                txtNombreAlumno.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // --- Si llegamos aquí, encontramos al alumno ---

            txtNombreAlumno.Text = "Alumno: " + _alumnoEncontrado.Alu_Apellido + ", " + _alumnoEncontrado.Alu_Nombre;
            txtNombreAlumno.Foreground = System.Windows.Media.Brushes.DarkBlue;

            // Usamos el DNI del alumno encontrado para buscar sus cursos
            DataTable dtCursos = _inscripcionRepo.getInscripcionesActivasPorDNI(_alumnoEncontrado.Alu_DNI);

            if (dtCursos.Rows.Count == 0)
            {
                txtNombreAlumno.Text += "\n(El alumno no tiene inscripciones activas para anular)";
                return;
            }

            cmbCursos.ItemsSource = dtCursos.DefaultView;
            cmbCursos.DisplayMemberPath = "Curso";
            cmbCursos.SelectedValuePath = "ID";

            gbCursos.IsEnabled = true;
            btnAnular.IsEnabled = true;
        }

        private void btnAnular_Click(object sender, RoutedEventArgs e)
        {
            // Validación: ¿Se seleccionó un curso?
            if (cmbCursos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione la inscripción que desea anular.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirmación
            MessageBoxResult result = MessageBox.Show(
                "¿Está seguro de que desea anular la inscripción al curso '" + ((DataRowView)cmbCursos.SelectedItem)["Curso"] + "'?",
                "Confirmar Anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Obtener el ID de la inscripción (Ins_ID)
                    int inscripcionID = (int)cmbCursos.SelectedValue;

                    // Llamar al repositorio para anular
                    _inscripcionRepo.anularInscripcion(inscripcionID);

                    MessageBox.Show("¡Inscripción anulada exitosamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Cerramos el formulario y avisamos que fue exitoso
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al anular la inscripción:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void LimpiarCampos()
        {
            _alumnoEncontrado = null;
            txtNombreAlumno.Text = string.Empty;
            cmbCursos.ItemsSource = null;
            gbCursos.IsEnabled = false;
            btnAnular.IsEnabled = false;
        }
    }
}
