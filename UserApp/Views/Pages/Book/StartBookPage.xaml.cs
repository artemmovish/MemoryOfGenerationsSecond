using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UserApp.ViewModels.Base;

namespace UserApp.Views.Pages.Book
{
    /// <summary>
    /// Логика взаимодействия для StartBookPage.xaml
    /// </summary>
    public partial class StartBookPage : Page
    {
        public StartBookPage()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataStore.MainViewModel.SetShapka(2);
            NavigationService.Navigate(DataStore.Instance.StartMusicPage);
        }
    }
}
