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
        }

        private void BtnAltaCurso_Click(object sender, RoutedEventArgs e)
        {
            {
                AltaCursoView w = new AltaCursoView();
                if (w.ShowDialog() == true)
                {
                    MessageBox.Show("Curso dado de alta:\n" +
                                  w.CursoNuevo.Cur_Nombre + " (" + w.CursoNuevo.Cur_ID + ")",
                                  "Alta exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
