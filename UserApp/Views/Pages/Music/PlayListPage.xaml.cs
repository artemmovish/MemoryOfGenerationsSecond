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

namespace UserApp.Views.Pages.Music
{
    /// <summary>
    /// Логика взаимодействия для PlayListPage.xaml
    /// </summary>
    public partial class PlayListPage : Page
    {
        public PlayListPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AdminPanel.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            UserPanel.Visibility = DataStore.AdminMode ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
