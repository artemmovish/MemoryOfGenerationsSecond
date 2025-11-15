using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models;
using Entity.Models.MusicEntity;
using Infastructure.Services;
using Infastructure.Services.Music;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.ViewModels.Base;
using UserApp.ViewModels.MusicVM;
using UserApp.Views.Pages.Book;
using UserApp.Views.Windows;

namespace UserApp.ViewModels.BookVM
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        User user = DataStore.Instance.User;

        [ObservableProperty]
        QuestForAuth questForAuth = new QuestForAuth()
        {
            Title = "Введите вопрос",
            Answer = "Введите ответ"
        };

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
        async void SaveQwest()
        {
            await QuestForAuthService.AddQuestAsync(QuestForAuth);

            var q = await QuestForAuthService.GetQuestByTitleAsync(QuestForAuth.Title);

            User.QuestForAuthId = q.Id;

            await UserService.UpdateUserAsync(user);
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

        // Добавьте эти свойства в класс ProfileViewModel
        [ObservableProperty]
        private string newPlayListName = "";

        [ObservableProperty]
        private UserPlayList selectedUserPlayList;

        [ObservableProperty]
        private List<UserPlayList> userPlayLists = new();

        // Добавьте в конструктор или метод инициализации
        partial void OnUserChanged(User value)
        {
            if (value != null)
            {
                LoadUserPlayLists();
            }
        }

        private async void LoadUserPlayLists()
        {
            UserPlayLists = await UserPlayListService.GetUserPlayListsByUserIdAsync(User.Id);
            OnPropertyChanged(nameof(UserPlayLists));
        }

        [RelayCommand]
        async Task CreatePlayList()
        {
            var page = DataStore.Instance.UserPlayListPage;

            page.DataContext = new UserPlayListViewModel();

            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void OpenPlayList(UserPlayList playList)
        {
            var page = DataStore.Instance.UserPlayListPage;

            page.DataContext = new UserPlayListViewModel(playList);

            DataStore.NavigationService.Navigate(page);
        }

        public async Task LoadUserPlayListAsync(int userPlayListId)
        {
            
        }

        [RelayCommand]
        async Task PlayPlayList(UserPlayList playList_)
        {
            var playList = await UserPlayListService.GetUserPlayListByIdAsync(playList_.Id);

            if (playList?.UserPlayListMusics?.Any() == true)
            {
                // Очищаем весь текущий список
                AudioService.TrackList.Clear();

                // Добавляем все треки из плейлиста
                foreach (var userPlayListMusic in playList.UserPlayListMusics)
                {
                    DataStore.AudioService.AddTrack(userPlayListMusic.Music.MusicPath);
                }

                // Запускаем воспроизведение
                DataStore.AudioService.Play();
                DataStore.MainViewModel.Message = $"Воспроизводится плейлист: {playList.Name}";
            }
            else
            {
                DataStore.MainViewModel.Message = "Плейлист пуст";
            }
        }
    }
}
