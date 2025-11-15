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

            var text = DataStore.AppDbContext.HelpTexts.FirstOrDefault(a => a.Id == 1).Text;

            UpdateToolTips(text, text);
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

        private void UpdateToolTips(string leftTip, string rightTip)
        {
            Left_btn.ToolTip = leftTip;
            Right_btn.ToolTip = rightTip;
        }
    }
}
