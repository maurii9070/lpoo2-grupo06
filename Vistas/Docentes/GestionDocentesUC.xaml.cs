using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ClasesBase.Services;

namespace Vistas.Docentes
{
    /// <summary>
    /// Lógica de interacción para DocentesView.xaml
    /// Integra Alta de Docente y Gestión de sus cursos en una sola pantalla.
    /// </summary>
    public partial class DocentesView : UserControl
    {
        private DocenteService _docenteService;
        private CursoService _cursoService;

        public DocentesView()
        {
            InitializeComponent();
            _docenteService = new DocenteService();
            _cursoService = new CursoService();

            CargarDocentes();
        }

        // --- Carga inicial del ComboBox ---
        private void CargarDocentes()
        {
            DataTable dt = _docenteService.ObtenerDocentes();
            cmbDocentes.ItemsSource = dt.DefaultView;
            // DisplayMemberPath y SelectedValuePath están definidos en el XAML
            cmbDocentes.SelectedIndex = -1;
        }


        // --- Lógica del Botón "Alta Docente" (Arriba a la derecha) ---
        private void BtnAltaDocente_Click(object sender, RoutedEventArgs e)
        {
            AltaDocenteView w = new AltaDocenteView();
            if (w.ShowDialog() == true)
            {
                MessageBox.Show("Docente agregado con éxito.", "Alta", MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargamos el combo para que aparezca el nuevo docente
                CargarDocentes();
            }
        }


        // ============================================================
        // LÓGICA DE GESTIÓN DE CURSOS (Migrada de la ventana anterior)
        // ============================================================

        private void cmbDocentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Limpiamos la grilla y deshabilitamos botones al cambiar de docente
            dgvCursos.ItemsSource = null;
            btnFinalizar.IsEnabled = false;
            btnCancelar.IsEnabled = false;

            if (cmbDocentes.SelectedValue != null)
            {
                int idDocente = (int)cmbDocentes.SelectedValue;
                DataTable dtCursos = _cursoService.ObtenerCursosPorDocente(idDocente);
                dgvCursos.ItemsSource = dtCursos.DefaultView;
            }
        }

        private void dgvCursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFinalizar.IsEnabled = false;
            btnCancelar.IsEnabled = false;

            if (dgvCursos.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)dgvCursos.SelectedItem;
                string estado = fila["Estado"].ToString().ToLower();

                // REGLA 1: Si está "en curso", se puede Finalizar.
                if (estado == "en curso" || estado == "en_curso")
                {
                    btnFinalizar.IsEnabled = true;
                }
                // REGLA 2: Si está "programado", se puede Cancelar.
                else if (estado == "programado")
                {
                    btnCancelar.IsEnabled = true;
                }
            }
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCursos.SelectedItem == null) return;

            DataRowView fila = (DataRowView)dgvCursos.SelectedItem;
            int idCurso = Convert.ToInt32(fila["ID_Curso"]);
            string nombreCurso = fila["Curso"].ToString();

            MessageBoxResult result = MessageBox.Show(
                "¿Desea finalizar el curso '" + nombreCurso + "'?",
                "Confirmar Finalización",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _cursoService.ActualizarEstadoCurso(idCurso, "finalizado");
                    MessageBox.Show("Curso finalizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Simulamos un cambio de selección para recargar la grilla
                    cmbDocentes_SelectionChanged(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCursos.SelectedItem == null) return;

            DataRowView fila = (DataRowView)dgvCursos.SelectedItem;
            int idCurso = Convert.ToInt32(fila["ID_Curso"]);
            string nombreCurso = fila["Curso"].ToString();

            MessageBoxResult result = MessageBox.Show(
                "ADVERTENCIA: ¿Desea CANCELAR el curso '" + nombreCurso + "'?\nEsto también cancelará todas las inscripciones asociadas.",
                "Confirmar Cancelación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _cursoService.ActualizarEstadoCurso(idCurso, "cancelado");
                    MessageBox.Show("Curso y sus inscripciones han sido cancelados.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Simulamos un cambio de selección para recargar la grilla
                    cmbDocentes_SelectionChanged(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}