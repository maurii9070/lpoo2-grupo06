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
using ClasesBase.Utils;
using ClasesBase.Entidades;

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

            List<string> estados = new List<string>
            {
                EstadoCurso.Programado,
                EstadoCurso.EnCurso,
                EstadoCurso.Finalizado,
                EstadoCurso.Cancelado
            };
            cboEstado.ItemsSource = estados;
            cboEstado.SelectedIndex = 0; // por defecto
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                dpFechaIni.SelectedDate == null ||
                dpFechaFin.SelectedDate == null)
            {
                MessageBox.Show("Complete los campos obligatorios.", "Faltan datos",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpFechaFin.SelectedDate <= dpFechaIni.SelectedDate)
            {
                MessageBox.Show("La fecha de fin debe ser posterior a la de inicio.", "Validación",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Crear objeto
            CursoNuevo = new Curso
            {
                Cur_ID          = 1,
                Cur_Nombre      = txtNombre.Text.Trim(),
                Cur_Descripcion = txtDesc.Text.Trim(),
                Cur_Cupo        = int.Parse(txtCupo.Text),
                Cur_FechaInicio = dpFechaIni.SelectedDate.Value,
                Cur_FechaFin    = dpFechaFin.SelectedDate.Value,
                Est_ID          = 1,
                Doc_ID          = 1
            };

            DialogResult = true; 
        }
    }
}
