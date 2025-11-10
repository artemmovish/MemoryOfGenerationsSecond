using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UserApp.ViewModels.Base;

namespace UserApp.Views.Pages.Music
{
    /// <summary>
    /// Логика взаимодействия для StartMusicPage.xaml
    /// </summary>
    public partial class StartMusicPage : Page
    {
        public StartMusicPage()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataStore.MainViewModel.SetShapka(1);
            NavigationService.GoBack();
        }
    }
}
