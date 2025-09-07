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

namespace Vistas.Docentes
{
    /// <summary>
    /// Interaction logic for AltaDocenteView.xaml
    /// </summary>
    public partial class AltaDocenteView : Window
    {
        public Docente DocenteNuevo { get; private set; }

        public AltaDocenteView()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            {
                // Validación mínima
                if (string.IsNullOrWhiteSpace(txtDni.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("Complete DNI, Apellido y Nombre.", "Faltan datos",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Crear objeto
                DocenteNuevo = new Docente
                {
                    Doc_DNI = txtDni.Text.Trim(),
                    Doc_Apellido = txtApellido.Text.Trim(),
                    Doc_Nombre = txtNombre.Text.Trim(),
                    Doc_Email = txtEmail.Text.Trim()
                };

                DialogResult = true;   // cierra la ventana y devuelve true
                
            }
        }
    }
}
