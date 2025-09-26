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

namespace Vistas.Docentes
{
    /// <summary>
    /// Interaction logic for DocentesView.xaml
    /// </summary>
    public partial class DocentesView : UserControl
    {
        public DocentesView()
        {
            InitializeComponent();
        }

        private void BtnAltaDocente_Click(object sender, RoutedEventArgs e)
        {
            AltaDocenteView w = new AltaDocenteView();
            if (w.ShowDialog() == true)        // <-- modal
            {
                // Aquí guardamos en memoria (falta repo/service)
                MessageBox.Show(
                                "Docente dado de alta:\n" +
                                w.DocenteNuevo.Doc_Apellido + ", " +
                                w.DocenteNuevo.Doc_Nombre,
                                "Alta exitosa",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
    }
}
