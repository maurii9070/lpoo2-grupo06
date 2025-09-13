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

namespace Vistas.Usuarios
{
    /// <summary>
    /// Interaction logic for GestionUsuariosUC.xaml
    /// </summary>
    public partial class GestionUsuariosUC : UserControl
    {
        public GestionUsuariosUC()
        {
            InitializeComponent();
        }
        private void btnAltaUsuarios_Click(object sender, RoutedEventArgs e)
        {
            AltaUsuariosView alta = new AltaUsuariosView();
            alta.Show();
        }
    }
}
