using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace UserApp.Convertes
{
    public class HalfConverter : IValueConverter
    {
        // Преобразует высоту/ширину в CornerRadius (делит на 2)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double size)
                return new CornerRadius(size / 2);  // Делает скругление = половине высоты/ширины

            return new CornerRadius(0);  // Если что-то пошло не так
        }

        // Обратное преобразование (не используется, но обязательно для интерфейса)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
