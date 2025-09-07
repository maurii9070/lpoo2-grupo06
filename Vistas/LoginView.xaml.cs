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
using ClasesBase.Entidades;
using ClasesBase.Services;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private AuthService _auth = new AuthService();

        public LoginView()
        {
            InitializeComponent();
            txtUsuario.Focus();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasenia = txtPassword.Password;

            Usuario usr = _auth.Autenticar(usuario, contrasenia);
            if (usr != null)
            {
                // Pasar usuario a la ventana principal (si la necesitás)
                MainView main = new MainView();
                main.Show();
                this.Close();          // cerrar login
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.",
                                "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Salir de la aplicacion
            Application.Current.Shutdown();
        }
    }
}
