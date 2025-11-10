using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models.MusicEntity;
using Infastructure.Services.Music;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using UserApp.ViewModels.Base;

namespace UserApp.ViewModels.MusicVM
{
    public partial class PlayListViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Music> _musics = new ObservableCollection<Music>();

        [ObservableProperty]
        private ObservableCollection<Music> _musicsInPlayList = new ObservableCollection<Music>();

        [ObservableProperty]
        private PlayList _currentPlayList = new PlayList();


        [ObservableProperty]
        private string _imagePath;

        public PlayListViewModel()
        {
            InitializeAsync();

        }
        
        [RelayCommand]
        void ChooseAMusic(Music music)
        {
            var page = DataStore.Instance.MusicPage;
            page.DataContext = music == null ? new MusicViewModel() : new MusicViewModel(music);
            DataStore.NavigationService.Navigate(page);
        }

        public PlayListViewModel(PlayList list)
        {
            ImagePath = list.ImagePath;
            LoadPlayListAsync(list.Id);
        }

        private async void InitializeAsync()
        {
            await LoadAllMusicsAsync();
        }

        private async Task LoadAllMusicsAsync()
        {
            var allMusics = await PlayListService.Context.Musics.ToListAsync();
            Musics = new ObservableCollection<Music>(allMusics);
            OnPropertyChanged(nameof(Musics));
        }

        public async Task LoadPlayListAsync(int playListId)
        {
            var playList = await PlayListService.GetPlayListByIdAsync(playListId);
            if (playList != null)
            {
                CurrentPlayList = playList;
                MusicsInPlayList = new ObservableCollection<Music>(playList.Musics);

                var allMusics = await PlayListService.Context.Musics.ToListAsync();
                Musics = new ObservableCollection<Music>(allMusics);

                OnPropertyChanged(nameof(MusicsInPlayList));
                OnPropertyChanged(nameof(CurrentPlayList));
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
                CurrentPlayList.ImagePath = ImagePath;
            }
        }

        [RelayCommand]
        public async Task SavePlayListAsync()
        {
            if (CurrentPlayList.Id > 0)
            {
                await PlayListService.UpdatePlayListAsync(CurrentPlayList);
                MessageBox.Show("Плейлист успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            await PlayListService.AddPlayListAsync(CurrentPlayList);
            MessageBox.Show("Плейлист успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        [RelayCommand]
        public async Task DeletePlayListAsync()
        {
            if (CurrentPlayList != null && CurrentPlayList.Id > 0)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этот плейлист?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await PlayListService.DeletePlayListAsync(CurrentPlayList.Id);
                    CurrentPlayList = new PlayList();
                    MusicsInPlayList.Clear();
                    OnPropertyChanged(nameof(CurrentPlayList));
                    MessageBox.Show("Плейлист успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        [RelayCommand]
        public async Task AddMusicToPlayListAsync(Music music)
        {
            await PlayListService.AddMusicToPlayListAsync(CurrentPlayList.Id, music.Id);
            await LoadPlayListAsync(CurrentPlayList.Id);
        }

        [RelayCommand]
        public async Task RemoveMusicFromPlayListAsync(Music music)
        {
            await PlayListService.RemoveMusicFromPlayListAsync(CurrentPlayList.Id, music.Id);
            await LoadPlayListAsync(CurrentPlayList.Id);
        }
    }
}