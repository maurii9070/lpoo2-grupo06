using System;
using System.Data;
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
using ClasesBase.Services;
using ClasesBase.Entidades; // Necesario para List<Estado>

namespace Vistas.Cursos
{
    /// <summary>
    /// Interaction logic for CursosView.xaml
    /// </summary>
    public partial class CursosView : UserControl
    {
        // Servicios y variable para guardar el ID
        private CursoService _cursoService;
        private int _cursoIdSeleccionado = 0;

        public CursosView()
        {
            InitializeComponent();
            _cursoService = new CursoService();
            CargarGrilla();
            CargarCombosEdit(); // Carga los combos del panel de edición una vez
        }

        private void CargarGrilla()
        {
            DataTable dt = _cursoService.ObtenerCursos();

            bool hayDatos = dt.Rows.Count > 0;

            dgCursos.Visibility = hayDatos ? Visibility.Visible : Visibility.Collapsed;
            txtSinDatos.Visibility = hayDatos ? Visibility.Collapsed : Visibility.Visible;

            if (hayDatos)
                dgCursos.ItemsSource = dt.DefaultView;
            else
                dgCursos.ItemsSource = null;

            // Deshabilitamos el panel de edición al recargar
            gbEditarCurso.IsEnabled = false;
            LimpiarPanelEdicion();
        }

        private void BtnAltaCurso_Click(object sender, RoutedEventArgs e)
        {
            {
                AltaCursoView w = new AltaCursoView();
                if (w.ShowDialog() == true)
                {
                    CargarGrilla();
                }
            }
        }

        // --- SECCIÓN DE EDICIÓN ---

        private void CargarCombosEdit()
        {
            // Cargar ComboBox de Estados
            List<Estado> estados = EstadoService.ObtenerEstadosCurso();
            cboEstadoEdit.ItemsSource = estados;
            cboEstadoEdit.DisplayMemberPath = "Est_Nombre";
            cboEstadoEdit.SelectedValuePath = "Est_ID";

            // Cargar ComboBox de Docentes
            // (Asumo que tienes DocenteService por tu AltaCursoView)
            DataTable dtDoc = new DocenteService().ObtenerDocentes();
            cboDocenteEdit.ItemsSource = dtDoc.DefaultView;
            cboDocenteEdit.DisplayMemberPath = "Display";
            cboDocenteEdit.SelectedValuePath = "Doc_ID";
        }

        private void LimpiarPanelEdicion()
        {
            _cursoIdSeleccionado = 0;
            txtNombreEdit.Text = "";
            txtDescEdit.Text = "";
            txtCupoEdit.Text = "";
            cboEstadoEdit.SelectedValue = null;
            cboDocenteEdit.SelectedValue = null;
            // --- LÍNEAS NUEVAS ---
            dpFechaIniEdit.SelectedDate = null;
            dpFechaFinEdit.SelectedDate = null;
            // --- FIN LÍNEAS NUEVAS ---
            lblMensaje.Text = "";
        }

        private void dgCursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Este código es correcto, PERO depende de que
            // CursoRepository.GetAll() incluya 'Est_ID' y 'Doc_ID'
            if (dgCursos.SelectedItem != null)
            {
                // Habilitamos el panel
                gbEditarCurso.IsEnabled = true;
                lblMensaje.Text = "";

                // Obtenemos la fila seleccionada
                DataRowView fila = (DataRowView)dgCursos.SelectedItem;

                // Guardamos el ID del curso
                _cursoIdSeleccionado = (int)fila["Cur_ID"];

                // Rellenamos los campos del panel de edición
                txtNombreEdit.Text = fila["Cur_Nombre"].ToString();
                txtDescEdit.Text = fila["Cur_Descripcion"].ToString();
                txtCupoEdit.Text = fila["Cur_Cupo"].ToString();

                // Estas líneas causan el error si 'Est_ID' o 'Doc_ID' no están en la consulta
                cboEstadoEdit.SelectedValue = (int)fila["Est_ID"];
                cboDocenteEdit.SelectedValue = (int)fila["Doc_ID"];

                // --- LÍNEAS NUEVAS (Lógica de Fechas Mejorada) ---

                // 1. Cargar las fechas del curso seleccionado
                DateTime fechaInicio = (DateTime)fila["Cur_FechaInicio"];
                dpFechaIniEdit.SelectedDate = fechaInicio;
                dpFechaFinEdit.SelectedDate = (DateTime)fila["Cur_FechaFin"];

                // 2. Lógica para "Fecha Inicio"
                // El usuario no puede mover la fecha de inicio a un día anterior a HOY,
                // A MENOS QUE el curso ya haya comenzado en el pasado.
                if (fechaInicio < DateTime.Today)
                {
                    // Si el curso ya comenzó, la fecha mínima seleccionable
                    // es la fecha en que comenzó (no se puede mover más al pasado).
                    dpFechaIniEdit.DisplayDateStart = fechaInicio;
                }
                else
                {
                    // Si el curso es futuro, la fecha mínima seleccionable es HOY.
                    dpFechaIniEdit.DisplayDateStart = DateTime.Today;
                }

                // 3. Lógica para "Fecha Fin"
                // (Esta lógica ya la tenías, solo la movemos aquí para agrupar)
                // La fecha de fin no puede ser anterior a la de inicio.
                dpFechaFinEdit.DisplayDateStart = fechaInicio;
                // --- FIN LÍNEAS NUEVAS ---
            }
        }

        // --- MÉTODO NUEVO (para la lógica de fechas) ---
        private void dpFechaIniEdit_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpFechaIniEdit.SelectedDate.HasValue && dpFechaFinEdit != null)
            {
                // Actualizamos la fecha mínima del DatePicker de Fin
                dpFechaFinEdit.DisplayDateStart = dpFechaIniEdit.SelectedDate.Value;

                // Si la fecha de fin actual es inválida, la limpiamos
                if (dpFechaFinEdit.SelectedDate.HasValue && dpFechaFinEdit.SelectedDate < dpFechaFinEdit.DisplayDateStart)
                {
                    dpFechaFinEdit.SelectedDate = null;
                }
            }
        }
        // --- FIN MÉTODO NUEVO ---

        private void btnGuardarEdicion_Click(object sender, RoutedEventArgs e)
        {
            if (_cursoIdSeleccionado == 0) return; // No hay nada seleccionado

            int cupo;
            if (!int.TryParse(txtCupoEdit.Text, out cupo) || cupo <= 0)
            {
                MessageBox.Show("El cupo debe ser un número mayor a 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // --- VALIDACIÓN DE FECHAS ---
            if (dpFechaIniEdit.SelectedDate == null || dpFechaFinEdit.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una fecha de inicio y fin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpFechaFinEdit.SelectedDate < dpFechaIniEdit.SelectedDate)
            {
                MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // --- FIN VALIDACIÓN ---

            // Creamos el objeto Curso con los datos actualizados
            Curso cursoEditado = new Curso
            {
                Cur_ID = _cursoIdSeleccionado,
                Cur_Nombre = txtNombreEdit.Text,
                Cur_Descripcion = txtDescEdit.Text,
                Cur_Cupo = cupo,
                // --- LÍNEAS NUEVAS ---
                Cur_FechaInicio = dpFechaIniEdit.SelectedDate.Value,
                Cur_FechaFin = dpFechaFinEdit.SelectedDate.Value,
                // --- FIN LÍNEAS NUEVAS ---
                Est_ID = (int)cboEstadoEdit.SelectedValue,
                Doc_ID = (int)cboDocenteEdit.SelectedValue
            };

            try
            {
                // Llamamos al servicio para actualizar
                _cursoService.ActualizarCurso(cursoEditado);

                // Mostramos un mensaje de éxito y recargamos la grilla
                lblMensaje.Text = "¡Guardado con éxito!";
                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Filtro para que el TextBox de Cupo solo acepte números
        private void txtCupoEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}