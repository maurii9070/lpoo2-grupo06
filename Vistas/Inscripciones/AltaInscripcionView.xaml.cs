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

namespace Vistas.Inscripciones
{
    /// <summary>
    /// Lógica de interacción para AltaInscripcionView.xaml
    /// </summary>
    public partial class AltaInscripcionView : Window
    {
        public Inscripcion inscripcion { get; private set; }
        public AltaInscripcionView()
        {
            InitializeComponent();
            CargarCursos();
            CargarEstados();
        }
        private void CargarCursos()
        {
            // --- Estados de curso ---
            List<Curso> cursos = new CursoService().getCursosProgramados();// devuelve List<Estado>
            cboCursos.ItemsSource = cursos; // 👈 asignar directamente la lista
            cboCursos.DisplayMemberPath = "Cur_Nombre";
            cboCursos.SelectedValuePath = "Cur_ID";
        }
        private void CargarEstados()
        {
            List<Estado> estados = EstadoService.ObtenerEstadosInscripcion();
            cboEstados.ItemsSource = estados;
            cboEstados.DisplayMemberPath = "Est_Nombre";
            cboEstados.SelectedValuePath = "Est_ID";
        }
        private void btnRegistrarInscripcion_Click(object sender, RoutedEventArgs e)
        {
            string dni = txtAlumno.Text;
            int curso = (int)cboCursos.SelectedValue;
            Alumno alumno = new AlumnoService().ObtenerAlumnoPorDNI(dni);
            if (alumno==null){
                MessageBox.Show("No existe un alumno con ese DNI.",
                        "Alumno no encontrado",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                return;
            }
            bool ya_inscripto = new InscripcionService().existeInscripcion(curso, alumno.Alu_ID);

            if (ya_inscripto)
            {
                MessageBox.Show("El alumno ya está inscripto en este curso.",
                                "Registro duplicado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            inscripcion = new Inscripcion
            {
                Ins_Fecha = dtInscripcion.SelectedDate.Value,
                Cur_ID= curso,
                Alu_ID= alumno.Alu_ID,
                Est_ID= (int)cboEstados.SelectedValue
            };

            // Persiste
            
            new InscripcionService().crearInscripcion(inscripcion);

            MessageBox.Show("Inscripcion dado de alta:\n" + inscripcion.Alu_ID,
                            "Alta exitosa",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            DialogResult = true;   // cierra la ventana modal

        }

        
    }
}
