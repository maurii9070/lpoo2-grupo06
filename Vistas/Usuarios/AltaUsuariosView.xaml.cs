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
using ClasesBase.Repositories;
namespace Vistas.Usuarios
{
    /// <summary>
    /// Interaction logic for AltaUsuariosView.xaml
    /// </summary>
    public partial class AltaUsuariosView : Window
    {
        RolRepository rolRep = new RolRepository();
        public AltaUsuariosView()
        {
            InitializeComponent();
            CargarRoles();
        }
        private void CargarRoles()
        {
            // Limpiar cualquier item existente primero
            cmbRol.Items.Clear();

            // Cargar roles directamente desde el Repository
            cmbRol.ItemsSource = RolRepository.ObtenerTodos();
            cmbRol.DisplayMemberPath = "Rol_Descripcion";
            cmbRol.SelectedValuePath = "Rol_ID";
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                // Obtener el rol seleccionado
                int rolId = (int)cmbRol.SelectedValue;
                string rolDescripcion = ObtenerDescripcionRol(rolId);

                Usuario usuario = new Usuario
                {
                    Usu_NombreUsuario = txtUsuario.Text,
                    Usu_Contrasenia = txtPassword.Password,
                    Usu_ApellidoNombre = txtApellidoNombre.Text,
                    Rol_ID = rolId
                };

                // Mostrar confirmación con los datos (pero NO guardar)
                string mensajeConfirmacion = "DATOS DEL USUARIO A CREAR\n\n" +
                                             "Usuario: " + usuario.Usu_NombreUsuario + "\n" +
                                             "Contraseña: " + new string('*', usuario.Usu_Contrasenia.Length) + "\n" +
                                             "Nombre: " + usuario.Usu_ApellidoNombre + "\n" +
                                             "Rol: " + rolDescripcion + "\n" +
                                             "Email: " + (string.IsNullOrEmpty(txtEmail.Text) ? "No especificado" : txtEmail.Text) + "\n\n" +
                                             "¿Confirmar la creación de este usuario?";

                MessageBoxResult result = MessageBox.Show(
                    mensajeConfirmacion,
                    "CONFIRMAR ALTA DE USUARIO",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // SOLO MOSTRAR MENSAJE DE ÉXITO, NO GUARDAR
                    string mensajeExito = "USUARIO CREADO EXITOSAMENTE\n\n" +
                                         "Se han capturado todos los datos correctamente.\n" +
                                         "En un sistema real, los datos se guardarian en la base de datos.\n\n" +
                                         "Usuario: " + usuario.Usu_NombreUsuario + "\n" +
                                         "Rol: " + rolDescripcion;

                    MessageBox.Show(
                        mensajeExito,
                        "SIMULACION EXITOSA",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    LimpiarCampos();
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("El campo Usuario es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUsuario.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("El campo Contraseña es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellidoNombre.Text))
            {
                MessageBox.Show("El campo Apellido y Nombre es obligatorio", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtApellidoNombre.Focus();
                return false;
            }

            if (cmbRol.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un Rol", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbRol.Focus();
                return false;
            }

            return true;
        }

        private string ObtenerDescripcionRol(int rolId)
        {
            // Usar directamente el Repository para obtener la descripción del rol
            Rol rol = rolRep.Obtener(rolId);
            return rol != null ? rol.Rol_Descripcion : "Desconocido";
        }

        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtPassword.Clear();
            txtApellidoNombre.Clear();
            txtEmail.Clear();
            cmbRol.SelectedIndex = -1;
            txtUsuario.Focus();
        }
    }
}
