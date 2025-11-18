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
using ClasesBase.Services;
using ClasesBase.Entidades;

namespace Vistas.Login
{
    /// <summary>
    /// Lógica de interacción para LoginUC.xaml
    /// </summary>
    public partial class LoginUC : UserControl
    {
        // Asumo que tienes una clase AuthService en tus servicios
        // Si no, asegúrate de usar UsuarioRepository o UsuarioService según corresponda
        private AuthService authService = new AuthService();

        public LoginUC()
        {
            InitializeComponent();
            txtUsuario.Focus();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasenia = txtPassword.Password;

            // Autenticamos y obtenemos el OBJETO COMPLETO del usuario
            Usuario usr = authService.Autenticar(usuario, contrasenia);

            if (usr != null)
            {
                // --- CAMBIO CLAVE AQUÍ ---
                // En lugar de 'new MainView()', pasamos el objeto 'usr' al constructor.
                // Esto enviará el Nombre y Rol a la pantalla principal.
                MainView main = new MainView(usr);
                main.Show();

                // Cerramos la ventana de Login
                Window ventana = Window.GetWindow(this);
                if (ventana != null)
                {
                    ventana.Close();
                }
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}