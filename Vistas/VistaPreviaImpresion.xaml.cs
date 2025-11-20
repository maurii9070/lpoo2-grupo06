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

            // Crear Grid para la tabla
            Grid tablaGrid = new Grid();
            tablaGrid.Background = System.Windows.Media.Brushes.White;

            // Definir columnas
            tablaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            tablaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            tablaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            // Fila de encabezado
            tablaGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Encabezados
            Border headerBorder1 = CrearBordeConTexto("Apellido y Nombre", true);
            Grid.SetRow(headerBorder1, 0);
            Grid.SetColumn(headerBorder1, 0);
            tablaGrid.Children.Add(headerBorder1);

            Border headerBorder2 = CrearBordeConTexto("Nombre de Usuario", true);
            Grid.SetRow(headerBorder2, 0);
            Grid.SetColumn(headerBorder2, 1);
            tablaGrid.Children.Add(headerBorder2);

            Border headerBorder3 = CrearBordeConTexto("Rol", true);
            Grid.SetRow(headerBorder3, 0);
            Grid.SetColumn(headerBorder3, 2);
            tablaGrid.Children.Add(headerBorder3);

            // Agregar filas de datos
            int filaActual = 1;
            if (_usuarios != null)
            {
                foreach (Usuario user in _usuarios)
                {
                    tablaGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    Border cellBorder1 = CrearBordeConTexto(user.Usu_ApellidoNombre ?? "", false);
                    Grid.SetRow(cellBorder1, filaActual);
                    Grid.SetColumn(cellBorder1, 0);
                    tablaGrid.Children.Add(cellBorder1);

                    Border cellBorder2 = CrearBordeConTexto(user.Usu_NombreUsuario ?? "", false);
                    Grid.SetRow(cellBorder2, filaActual);
                    Grid.SetColumn(cellBorder2, 1);
                    tablaGrid.Children.Add(cellBorder2);

                    Border cellBorder3 = CrearBordeConTexto(user.RolNombre ?? "", false);
                    Grid.SetRow(cellBorder3, filaActual);
                    Grid.SetColumn(cellBorder3, 2);
                    tablaGrid.Children.Add(cellBorder3);

                    filaActual++;
                }
            }

            panel.Children.Add(tablaGrid);

            // "Arma" el documento fijo
            fixedPage.Children.Add(panel);
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            return fixedDoc;
        }

        private Border CrearBordeConTexto(string texto, bool esEncabezado)
        {
            Border border = new Border();
            border.BorderBrush = System.Windows.Media.Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Padding = new Thickness(5);

            if (esEncabezado)
            {
                border.Background = System.Windows.Media.Brushes.Gainsboro;
            }
            else
            {
                border.Background = System.Windows.Media.Brushes.White;
            }

            TextBlock textBlock = new TextBlock();
            textBlock.Text = texto;
            textBlock.FontSize = 12;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
            textBlock.VerticalAlignment = VerticalAlignment.Center;

            if (esEncabezado)
            {
                textBlock.FontWeight = FontWeights.Bold;
            }

            border.Child = textBlock;

            return border;
        }
    }
}