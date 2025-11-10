using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserApp.Views.Pages.Book
{
    /// <summary>
    /// Логика взаимодействия для UpdateProfilePage.xaml
    /// </summary>
    public partial class UpdateProfilePage : Page
    {
        public UpdateProfilePage()
        {
            InitializeComponent();
        }

        public UpdateProfilePage(string hexColor)
        {
            InitializeComponent();
            SetBackgroundColor(hexColor);
        }
        private void SetBackgroundColor(string hexColor)
        {
            try
            {
                // Конвертируем HEX в SolidColorBrush
                var color = (Color)ColorConverter.ConvertFromString(hexColor);
                panel.Background = new SolidColorBrush(color);

                // Или для конкретного элемента, например, MainGrid:
                // MainGrid.Background = new SolidColorBrush(color);
            }
            catch
            {
                // Обработка ошибки (например, если цвет в неверном формате)
                panel.Background = Brushes.White; // Цвет по умолчанию
            }
        }
    }
}
