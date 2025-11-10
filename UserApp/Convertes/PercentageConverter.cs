using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UserApp.Convertes
{
    public class PercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 ||
                !(values[0] is double) ||
                !(values[1] is double) ||
                !(values[2] is double))
                return 0.0;

            double value = (double)values[0];
            double max = (double)values[1];
            double width = (double)values[2];

            if (max == 0) return 0.0;

            return (value / max) * width;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
