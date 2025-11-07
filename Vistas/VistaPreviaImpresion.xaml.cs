using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;   
using System.Printing;         
using ClasesBase.Entidades;    
using System.Windows.Markup;   
using System.Windows.Data;   

namespace Vistas
{
    public partial class VistaPreviaImpresion : Window
    {

        private IEnumerable<Usuario> _usuarios;


        public VistaPreviaImpresion(IEnumerable<Usuario> usuarios)
        {
            InitializeComponent();

            //Guarda la lista de usuarios
            _usuarios = usuarios;

            //Construye la VISTA PREVIA en pantalla (el FlowDocument)
            ConstruirVistaPreviaFlowDocument(usuarios);
        }

        private void ConstruirVistaPreviaFlowDocument(IEnumerable<Usuario> usuarios)
        {
            Table tablaUsuarios = new Table();
            tablaUsuarios.CellSpacing = 0; // Sin espaciado
            tablaUsuarios.BorderBrush = System.Windows.Media.Brushes.Gray;
            tablaUsuarios.BorderThickness = new Thickness(1);

            tablaUsuarios.Columns.Add(new TableColumn() { Width = new GridLength(250) });
            tablaUsuarios.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            tablaUsuarios.Columns.Add(new TableColumn() { Width = new GridLength(150) });

            tablaUsuarios.RowGroups.Add(new TableRowGroup());

            TableRow filaEncabezado = new TableRow();
            filaEncabezado.FontWeight = FontWeights.Bold;
            filaEncabezado.Background = System.Windows.Media.Brushes.Gainsboro;

            filaEncabezado.Cells.Add(CrearCeldaParaFlowDoc("Apellido y Nombre"));
            filaEncabezado.Cells.Add(CrearCeldaParaFlowDoc("Usuario"));
            filaEncabezado.Cells.Add(CrearCeldaParaFlowDoc("Rol"));
            tablaUsuarios.RowGroups[0].Rows.Add(filaEncabezado);

            // 5. Filas de Datos
            if (usuarios != null)
            {
                foreach (Usuario user in usuarios)
                {
                    TableRow filaDatos = new TableRow();
                    filaDatos.Cells.Add(CrearCeldaParaFlowDoc(user.Usu_ApellidoNombre));
                    filaDatos.Cells.Add(CrearCeldaParaFlowDoc(user.Usu_NombreUsuario));
                    filaDatos.Cells.Add(CrearCeldaParaFlowDoc(user.RolNombre));
                    tablaUsuarios.RowGroups[0].Rows.Add(filaDatos);
                }
            }

            flowDoc.Blocks.Add(tablaUsuarios);
        }

        private TableCell CrearCeldaParaFlowDoc(string texto)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = texto;
            textBlock.TextWrapping = TextWrapping.NoWrap; // No permite el salto de línea
            textBlock.TextTrimming = TextTrimming.CharacterEllipsis; // Pone "..." si es muy largo

            BlockUIContainer container = new BlockUIContainer(textBlock);

            TableCell celda = new TableCell(container);

            celda.BorderBrush = System.Windows.Media.Brushes.Black;
            celda.BorderThickness = new Thickness(0, 0, 1, 1);
            celda.Padding = new Thickness(5);
            return celda;
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true)
            {
                return; 
            }

            FixedDocument docParaImprimir = CrearDocumentoFijoParaImprimir();

            printDialog.PrintDocument(docParaImprimir.DocumentPaginator, "Listado de Usuarios");
        }

        private FixedDocument CrearDocumentoFijoParaImprimir()
        {
            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            fixedPage.Width = 794;
            fixedPage.Height = 1122;

            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(72); // Margen

            // Título
            TextBlock titulo = new TextBlock();
            titulo.Text = "Listado de Usuarios";
            titulo.FontSize = 24;
            titulo.FontWeight = FontWeights.Bold;
            titulo.Margin = new Thickness(0, 0, 0, 20);
            panel.Children.Add(titulo);

            DataGrid grid = new DataGrid();
            grid.ItemsSource = _usuarios; 
            grid.AutoGenerateColumns = false;
            grid.IsReadOnly = true;

            // Columnas
            grid.Columns.Add(new DataGridTextColumn {
                Header = "Apellido y Nombre",
                Binding = new Binding("Usu_ApellidoNombre"),
                Width = new DataGridLength(250)
            });
            grid.Columns.Add(new DataGridTextColumn {
                Header = "Nombre de Usuario",
                Binding = new Binding("Usu_NombreUsuario"),
                Width = new DataGridLength(150)
            });
            grid.Columns.Add(new DataGridTextColumn {
                Header = "Rol",
                Binding = new Binding("RolNombre"),
                Width = new DataGridLength(150)
            });

            panel.Children.Add(grid);

            // "Arma" el documento fijo [cite: 435-441]
            fixedPage.Children.Add(panel);
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            return fixedDoc;
        }
    }
}