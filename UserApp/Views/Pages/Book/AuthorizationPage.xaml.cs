using Entity.Models;
using Infastructure.Services;
using Infastructure.Services.Music;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
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

namespace UserApp.Views.Pages.Book
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private async void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = new User()
            {
                Username = Username2.Text,
                Password = Password2.Text,
                MyThoughts = new ObservableCollection<MyThought>(),
                FavoriteBooks = new ObservableCollection<FavoriteBook>(),
                AvatarPath = "/Resources/Book/profileIcon.png"
            };

            await UserService.AddUserAsync(user);
           
            DataStore.Instance.User = await UserService.AuthenticateAsync(Username2.Text, Password2.Text);

            DataStore.MainViewModel.Message = "Вы зарегестрировались";

            DataStore.MainViewModel.AvatarPath = user.AvatarPath;

            DataStore.NavigationService.GoBack();
            await Task.Delay(100);
            DataStore.NavigationService.Navigate(DataStore.Instance.MainBookPage);
        }

        private void HyperlinkToReg_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPanel.Visibility = Visibility.Collapsed;
            RegistrationPanel2.Visibility = Visibility.Visible;
        }

        private void HyperlinkToAuth_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPanel.Visibility = Visibility.Visible;
            RegistrationPanel2.Visibility = Visibility.Collapsed;
        }

        private async void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = await UserService.AuthenticateAsync(Username.Text, Password.Text);
            

            if (user != null)
            {
                user.MyThoughts = await MyThoughtService.GetUserThoughtsAsync(user.Id);
                user.FavoriteBooks = await FavoriteBookService.GetUserFavoritesAsync(user.Id);
                user.FavoriteMusics = await FavoriteMusicService.GetUserFavoritesAsync(user.Id);
                DataStore.Instance.User = user;

                DataStore.MainViewModel.AvatarPath = user.AvatarPath;

                DataStore.MainViewModel.Message = "Вы вошли";
                DataStore.NavigationService.GoBack();
                await Task.Delay(500);

                DataStore.AdminMode = user.Username == "admin";

                DataStore.NavigationService.Navigate(DataStore.Instance.MainBookPage);

                return;
            }

            DataStore.MainViewModel.Message = "Неверный логин или пароль";
            DataStore.MainViewModel.OpenShapka();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
