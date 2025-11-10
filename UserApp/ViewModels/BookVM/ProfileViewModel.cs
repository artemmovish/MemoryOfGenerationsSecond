using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models;
using Entity.Models.MusicEntity;
using Infastructure.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.ViewModels.Base;
using UserApp.ViewModels.MusicVM;
using UserApp.Views.Pages.Book;

namespace UserApp.ViewModels.BookVM
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        User user = DataStore.Instance.User;

        [ObservableProperty]
        string password = "";

        [ObservableProperty]
        string newPassword = "";

        [RelayCommand]
        void ChooseAnBook(Book book)
        {
            var page = DataStore.Instance.BookPage;

            page.DataContext = book == null ? new BookViewModel() : new BookViewModel(book);

            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void ChooseAMusic(FavoriteMusic Fmusic)
        {
            var music = Fmusic.Music;
            var page = DataStore.Instance.MusicPage;
            page.DataContext = music == null ? new MusicViewModel() : new MusicViewModel(music);
            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void ToUpdateProfile()
        {
            //var page = new UpdateProfilePage();

            //page.DataContext = this;

            //DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        async Task UpdateUsername()
        {
            await UserService.UpdateUserAsync(User);
            DataStore.MainViewModel.Message = "Имя изменено";
        }

        [RelayCommand]
        public async Task UpdateImage()
        {
            try
            {
                // Открываем диалог выбора файла
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Выберите изображение аватара",
                    Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
                    Multiselect = false
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем выбранный файл
                    string selectedFilePath = openFileDialog.FileName;

                    // Обновляем путь к аватару
                    User.AvatarPath = selectedFilePath;

                    // Сохраняем изменения
                    await UserService.UpdateUserAsync(User);

                    DataStore.MainViewModel.Message = "Аватар успешно изменен";
                    DataStore.MainViewModel.AvatarPath = User.AvatarPath;
                    // Обновляем интерфейс (если нужно)
                    OnPropertyChanged(nameof(User));
                }
            }
            catch (Exception ex)
            {
                DataStore.MainViewModel.Message = $"Ошибка при выборе аватара: {ex.Message}";
            }
        }

        [RelayCommand]
        async Task UpdatePassword()
        {
            if (UserService.VerifyPassword(Password, User.Password))
            {
                User.Password = NewPassword;
                await UserService.UpdatePasswordUserAsync(User);
                User = await UserService.GetUserByIdAsync(User.Id);
                DataStore.MainViewModel.Message = "Пароль изменен";
                return;
            }
            DataStore.MainViewModel.Message = "Неверный пароль";
        }
    }
}
