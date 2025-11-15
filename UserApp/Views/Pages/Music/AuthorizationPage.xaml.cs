using Entity.Models;
using Infastructure.Services;
using Infastructure.Services.Music;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UserApp.Views.Windows;

namespace UserApp.Views.Pages.Music
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();

            RegistrationPanel.Visibility = Visibility.Visible;
            RegistrationPanel2.Visibility = Visibility.Collapsed;
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

            DataStore.Instance.User = await UserService.AuthenticateAsync(user.Username, user.Password);
            DataStore.MainViewModel.Message = "Вы зарегестрировались, войдите в свой аккаунт";

            RegistrationPanel.Visibility = Visibility.Visible;
            RegistrationPanel2.Visibility = Visibility.Collapsed;
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
            user.FavoriteMusics = await FavoriteMusicService.GetUserFavoritesAsync(user.Id);
            if (user != null)
            {
                if (user != null && user.QuestForAuthId != null)
                {
                    user.QuestForAuth = await QuestForAuthService.GetQuestByIdAsync((int)user.QuestForAuthId);
                }

                    DataStore.Instance.User = user;
                DataStore.MainViewModel.Message = "Вы вошли";

                DataStore.AdminMode = user.Username == "admin";

                DataStore.MainViewModel.AvatarPath = user.AvatarPath;

                DataStore.NavigationService.GoBack();
                DataStore.NavigationService.Navigate(DataStore.Instance.MainMusicPage);
                return;
            }

            DataStore.MainViewModel.Message = "Неверный логин или пароль";
            DataStore.MainViewModel.OpenShapka();
        }

        private async void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var user = await UserService.GetUserByUsernameAsync(Username.Text);

            if (user != null && user.QuestForAuthId != null)
            {
                user.QuestForAuth = await QuestForAuthService.GetQuestByIdAsync((int)user.QuestForAuthId);
                var windows = new CheckQwest(user);
                bool? result = windows.ShowDialog();
                if (result != null)
                {
                    DataStore.Instance.User = user;
                    DataStore.MainViewModel.Message = "Вы вошли";
                    DataStore.NavigationService.GoBack();
                    DataStore.AdminMode = user.Username == "admin";

                    DataStore.NavigationService.Navigate(DataStore.Instance.MainBookPage);

                    return;
                }
            }
            else
            {
                MessageBox.Show("Вопрос не найден для этого пользователя");
            }

        }
    }
}
