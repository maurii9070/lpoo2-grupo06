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
using System.Data;
using ClasesBase.Services;
namespace Vistas.Inscripciones
{
    /// <summary>
    /// Lógica de interacción para GestionDeInscripcionesUC.xaml
    /// </summary>
    public partial class GestionDeInscripcionesUC : UserControl
    {
        public GestionDeInscripcionesUC()
        {
            InitializeComponent();
            CargarGrilla();
        }
        private void CargarGrilla()
        {
            DataTable dt = new InscripcionService().obtenerInscripciones();

            bool hayDatos = dt.Rows.Count > 0;

            dgInscripciones.Visibility = hayDatos ? Visibility.Visible : Visibility.Collapsed;
            txtSinDatos.Visibility = hayDatos ? Visibility.Collapsed : Visibility.Visible;

            if (hayDatos)
                dgInscripciones.ItemsSource = dt.DefaultView;
            else
                dgInscripciones.ItemsSource = null;   // limpiar por si acaso
        }

        private void btnInscripcion_Click(object sender, RoutedEventArgs e)
        {
            AltaInscripcionView inscripcionForm = new AltaInscripcionView();
            if (inscripcionForm.ShowDialog() == true)
            {
                CargarGrilla();
            }
        }
    }
}
