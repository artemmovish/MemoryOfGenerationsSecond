using Entity.Models.MusicEntity;
using Infastructure.Context;
using Infastructure.Services;
using Infastructure.Services.Music;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserApp.ViewModels;
using UserApp.ViewModels.Base;
using UserApp.ViewModels.BookVM;
using UserApp.Views.Pages.Book;
using UserApp.Views.Pages.Music;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public int NumberShapka = 1;
    public MainWindow()
    {
        InitializeComponent();

        var context = new AppDbContext();

        // Инициализация сервисов
        AuthorService.Context = context;
        BookService.Context = context;
        FavoriteBookService.Context = context;
        MyThoughtService.Context = context;
        UserService.Context = context;

        MusicService.Context = context;
        ActorService.Context = context;
        PlayListService.Context = context;
        FavoriteMusicService.Context = context;

        MainFrame.Navigate(DataStore.Instance.StartBookPage);
        DataStore.NavigationService = MainFrame.NavigationService;

        DataContext = DataStore.MainViewModel;

        DataStore.MainViewModel.SetShapka = ChangeShapka;

        DataStore.MainViewModel.OpenShapka = OpenShapka;
        DataStore.MainViewModel.CloseShapka = CloseShapka;

        Shapka.Visibility = Visibility.Visible;
        Shapka2.Visibility = Visibility.Collapsed;
    }

    private void Icon_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.NavigationService.GoBack();
    }

    public void ChangeShapka(int number)
    {
        NumberShapka = number;

        switch (number)
        {
            case 1:
                Shapka.Visibility = Visibility.Visible;
                Shapka2.Visibility = Visibility.Collapsed;
                break;
            case 2:
                Shapka.Visibility = Visibility.Collapsed;
                Shapka2.Visibility = Visibility.Visible;
                break;
            default:
                break;
        }
    }

    public void OpenShapka()
    {
        switch (NumberShapka)
        {
            case 1:
                Shapka.Visibility = Visibility.Visible;
                Shapka2.Visibility = Visibility.Collapsed;
                break;
            case 2:
                Shapka.Visibility = Visibility.Collapsed;
                Shapka2.Visibility = Visibility.Visible;
                break;
            default:
                break;
        }
    }
    public void CloseShapka()
    {
        Shapka.Visibility = Visibility.Collapsed;
        Shapka2.Visibility = Visibility.Collapsed;
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F1)
        {
            switch (NumberShapka)
            {
                case 1:
                    Shapka.Visibility = Shapka.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
                    break;
                case 2:
                    Shapka2.Visibility = Shapka2.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        if (e.Key == Key.Escape)
        {
            var result = MessageBox.Show(
                "Закрыть приложение?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown(); // Закрыть приложение
            }
        }
    }
    private void About_Click(object sender, RoutedEventArgs e)
    {
        switch (NumberShapka)
        {
            case 1:
                DataStore.NavigationService.Navigate(new AboutPage());
                break;
            case 2:
                DataStore.NavigationService.Navigate(new AboutMusicPage());
                break;
            default:
                break;
        }
    }
    private void Help_Click(object sender, RoutedEventArgs e)
    {
        switch (NumberShapka)
        {
            case 1:
                DataStore.NavigationService.Navigate(new HelpPage());
                break;
            case 2:
                DataStore.NavigationService.Navigate(new HelpMusicPage());
                break;
            default:
                break;
        }
        
    }
    private void Profile_Click(object sender, RoutedEventArgs e)
    {
        if (DataStore.Instance.User == null)
        {
            MessageBox.Show("Вы не вошли в аккаунт");
            return;
        }

        var page = new ProfilePage();

        if (NumberShapka == 2)
        {
            page = new ProfilePage("#292929");
        }

        
        page.DataContext = new ProfileViewModel();
        DataStore.NavigationService.Navigate(page);
    }
}

