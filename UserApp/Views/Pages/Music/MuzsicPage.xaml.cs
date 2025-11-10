using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserApp.ViewModels.Base;
using UserApp.ViewModels.BookVM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UserApp.Views.Pages.Music
{
    /// <summary>
    /// Логика взаимодействия для MuzsicPage.xaml
    /// </summary>
    public partial class MusicPage : Page
    {
        public MusicPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Управление видимостью панелей
            AdminPanel.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            UserPanel.Visibility = DataStore.AdminMode ? Visibility.Collapsed : Visibility.Visible;
        }



    }
}
