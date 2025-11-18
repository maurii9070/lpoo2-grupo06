using System;
using System.Windows;
using Vistas.Docentes;
using Vistas.Inscripciones;
using Vistas.Cursos;
using Vistas.Usuarios;
using Vistas.Alumnos;
using ClasesBase.Entidades; // Necesario para usar la clase Usuario

namespace Vistas
{
    public partial class MainView : Window
    {
        // Constructor por defecto (Solo para pruebas)
        public MainView()
        {
            InitializeComponent();
        }

        // Constructor PRINCIPAL: Recibe el objeto Usuario desde el Login
        public MainView(Usuario usuario)
        {
            InitializeComponent();
            ConfigurarBienvenida(usuario);
        }

        private void ConfigurarBienvenida(Usuario u)
        {
            if (u != null)
            {
                // --- HEADER (Arriba a la derecha) ---
                txtUsuarioHeader.Text = u.Usu_ApellidoNombre;
                txtRolHeader.Text = "Rol: " + u.RolNombre; // Mostramos el rol aquí también

                // --- MENSAJE CENTRAL (Pantalla de inicio) ---

                // Título grande: ¡Bienvenido, [Nombre]!
                txtBienvenida.Text = "¡Bienvenido, " + u.Usu_ApellidoNombre + "!";

                // Subtítulo: Texto explicativo sin repetir el nombre innecesariamente
                txtBienvenidaDetalle.Text = "Has ingresado al Sistema de Gestión Académica con el perfil de " + u.RolNombre + ".\n" +
                                            "Utiliza el menú lateral izquierdo para acceder a las diferentes secciones.";
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // --- Navegación del Menú ---

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

        private void MenuUsuarios_Click(object sender, RoutedEventArgs e)
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