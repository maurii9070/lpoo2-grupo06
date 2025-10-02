using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Data;

namespace Vistas.Resources
{
    public class ConversorDeEstado : IValueConverter
    {
        public object Convert(object estadoValor, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = estadoValor as string;
            switch (estado)
            {
                case "Programado":
                    return Brushes.Green;
                case "En curso":
                    return Brushes.Blue;
                case "Finalizado":
                    return Brushes.DarkCyan;
                case "Cancelado":
                    return Brushes.Red;
                default:
                    return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
