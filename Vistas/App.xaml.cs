using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ruta base de ejecución (bin\Debug normalmente)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Subo carpetas hasta llegar al proyecto ClasesBase\App_Data
            // Ajustá el número de ".." según tu estructura
            string dataDir = Path.Combine(baseDir, @"..\..\..\ClasesBase");

            // Normalizo la ruta y la asigno como DataDirectory
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(dataDir));
        }
    }
}
