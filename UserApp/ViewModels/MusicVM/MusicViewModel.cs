using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models;
using Entity.Models.MusicEntity;
using Infastructure.Services;
using Infastructure.Services.Music;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserApp.ViewModels.Base;
using UserApp.Views.Pages.Music;

namespace UserApp.ViewModels.MusicVM
{
    public partial class MusicViewModel : ObservableObject, IDisposable
    {
        [ObservableProperty]
        Music selectedMusic = new();

        [ObservableProperty]
        string imagePath;

        [ObservableProperty]
        string musicPath;

        public ObservableCollection<Actor> Actors { get; set; } = new ObservableCollection<Actor>();

        [ObservableProperty]
        User currentUser;

        [ObservableProperty]
        bool isAdmin;

        public MusicViewModel()
        {
            if (DataStore.IsInDesignMode)
            {
                Actor actor = new()
                {
                    Name = "The Weeknd"
                };

                SelectedMusic = new Music
                {
                    Id = 1,
                    Name = "Blinding Lights",
                    ActorId = 1,
                    Actor = actor,
                    MusicPath = "https://example.com/music/weeknd_blinding_lights.mp3",
                    ImagePath = "https://upload.wikimedia.org/wikipedia/en/0/05/The_Weeknd_-_After_Hours.png",
                    TextPath = "/lyrics/weeknd_blinding_lights.txt"
                };

                CurrentUser = new User
                {
                    Id = 1,
                    AvatarPath = "/path/to/avatar.jpg",
                    Username = "TestUser"
                };

                // Тестовые исполнители
                var actor1 = new Actor
                {
                    Id = 1,
                    Name = "The Weeknd",
                    ImagePath = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/The_Weeknd_-_Blinding_Lights_%28single%29.png/800px-The_Weeknd_-_Blinding_Lights_%28single%29.png",
                    Description = "Канадский певец и автор песен"
                };

                var actor2 = new Actor
                {
                    Id = 2,
                    Name = "Dua Lipa",
                    ImagePath = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4e/Dua_Lipa_2018_MTV_EMA.jpg/800px-Dua_Lipa_2018_MTV_EMA.jpg",
                    Description = "Британская певица и автор песен"
                };

                Actors.Add(actor1);
                Actors.Add(actor2);

                return;
            }
            IsAdmin = DataStore.AdminMode;
            LoadData();
        }

        public MusicViewModel(Music music)
        {
            IsAdmin = DataStore.AdminMode;
            SelectedMusic = music;
            ImagePath = music.ImagePath;
            MusicPath = music.MusicPath;
            LoadData();
        }

        [RelayCommand]
        void PlayMusic()
        {
            if (!string.IsNullOrEmpty(SelectedMusic.MusicPath))
            {
                DataStore.AudioService.AddTrack(SelectedMusic.MusicPath);
                DataStore.AudioService.Play();
            }
        }

        public async Task LoadData()
        {
            Actors = new ObservableCollection<Actor>(await ActorService.GetAllActorsAsync());
            LoadUser();
        }

        private void LoadUser()
        {
            CurrentUser = DataStore.Instance.User;
        }

        [RelayCommand]
        public async void DeleteMusic()
        {
            try
            {
                await MusicService.DeleteMusicAsync(SelectedMusic.Id);
                DataStore.MainViewModel.Message = "Трек удален";
            }
            catch (Exception)
            {
                MessageBox.Show("Трек не найден!");
            }
            DataStore.NavigationService.GoBack();
        }

        [RelayCommand]
        public void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите обложку трека",
                Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string extension = Path.GetExtension(selectedFilePath).ToLower();

                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp")
                {
                    SelectedMusic.ImagePath = selectedFilePath;
                    ImagePath = selectedFilePath;
                    DataStore.MainViewModel.Message = "Обложка загружена";
                }
                else
                {
                    MessageBox.Show("Выберите файл изображения (PNG, JPG, JPEG, BMP).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        [RelayCommand]
        public void AddMusicFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите аудиофайл",
                Filter = "Аудиофайлы (*.mp3;*.wav;*.ogg;*.flac)|*.mp3;*.wav;*.ogg;*.flac|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string extension = Path.GetExtension(selectedFilePath).ToLower();

                if (extension == ".mp3" || extension == ".wav" || extension == ".ogg" || extension == ".flac")
                {
                    SelectedMusic.MusicPath = selectedFilePath;
                    MusicPath = selectedFilePath;
                    DataStore.MainViewModel.Message = "Аудиофайл загружен";
                }
                else
                {
                    MessageBox.Show("Выберите файл аудио (MP3, WAV, OGG, FLAC).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        [RelayCommand]
        public void AddLyricsFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите PDF-файл с текстом",
                Filter = "PDF-файлы (*.pdf)|*.pdf|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedMusic.TextPath = openFileDialog.FileName;
                DataStore.MainViewModel.Message = "PDF-файл загружен";
            }
        }

        [RelayCommand]
        public void AddActor(Actor actor)
        {
            SelectedMusic.Actor = actor;
            SelectedMusic.ActorId = actor.Id;
            DataStore.MainViewModel.Message = $"Добавлен исполнитель: {actor.Name}";
        }

        [RelayCommand]
        public async void SaveMusic()
        {
            if (SelectedMusic.Id == 0)
            {
                await MusicService.AddMusicAsync(SelectedMusic);
                DataStore.MainViewModel.Message = "Трек добавлен";
            }
            else
            {
                await MusicService.UpdateMusicAsync(SelectedMusic);
                DataStore.MainViewModel.Message = "Трек изменен";
            }
            DataStore.NavigationService.GoBack();
        }

        public void Dispose()
        {
            // Очистка ресурсов, если необходимо
        }

        [RelayCommand]
        public async void AddFavorite()
        {
            var user = DataStore.Instance.User;
            if (!await FavoriteMusicService.IsMusicFavoriteAsync(user.Id, SelectedMusic.Id))
            {
                FavoriteMusic mus = new()
                {
                    UserId = user.Id,
                    MusicId = SelectedMusic.Id
                };
                await FavoriteMusicService.AddFavoriteAsync(mus);

                user.FavoriteMusics = await FavoriteMusicService.GetUserFavoritesAsync(user.Id);

                DataStore.MainViewModel.Message = "Избранные добавлены";
            }
            else
            {
                await FavoriteMusicService.RemoveFavoriteAsync(user.Id, SelectedMusic.Id);

                user.FavoriteMusics = await FavoriteMusicService.GetUserFavoritesAsync(user.Id);

                DataStore.MainViewModel.Message = "Избранные удалены";
            }
        }
    }
}
