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
using Vistas.Docentes;
using Vistas.Inscripciones;
using Vistas.Cursos;

using Vistas.Usuarios;
using Vistas.Alumnos;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuDocentes_Click(object sender, RoutedEventArgs e)
        {
            AreaContenido.Content = new DocentesView();
        }

        private void MenuCursos_Click(object sender, RoutedEventArgs e)
        {
            AreaContenido.Content = new CursosView();
        }

        private void MenuAlumnos_Click(object sender, RoutedEventArgs e)
        {
            AreaContenido.Content = new GestionAlumnoView();
        }
        private void MenuUsuarios_Click(object sender, RoutedEventArgs e )
        {
            AreaContenido.Content = new GestionUsuariosUC();
        }

        private void MenuEstadoCursos_Click(object sender, RoutedEventArgs e)
        {
            AreaContenido.Content = new EstadosCursosView();
        }

        private void MenuInscripciones_Click(object sender, RoutedEventArgs e)
        {
            AreaContenido.Content = new GestionDeInscripcionesUC();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AreaContenido.Content = new Vistas.Inscripciones.AcreditacionUC();
        }
    }
}
