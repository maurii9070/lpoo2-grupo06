using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls; // Necesario para SelectionChangedEventArgs
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClasesBase.Utils;
using ClasesBase.Entidades;
using ClasesBase.Services;

namespace Vistas.Cursos
{
    public partial class AltaCursoView : Window
    {
        private CursoService cursoService = new CursoService();
        private int _idProgramado;
        public Curso CursoNuevo { get; private set; }

        public AltaCursoView()
        {
            InitializeComponent();
            txtNombre.Focus();
            CargarCombos();

            // Bloquea las fechas pasadas en Fecha Inicio
            dpFechaIni.DisplayDateStart = DateTime.Today;
        }

        private void CargarCombos()
        {
            // --- Estados de curso ---
            // Busca el ID de "programado"
            List<Estado> estados = EstadoService.ObtenerEstadosCurso();
            Estado estadoProgramado = estados.FirstOrDefault(e => e.Est_Nombre.Equals("programado", StringComparison.InvariantCultureIgnoreCase));

            if (estadoProgramado != null)
            {
                _idProgramado = estadoProgramado.Est_ID;
            }
            else
            {
                // Estado crítico: si no existe "programado", no se puede guardar
                _idProgramado = -1; // ID Inválido
                MessageBox.Show("Error de configuración: No se encontró el estado 'programado' en la base de datos.", "Error Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
                btnGuardar.IsEnabled = false;
            }

            // --- Docentes ---
            DataTable dtDoc = new DocenteService().ObtenerDocentes();
            cboDocente.ItemsSource = dtDoc.DefaultView;
            cboDocente.DisplayMemberPath = "NombreCompleto";
            cboDocente.SelectedValuePath = "Doc_ID";
        }


        // --- Se ejecuta cuando cambia la Fecha de Inicio ---
        private void dpFechaIni_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificamos que se haya seleccionado una fecha
            if (dpFechaIni.SelectedDate.HasValue)
            {
                // Le decimos al DatePicker de Fin que su primera fecha
                // seleccionable es la que se acaba de elegir en Fecha Inicio.
                dpFechaFin.DisplayDateStart = dpFechaIni.SelectedDate.Value;

                // Opcional: Si el usuario ya tenía una fecha de fin
                // y ahora es inválida, la limpiamos.
                if (dpFechaFin.SelectedDate.HasValue && dpFechaFin.SelectedDate < dpFechaFin.DisplayDateStart)
                {
                    dpFechaFin.SelectedDate = null;
                }
            }
        }


        private void txtCupo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // si NO es dígito, cancelamos el carácter
            e.Handled = !char.IsDigit(e.Text, 0);
        }


        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones rápidas
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDesc.Text) ||
                dpFechaIni.SelectedDate == null ||
                dpFechaFin.SelectedDate == null ||
                cboDocente.SelectedValue == null)
            {
                MessageBox.Show("Complete todos los campos correctamente.",
                                "Alta Curso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int cupo;
            if (!int.TryParse(txtCupo.Text, out cupo) || cupo <= 0)
            {
                MessageBox.Show("Ingrese un cupo válido mayor a 0.");
                return;
            }

            // --- VALIDACIONES DE FECHA ---
            // (Se mantienen por seguridad, por si el usuario escribe a mano)

            if (dpFechaIni.SelectedDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("La fecha de inicio no puede ser anterior al día de hoy.",
                                "Validación de Fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpFechaFin.SelectedDate < dpFechaIni.SelectedDate)
            {
                MessageBox.Show("La fecha de fin debe ser posterior o igual a la de inicio.",
                                "Validación de Fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cursoService.CursoExistente(txtNombre.Text.Trim()))
            {
                MessageBox.Show("Ya existe un curso con ese nombre", "Validación de Nombre", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Arma el objeto
            CursoNuevo = new Curso
            {
                Cur_Nombre = txtNombre.Text.Trim(),
                Cur_Descripcion = txtDesc.Text.Trim(),
                Cur_Cupo = cupo,
                Cur_FechaInicio = dpFechaIni.SelectedDate.Value,
                Cur_FechaFin = dpFechaFin.SelectedDate.Value,
                Est_ID = _idProgramado, // Asignación directa
                Doc_ID = (int)cboDocente.SelectedValue
            };

            // Persiste
            cursoService.GuardarCurso(CursoNuevo);

            MessageBox.Show("Curso dado de alta:\n" + CursoNuevo.Cur_Nombre,
                            "Alta exitosa",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            DialogResult = true;   // cierra la ventana modal
        }
    }
}