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

namespace Vistas.Cursos
{
    /// <summary>
    /// Interaction logic for CursosView.xaml
    /// </summary>
    public partial class CursosView : UserControl
    {
        public CursosView()
        {
            InitializeComponent();
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            DataTable dt = new CursoService().ObtenerCursos();

            bool hayDatos = dt.Rows.Count > 0;

            dgCursos.Visibility = hayDatos ? Visibility.Visible : Visibility.Collapsed;
            txtSinDatos.Visibility = hayDatos ? Visibility.Collapsed : Visibility.Visible;

            if (hayDatos)
                dgCursos.ItemsSource = dt.DefaultView;
            else
                dgCursos.ItemsSource = null;   // limpiar por si acaso
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


    }
}
