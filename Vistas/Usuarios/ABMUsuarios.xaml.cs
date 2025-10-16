using System;
using System.Collections.ObjectModel;
using System.Windows;
using ClasesBase.Entidades;
using ClasesBase.Services;

namespace Vistas.Usuarios
{
    public partial class ABMUsuarios : Window
    {
        private ObservableCollection<Usuario> _usuarios;
        private int _posicionActual = 0;
        private UsuarioService _service = new UsuarioService();

        public ABMUsuarios()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _usuarios = _service.ObtenerUsuarios();

            if (_usuarios != null && _usuarios.Count > 0)
            {
                MostrarUsuarioActual();
            }
            else
            {
                LimpiarCampos(); 
            }
        }

        private void MostrarUsuarioActual()
        {
            if (_usuarios == null || _usuarios.Count == 0) return;

            Usuario u = _usuarios[_posicionActual];

            // Separar Apellido y Nombre desde Usu_ApellidoNombre
            string[] partes = u.Usu_ApellidoNombre.Split(new char[] { ' ' }, 2);
            string apellido = partes.Length > 0 ? partes[0] : "";
            string nombre = partes.Length > 1 ? partes[1] : "";

            Apellido_txt.Text = apellido;
            nombre_txt.Text = nombre;
            user_txt.Text = u.Usu_NombreUsuario;
            password_txt.Text = new string('*', u.Usu_Contrasenia.Length); // asteriscos
        }

        // --- Navegación ---
        private void Btn_Primero_Click(object sender, RoutedEventArgs e)
        {
            _posicionActual = 0;
            MostrarUsuarioActual();
        }

        private void Btn_Anterior_Click(object sender, RoutedEventArgs e)
        {
            if (_posicionActual > 0)
                _posicionActual--;
            MostrarUsuarioActual();
        }

        private void Btn_Siguiente_Click(object sender, RoutedEventArgs e)
        {
            if (_posicionActual < _usuarios.Count - 1)
                _posicionActual++;
            MostrarUsuarioActual();
        }

        private void btn_Ultimo_Click(object sender, RoutedEventArgs e)
        {
            _posicionActual = _usuarios.Count - 1;
            MostrarUsuarioActual();
        }

        private void Btn_Nuevo_Click(object sender, RoutedEventArgs e)
        {
            AltaUsuariosView alta = new AltaUsuariosView();
            if (alta.ShowDialog() == true) // Esto depende de que AltaUsuariosView use DialogResult = true
            {
                // Solo actualizo la lista interna del ABM para que siga funcionando
                _usuarios = _service.ObtenerUsuarios();
                if (_usuarios.Count > 0)
                {
                    _posicionActual = _usuarios.Count - 1;
                    MostrarUsuarioActual();
                }
                else
                {
                    LimpiarCampos();
                }
            }
        }

        private void Btn_Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (_usuarios == null || _usuarios.Count == 0) return;

            Usuario seleccionado = _usuarios[_posicionActual];
            AltaUsuariosView alta = new AltaUsuariosView(seleccionado);
            if (alta.ShowDialog() == true)
            {
                _usuarios = _service.ObtenerUsuarios();
                MostrarUsuarioActual();
            }
        }

        private void Btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (_usuarios == null || _usuarios.Count == 0) return;

            Usuario seleccionado = _usuarios[_posicionActual];
            MessageBoxResult result = MessageBox.Show(
                "¿Eliminar al usuario '" + seleccionado.Usu_NombreUsuario + "'?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _service.EliminarUsuario(seleccionado.Usu_ID);
                _usuarios.Remove(seleccionado);

                if (_usuarios.Count > 0)
                {
                    if (_posicionActual >= _usuarios.Count)
                        _posicionActual = _usuarios.Count - 1;
                    MostrarUsuarioActual();
                }
                else
                {
                    LimpiarCampos();
                }
            }
        }

        private void LimpiarCampos()
        {
            Apellido_txt.Text = "";
            nombre_txt.Text = "";
            user_txt.Text = "";
            password_txt.Text = "";
        }

        private void Btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cerramos sin importar si hubo cambios o no
        }
    }
}