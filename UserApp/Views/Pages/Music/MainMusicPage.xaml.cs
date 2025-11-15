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
using UserApp.ViewModels.MusicVM;
using UserApp.Views.Pages.Book;

namespace UserApp.Views.Pages.Music
{
    /// <summary>
    /// Логика взаимодействия для MainMusicPage.xaml
    /// </summary>
    public partial class MainMusicPage : Page
    {
        public MainMusicPage()
        {
            InitializeComponent();
            DataContext = new MainMusicViewModel();
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddMusicBtn.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            AddActorBtn.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            AddPlayBtn.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;

            var model = new MainMusicViewModel();

            await Task.Delay(1000);

            DataContext = model;
        }

        private void OpenBooks_Click(object sender, RoutedEventArgs e)
        {
            AudioBookPanel.Visibility = Visibility.Visible;
            AuthorPanel.Visibility = Visibility.Collapsed;
            PlayListPanel.Visibility = Visibility.Collapsed;
            GenrePanel.Visibility = Visibility.Collapsed;
        }

        private void OpenAuthors_Click(object sender, RoutedEventArgs e)
        {
            AudioBookPanel.Visibility = Visibility.Collapsed;
            AuthorPanel.Visibility = Visibility.Visible;
            PlayListPanel.Visibility = Visibility.Collapsed;
            GenrePanel.Visibility = Visibility.Collapsed;
        }

        private void OpenGenres_Click(object sender, RoutedEventArgs e)
        {
            AudioBookPanel.Visibility = Visibility.Collapsed;
            AuthorPanel.Visibility = Visibility.Collapsed;
            PlayListPanel.Visibility = Visibility.Collapsed;
            GenrePanel.Visibility = Visibility.Visible;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AudioBookPanel.Visibility = Visibility.Visible;
            AuthorPanel.Visibility = Visibility.Visible;
            GenrePanel.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
