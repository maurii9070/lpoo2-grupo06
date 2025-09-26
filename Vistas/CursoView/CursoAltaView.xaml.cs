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
using System.Windows.Navigation;
using System.Windows.Shapes;

using ClasesBase.Entidades;
using ClasesBase.Services;
using ClasesBase.Repositories;

namespace Vistas.CursoView
{
    /// <summary>
    /// Interaction logic for CursoAltaView.xaml
    /// </summary>
    public partial class CursoAltaView : UserControl
    {
        private CursoService service;
        public CursoAltaView()
        {
            InitializeComponent();
            service = new CursoService();
            CargarCombos();
        }
        private void CargarCombos()
        {
            // Cargar solo estados de curso (Esty_ID = 1)
            cmbEstado.ItemsSource = EstadoService.ObtenerEstadosCurso();
            cmbEstado.SelectedValuePath = "Est_ID";
            cmbEstado.DisplayMemberPath = "Est_Nombre";

            // Cargar docentes
            //cmbdocente.itemssource = docenteservice.obtenertodos();
            //cmbdocente.selectedvaluepath = "doc_id";
            //cmbdocente.displaymemberpath = "doc_apellido";
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                int cupo = int.Parse(txtCupo.Text);
                int estadoId = (int)cmbEstado.SelectedValue;
                int docenteId = (int)cmbDocente.SelectedValue;

                Curso curso = new Curso
                {
                    Cur_Nombre = txtNombre.Text,
                    Cur_Descripcion = txtDescripcion.Text,
                    Cur_Cupo = cupo,
                    Cur_FechaInicio = dpFechaInicio.SelectedDate.Value,
                    Cur_FechaFin = dpFechaFin.SelectedDate.Value,
                    Est_ID = estadoId, // ID del estado seleccionado
                    Doc_ID = docenteId // ID del docente seleccionado
                };

                string mensajeConfirmacion = "¿Está seguro de guardar el siguiente curso?\n\n" +
                                             "Nombre: " + curso.Cur_Nombre + "\n" +
                                             "Descripción: " + curso.Cur_Descripcion + "\n" +
                                             "Cupo: " + curso.Cur_Cupo + "\n" +
                                             "Fecha Inicio: " + curso.Cur_FechaInicio.ToString("dd/MM/yyyy") + "\n" +
                                             "Fecha Fin: " + curso.Cur_FechaFin.ToString("dd/MM/yyyy") + "\n" +
                                             "Estado: " + ((Estado)cmbEstado.SelectedItem).Est_Nombre + "\n" +
                                             "Docente: " + ((Docente)cmbDocente.SelectedItem).Doc_Apellido;

                MessageBoxResult result = MessageBox.Show(
                    mensajeConfirmacion,
                    "CONFIRMAR ALTA",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    service.GuardarCurso(curso);

                    string mensajeExito = "✅ CURSO GUARDADO EXITOSAMENTE\n\n" +
                                         "ID del curso: " + curso.Cur_ID + "\n" +
                                         "Nombre: " + curso.Cur_Nombre;

                    MessageBox.Show(
                        mensajeExito,
                        "OPERACIÓN EXITOSA",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    LimpiarCampos();
                }
            }
        }

        private bool ValidarCampos()
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo NOMBRE es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("El campo DESCRIPCIÓN es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtDescripcion.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCupo.Text))
            {
                MessageBox.Show("El campo CUPO es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCupo.Focus();
                return false;
            }

            if (dpFechaInicio.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una FECHA DE INICIO", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                dpFechaInicio.Focus();
                return false;
            }

            if (dpFechaFin.SelectedDate == null)
            {
                MessageBox.Show("Debe seleccionar una FECHA DE FIN", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                dpFechaFin.Focus();
                return false;
            }
            if (cmbEstado.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un ESTADO de la lista", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbEstado.Focus();
                return false;
            }
            // Validar formatos numéricos
            int cupo;
            if (!int.TryParse(txtCupo.Text, out cupo) || cupo <= 0)
            {
                MessageBox.Show("El CUPO debe ser un número válido mayor a 0", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCupo.SelectAll();
                txtCupo.Focus();
                return false;
            }


            // Validar fechas
            if (dpFechaInicio.SelectedDate.Value > dpFechaFin.SelectedDate.Value)
            {
                MessageBox.Show("La FECHA DE INICIO no puede ser posterior a la FECHA DE FIN", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                dpFechaInicio.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtCupo.Clear();
            dpFechaInicio.SelectedDate = null;
            dpFechaFin.SelectedDate = null;
            txtNombre.Focus();
        }
    }
}
