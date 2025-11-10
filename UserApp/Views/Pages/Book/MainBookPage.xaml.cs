using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UserApp.ViewModels.Base;
using UserApp.ViewModels.BookVM;

namespace UserApp.Views.Pages.Book
{
    /// <summary>
    /// Логика взаимодействия для MainBookPage.xaml
    /// </summary>
    public partial class MainBookPage : Page
    {
        public MainBookPage()
        {
            InitializeComponent(); 
            DataContext = new MainBookViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddAuthorBtn.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            AddBookBtn.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            DataContext = new MainBookViewModel();
        }

        private void OpenBooks_Click(object sender, RoutedEventArgs e)
        {
            BookPanel.Visibility = Visibility.Visible;
            AudioBookPanel.Visibility = Visibility.Visible;
            AuthorPanel.Visibility = Visibility.Collapsed;
            GenrePanel.Visibility = Visibility.Collapsed;
        }

        private void OpenAuthors_Click(object sender, RoutedEventArgs e)
        {
            BookPanel.Visibility = Visibility.Collapsed;
            AudioBookPanel.Visibility = Visibility.Collapsed;
            AuthorPanel.Visibility = Visibility.Visible;
            GenrePanel.Visibility = Visibility.Collapsed;
        }

        private void OpenGenres_Click(object sender, RoutedEventArgs e)
        {
            BookPanel.Visibility = Visibility.Collapsed;
            AudioBookPanel.Visibility = Visibility.Collapsed;
            AuthorPanel.Visibility = Visibility.Collapsed;
            GenrePanel.Visibility = Visibility.Visible;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BookPanel.Visibility = Visibility.Visible;
            AudioBookPanel.Visibility = Visibility.Visible;
            AuthorPanel.Visibility = Visibility.Visible;
            GenrePanel.Visibility = Visibility.Visible;

            var text = SearchTextBox.Text;

            var vm = new MainBookViewModel();

            vm.SearchText = text;

            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

// BookPanel AudioBookPanel
