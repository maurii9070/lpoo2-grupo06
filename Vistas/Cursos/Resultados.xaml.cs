using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClasesBase.Services;
using ClasesBase.Entidades;
using ClasesBase.Utils;

namespace Vistas.Cursos
{
    public partial class Resultados : Window
    {
        public Resultados()
        {
            InitializeComponent();
            cargarAlumnos();
            txtFinalizados.Text = "-";
            txtEnCurso.Text = "-";
        }

        private void cargarAlumnos()
        {
            DataTable dtAlumnos = new AlumnoService().ObtenerAlumnos();
            cmbAlumnos.ItemsSource = dtAlumnos.DefaultView;
            cmbAlumnos.DisplayMemberPath = "Display";
            cmbAlumnos.SelectedValuePath = "Alu_ID";

        }

        // Obtiene los cursos del del alumno seleccionado, limpia la vista anterior
        private void cmbAlumnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LimpiarVista();

            int aluId = Convert.ToInt32(cmbAlumnos.SelectedValue);
            DataTable dtResultados = new InscripcionService().GetResultadosPorAlumno(aluId);

            if (dtResultados == null || dtResultados.Rows.Count == 0)
            {
                MessageBox.Show("Este alumno no tiene ningún curso inscrito.", "Sin cursos", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MostrarTotales(dtResultados);
            MostrarCursosFinalizados(dtResultados);
        }

        private void LimpiarVista()
        {
            cursosFinalizados.ItemsSource = null;
            txtFinalizados.Text = "-";
            txtEnCurso.Text = "-";
        }

        // Muestra en pantalla la cantidad de cursos finalizados y en curso
        private void MostrarTotales(DataTable dt)
        {
            int finalizados = ContarPorEstado(dt, EstadoCurso.Finalizado);
            int enCurso = ContarPorEstado(dt, EstadoCurso.EnCurso);

            txtFinalizados.Text = finalizados.ToString();
            txtEnCurso.Text = enCurso.ToString();
            if (finalizados == 0 && enCurso == 0)
            {
                MessageBox.Show("Los cursos de este alumno no esta finalizados ni en curso.", "Cursos", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        // Cuenta la cantidad de cursos que coinciden con el estado
        private int ContarPorEstado(DataTable dt, string estado)
        {
            return dt.AsEnumerable()
                     .Count(row => row["Estado"].ToString().Trim().ToLower() == estado);
        }

        // Filtra los cursos finalizados del dataTable
        // Si no hay cursos finalizados, limpia la grilla
        private void MostrarCursosFinalizados(DataTable dt)
        {
            DataView viewFinalizados = new DataView(dt)
            {
                RowFilter = "Estado = 'finalizado'"
            };
            cursosFinalizados.ItemsSource = viewFinalizados.Count > 0 ? viewFinalizados : null;
        }


        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
