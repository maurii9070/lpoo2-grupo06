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
using System.ComponentModel;          // ICollectionView
using System.Collections.ObjectModel; // ObservableCollection
using ClasesBase;                     // TrabajarUsuarios
using ClasesBase.Entidades;           // Usuario
using ClasesBase.Services;

namespace Vistas.Usuarios
{
    /// <summary>
    /// Interaction logic for GestionUsuariosUC.xaml
    /// </summary>
    public partial class GestionUsuariosUC : UserControl
    {
        private ICollectionView view;
        private UsuarioService _service = new UsuarioService();

        public GestionUsuariosUC()
        {
            InitializeComponent();
            CargarGrilla();   // <-- mismo estilo que CursosView
        }

        private void CargarGrilla()
        {
            ObservableCollection<Usuario> lista = _service.ObtenerUsuarios();

            view = CollectionViewSource.GetDefaultView(lista);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("Usu_NombreUsuario",
                                                          ListSortDirection.Ascending));

            DtgUsuarios.ItemsSource = view;

            bool hay = lista.Count > 0;
            DtgUsuarios.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
            txtSinDatos.Visibility = hay ? Visibility.Collapsed : Visibility.Visible;
            TxtFiltro.Visibility = hay ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            view.Filter = new Predicate<object>(UsuarioFilter);
        }

        private bool UsuarioFilter(object item)
        {
            Usuario u = item as Usuario;
            if (u == null) return false;
            string txt = TxtFiltro.Text.ToLower();
            return u.Usu_NombreUsuario.ToLower().Contains(txt);
        }

        private void Btn_ABM_Click(object sender, RoutedEventArgs e)
        {
            ABMUsuarios abm = new ABMUsuarios();
            abm.ShowDialog();     // Mostramos el ABM
            CargarGrilla();       // Siempre recargamos la grilla al volver
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            // 'view' es tu ICollectionView.
            // Usamos .Cast<Usuario>() para obtener la lista que está
            // actualmente en la grilla (ya sea filtrada o completa).
            IEnumerable<Usuario> listaParaImprimir = view.Cast<Usuario>();

            // Creamos y mostramos la nueva ventana
            VistaPreviaImpresion vistaPrevia = new VistaPreviaImpresion(listaParaImprimir);
            vistaPrevia.ShowDialog();
        }
    }
}