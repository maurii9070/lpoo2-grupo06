using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;    // Para FlowDocument, Table, etc.
using System.Printing;          // Para PrintDialog
using ClasesBase.Entidades;       // Para tu clase Usuario
using System.Windows.Markup;    // Para IAddChild (impresión)
using System.Windows.Data;      // Para Binding (impresión)

namespace Vistas
{
    public partial class VistaPreviaImpresion : Window
    {
        // Variable para guardar la lista de usuarios
        private IEnumerable<Usuario> _usuarios;

        // El constructor recibe la lista y la guarda
        public VistaPreviaImpresion(IEnumerable<Usuario> usuarios)
        {
            InitializeComponent();

            // 1. Guarda la lista de usuarios
            _usuarios = usuarios;

            // 2. Construye la VISTA PREVIA en pantalla (el FlowDocument)
            ConstruirVistaPreviaFlowDocument(usuarios);
        }

        // ===================================================================
        // PARTE 1: CONSTRUIR LA VISTA PREVIA (FLOW DOCUMENT)
        // ===================================================================

        /// <summary>
        /// Método 1: Construye la tabla de VISTA PREVIA (un FlowDocument)
        /// como lo pide el TP5 y la teoría[cite: 147, 195].
        /// </summary>
        private void ConstruirVistaPreviaFlowDocument(IEnumerable<Usuario> usuarios)
        {
            // 1. Crear la Tabla (similar al ejemplo de la teoría [cite: 195])
            Table tablaUsuarios = new Table();
            tablaUsuarios.CellSpacing = 0; // Sin espaciado
            tablaUsuarios.BorderBrush = System.Windows.Media.Brushes.Gray;
            tablaUsuarios.BorderThickness = new Thickness(1);

            // 2. Definir 3 columnas
            tablaUsuarios.Columns.Add(new TableColumn() { Width = new GridLength(250) });
            tablaUsuarios.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            tablaUsuarios.Columns.Add(new TableColumn() { Width = new GridLength(150) });

            // 3. Crear el grupo de filas
            tablaUsuarios.RowGroups.Add(new TableRowGroup());

            // 4. Fila de Encabezado
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

            // 6. Añadir la tabla al FlowDocument
            // "flowDoc" es el Name que le dimos en el XAML
            flowDoc.Blocks.Add(tablaUsuarios);
        }

        /// <summary>
        /// Helper para crear celdas con bordes y padding para el FlowDocument
        /// </summary>
        private TableCell CrearCeldaParaFlowDoc(string texto)
        {
            // --- ESTE ES EL CAMBIO ---
            
            // 1. Creamos un TextBlock que NO corta el texto
            TextBlock textBlock = new TextBlock();
            textBlock.Text = texto;
            textBlock.TextWrapping = TextWrapping.NoWrap; // No permite el salto de línea
            textBlock.TextTrimming = TextTrimming.CharacterEllipsis; // Pone "..." si es muy largo

            // 2. Usamos un BlockUIContainer para meter el TextBlock en la celda
            // (Como dice la teoría, permite agregar UIElements) 
            BlockUIContainer container = new BlockUIContainer(textBlock);

            // 3. Creamos la celda y le pasamos el container
            TableCell celda = new TableCell(container);
            
            // --- FIN DEL CAMBIO ---

            // Esto queda igual
            celda.BorderBrush = System.Windows.Media.Brushes.Black;
            celda.BorderThickness = new Thickness(0, 0, 1, 1);
            celda.Padding = new Thickness(5);
            return celda;
        }


        // ===================================================================
        // PARTE 2: LÓGICA DE IMPRESIÓN (FIXED DOCUMENT)
        // ===================================================================

        /// <summary>
        /// Método 2: Se ejecuta al presionar "Imprimir"[cite: 8].
        /// </summary>
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true)
            {
                return; // El usuario canceló
            }

            // 1. Crea el DOCUMENTO FIJO (el bueno) para la impresora
            FixedDocument docParaImprimir = CrearDocumentoFijoParaImprimir();

            // 2. Manda a imprimir el documento fijo[cite: 521, 522].
            // La vista previa (flowDoc) NUNCA SE TOCA.
            // Por eso no desaparecen los datos y la impresión no se corta.
            printDialog.PrintDocument(docParaImprimir.DocumentPaginator, "Listado de Usuarios");
        }


        /// <summary>
        /// Método 3: Construye el DOCUMENTO FIJO (FixedDocument)
        /// que se enviará a la impresora/PDF (basado en la teoría [cite: 433-454]).
        /// </summary>
        private FixedDocument CrearDocumentoFijoParaImprimir()
        {
            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            // Tamaño de página A4
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

            // DataGrid (la mejor forma de imprimir una tabla)
            DataGrid grid = new DataGrid();
            grid.ItemsSource = _usuarios; // Usa la lista que guardamos
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