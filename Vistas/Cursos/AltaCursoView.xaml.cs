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
using System.Windows.Shapes;
using ClasesBase.Utils;
using ClasesBase.Entidades;
using ClasesBase.Services;

namespace Vistas.Cursos
{
    /// <summary>
    /// Interaction logic for AltaCursoView.xaml
    /// </summary>
    public partial class AltaCursoView : Window
    {

        public Curso CursoNuevo { get; private set; }

        public AltaCursoView()
        {
            InitializeComponent();
            txtNombre.Focus();
            CargarCombos();
        }

        private void CargarCombos()
        {
            // --- Estados de curso ---
            DataTable dtEst = new EstadoService().ObtenerEstadosDeCurso();
            cboEstado.ItemsSource = dtEst.DefaultView;
            cboEstado.DisplayMemberPath = "Est_Nombre";
            cboEstado.SelectedValuePath = "Est_ID";

            // --- Docentes ---
            DataTable dtDoc = new DocenteService().ObtenerDocentes();
            cboDocente.ItemsSource = dtDoc.DefaultView;
            cboDocente.DisplayMemberPath = "Display";
            cboDocente.SelectedValuePath = "Doc_ID";
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
                string.IsNullOrWhiteSpace(txtDesc.Text)  ||
                dpFechaIni.SelectedDate == null          ||
                dpFechaFin.SelectedDate == null          ||
                cboEstado.SelectedValue == null          ||
                cboDocente.SelectedValue == null)
            {
                MessageBox.Show("Complete todos los campos correctamente.",
                                "Alta Curso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int cupo;   // declaración previa (C# 4)
            if (!int.TryParse(txtCupo.Text, out cupo) || cupo <= 0)
            {
                MessageBox.Show("Ingrese un cupo válido mayor a 0.");
                return;
            }

            // FechaFin debe ser >= FechaInicio
            if (dpFechaFin.SelectedDate < dpFechaIni.SelectedDate)
            {
                MessageBox.Show("La fecha de fin debe ser posterior o igual a la de inicio.",
                                "Alta Curso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Arma el objeto
            CursoNuevo = new Curso
            {
                Cur_Nombre      = txtNombre.Text.Trim(),
                Cur_Descripcion = txtDesc.Text.Trim(),
                Cur_Cupo        = cupo,
                Cur_FechaInicio = dpFechaIni.SelectedDate.Value,
                Cur_FechaFin    = dpFechaFin.SelectedDate.Value,
                Est_ID          = (int)cboEstado.SelectedValue,
                Doc_ID          = (int)cboDocente.SelectedValue
            };

            // Persiste
            new CursoService().GuardarCurso(CursoNuevo);

            MessageBox.Show("Curso dado de alta:\n" + CursoNuevo.Cur_Nombre,
                            "Alta exitosa",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            DialogResult = true;   // cierra la ventana modal
        }
    }
}
