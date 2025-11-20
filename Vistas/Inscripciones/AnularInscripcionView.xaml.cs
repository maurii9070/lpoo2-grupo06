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
using ClasesBase.Services; // Agregamos el namespace de Services
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

        // Usamos el servicio para la anulación
        private InscripcionService _inscripcionService;

        private Alumno _alumnoEncontrado;

        public AnularInscripcionView()
        {
            InitializeComponent();
            _alumnoRepo = new AlumnoRepository();
            _inscripcionRepo = new InscripcionRepository();
            _inscripcionService = new InscripcionService(); // Inicializamos
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string input = txtDNI_o_ID.Text;

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Por favor, ingrese un DNI o un ID.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LimpiarCampos();

            _alumnoEncontrado = _alumnoRepo.GetAlumnoByDNI(input);

            if (_alumnoEncontrado == null)
            {
                int id;
                if (int.TryParse(input, out id))
                {
                    _alumnoEncontrado = _alumnoRepo.GetAlumnoById(id);
                }
            }

            if (_alumnoEncontrado == null)
            {
                txtNombreAlumno.Text = "Alumno no encontrado (se buscó por DNI e ID).";
                txtNombreAlumno.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            txtNombreAlumno.Text = "Alumno: " + _alumnoEncontrado.Alu_Apellido + ", " + _alumnoEncontrado.Alu_Nombre;
            txtNombreAlumno.Foreground = System.Windows.Media.Brushes.DarkBlue;

            DataTable dtCursos = _inscripcionRepo.getInscripcionesActivasPorDNI(_alumnoEncontrado.Alu_DNI);

            if (dtCursos.Rows.Count == 0)
            {
                txtNombreAlumno.Text += "\n(El alumno no tiene inscripciones activas para anular)";
                return;
            }

            cmbCursos.ItemsSource = dtCursos.DefaultView;
            cmbCursos.DisplayMemberPath = "Curso";

            // El ValuePath es "ID" (Ins_ID), pero necesitamos también el ID_Curso
            // Lo sacaremos del SelectedItem al hacer click
            cmbCursos.SelectedValuePath = "ID";

            gbCursos.IsEnabled = true;
            btnAnular.IsEnabled = true;
        }

        private void btnAnular_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCursos.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione la inscripción que desea anular.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtenemos los datos de la fila seleccionada
            DataRowView filaSeleccionada = (DataRowView)cmbCursos.SelectedItem;
            string nombreCurso = filaSeleccionada["Curso"].ToString();
            int idInscripcion = Convert.ToInt32(filaSeleccionada["ID"]);
            int idCurso = Convert.ToInt32(filaSeleccionada["ID_Curso"]); // Obtenemos el ID del curso desde la fila

            MessageBoxResult result = MessageBox.Show(
                "¿Está seguro de que desea anular la inscripción al curso '" + nombreCurso + "'?",
                "Confirmar Anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // CAMBIO IMPORTANTE:
                    // Usamos el servicio en lugar del repositorio directo
                    // y pasamos el idCurso para que pueda verificar el cupo.
                    _inscripcionService.AnularInscripcion(idInscripcion, idCurso);

                    MessageBox.Show("¡Inscripción anulada exitosamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

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