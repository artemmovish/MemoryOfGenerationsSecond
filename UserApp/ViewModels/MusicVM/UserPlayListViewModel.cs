using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models.MusicEntity;
using Infastructure.Services.Music;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using UserApp.ViewModels.Base;

namespace UserApp.ViewModels.MusicVM
{
    public partial class UserPlayListViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Music> _musics = new ObservableCollection<Music>();

        [ObservableProperty]
        private ObservableCollection<Music> _musicsInPlayList = new ObservableCollection<Music>();

        [ObservableProperty]
        private UserPlayList _currentUserPlayList = new UserPlayList();

        [ObservableProperty]
        private string _imagePath;

        public UserPlayListViewModel()
        {
            InitializeAsync();
        }

        public UserPlayListViewModel(UserPlayList userPlayList)
        {
            ImagePath = userPlayList.PathPhoto;
            LoadUserPlayListAsync(userPlayList.Id);
        }

        private async void InitializeAsync()
        {
            await LoadAllMusicsAsync();
        }

        private async Task LoadAllMusicsAsync()
        {
            var allMusics = await UserPlayListService.Context.Musics.ToListAsync();
            Musics = new ObservableCollection<Music>(allMusics);
            OnPropertyChanged(nameof(Musics));
        }

        public async Task LoadUserPlayListAsync(int userPlayListId)
        {
            var userPlayList = await UserPlayListService.GetUserPlayListByIdAsync(userPlayListId);
            if (userPlayList != null)
            {
                CurrentUserPlayList = userPlayList;
                var musics = await UserPlayListService.GetMusicsFromUserPlayListAsync(userPlayListId);
                MusicsInPlayList = new ObservableCollection<Music>(musics);

                var allMusics = await UserPlayListService.Context.Musics.ToListAsync();
                Musics = new ObservableCollection<Music>(allMusics);

                OnPropertyChanged(nameof(MusicsInPlayList));
                OnPropertyChanged(nameof(CurrentUserPlayList));
            }
        }

        [RelayCommand]
        private void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                Title = "Выберите изображение для плейлиста"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                CurrentUserPlayList.PathPhoto = ImagePath;
            }
        }

        [RelayCommand]
        public async Task SaveUserPlayListAsync()
        {
            if (CurrentUserPlayList.Id > 0)
            {
                await UserPlayListService.UpdateUserPlayListAsync(CurrentUserPlayList);
                MessageBox.Show("Плейлист успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Для нового плейлиста нужно указать UserId (здесь используется текущий пользователь)
            // Вам нужно будет получить ID текущего пользователя из контекста приложения
            int currentUserId = GetCurrentUserId(); // Этот метод нужно реализовать
            CurrentUserPlayList.UserId = currentUserId;

            await UserPlayListService.AddUserPlayListAsync(CurrentUserPlayList);
            MessageBox.Show("Плейлист успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        public async Task DeleteUserPlayListAsync()
        {
            if (CurrentUserPlayList != null && CurrentUserPlayList.Id > 0)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот плейлист?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await UserPlayListService.DeleteUserPlayListAsync(CurrentUserPlayList.Id);
                    CurrentUserPlayList = new UserPlayList();
                    MusicsInPlayList.Clear();
                    OnPropertyChanged(nameof(CurrentUserPlayList));
                    MessageBox.Show("Плейлист успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        [RelayCommand]
        public async Task AddMusicToUserPlayListAsync(Music music)
        {
            if (CurrentUserPlayList.Id > 0)
            {
                await UserPlayListService.AddMusicToUserPlayListAsync(CurrentUserPlayList.Id, music.Id);
                await LoadUserPlayListAsync(CurrentUserPlayList.Id);
            }
        }

        [RelayCommand]
        public async Task RemoveMusicFromUserPlayListAsync(Music music)
        {
            if (CurrentUserPlayList.Id > 0)
            {
                await UserPlayListService.RemoveMusicFromUserPlayListAsync(CurrentUserPlayList.Id, music.Id);
                await LoadUserPlayListAsync(CurrentUserPlayList.Id);
            }
        }

        private int GetCurrentUserId()
        {
            return DataStore.Instance.User.Id;
        }
    }
}