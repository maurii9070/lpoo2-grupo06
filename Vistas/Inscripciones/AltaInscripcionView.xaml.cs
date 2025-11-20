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
        private readonly AlumnoService _alumnoService = new AlumnoService();
        public AltaInscripcionView()
        {
            InitializeComponent();
            CargarCursos();
        }
        private void CargarCursos()
        {
            // --- Estados de curso ---
            List<Curso> cursos = new CursoService().getCursosProgramados();
            if (cursos == null || cursos.Count == 0)
            {
                cboCursos.ItemsSource = null;
                cboCursos.Items.Clear();
                cboCursos.Items.Add("No hay cursos disponibles");
                cboCursos.SelectedIndex = 0;
                cboCursos.IsEnabled = false; 
                // para que no intenten seleccionarlo

                btnRegistrarInscripcion.IsEnabled = false; 
                return;
            }

            cboCursos.ItemsSource = cursos; 
            cboCursos.DisplayMemberPath = "Cur_Nombre";
            cboCursos.SelectedValuePath = "Cur_ID";
        }
        
        private void btnRegistrarInscripcion_Click(object sender, RoutedEventArgs e)
        {
 
            Estado estadoConfirmado = EstadoService.ObtenerEstadosInscripcion().FirstOrDefault(estado => estado.Est_Nombre == "inscripto");
            
            //Validar alumno
            Alumno alumno = ValidarAlumno();
            if (alumno == null) return;
            //Validar curso
            Curso curso = ValidarCurso();
            if (curso == null) return;

            bool ya_inscripto = new InscripcionService().existeInscripcion(curso.Cur_ID, alumno.Alu_ID);

            if (ya_inscripto)
            {
                MostrarError("El alumno ya esta inscripto en este curso.");
                return;
            }

            inscripcion = new Inscripcion
            {
                Ins_Fecha = DateTime.Now,
                Cur_ID= curso.Cur_ID,
                Alu_ID= alumno.Alu_ID,
                Est_ID= estadoConfirmado.Est_ID
            };
            //Se actualiza el cupo cuando se da de alta una inscripcion
            curso.Cur_Cupo--;
            CursoService service = new CursoService();
            service.ActualizarCurso(curso);
            // Persiste
            
            new InscripcionService().crearInscripcion(inscripcion);

            MessageBox.Show("Inscripcion dada de alta correctamente:\n" + 
                             "Fecha de Inscripcion: "+ inscripcion.Ins_Fecha+ "\n"+
                             "ID del Curso: "+ inscripcion.Cur_ID+"\n"+
                             "ID del Alumno: " + inscripcion.Alu_ID + "\n",
                              "Alta exitosa",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            DialogResult = true;   // cierra la ventana modal

        }
        private Alumno ValidarAlumno()
        {
            string dni = txtAlumno.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni))
            {
                MostrarError("Debe ingresar un DNI.");
                return null;
            }

            Alumno alumno = _alumnoService.ObtenerAlumnoPorDNI(dni);

            if (alumno == null)
            {
                MostrarAdvertencia("No existe un alumno con ese DNI.");
                return null;
            }

            return alumno;
        }
        private Curso ValidarCurso()
        {
            if (cboCursos.SelectedItem == null)
            {
                MostrarError("Debe seleccionar un curso.");
                return null;
            }
            Curso curso = (Curso) cboCursos.SelectedItem;
            if (curso.Cur_Cupo <= 0)
            {
                MostrarError("El curso no tiene cupos disponibles.");
                return null;
            }

            return curso;
        }
        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MostrarAdvertencia(string mensaje)
        {
            MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
