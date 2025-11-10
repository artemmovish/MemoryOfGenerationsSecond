using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models.MusicEntity;
using Infastructure.Services;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using UserApp.ViewModels.Base;
using Infastructure.Services.Music;

namespace UserApp.ViewModels.MusicVM
{
    public partial class ActorViewModel : ObservableObject
    {
        [ObservableProperty]
        Actor selectedActor = new();

        [ObservableProperty]
        string imagePath;

        [ObservableProperty]
        bool isAdmin;

        public ActorViewModel(Actor actor)
        {
            IsAdmin = DataStore.AdminMode;
            SelectedActor = actor;
            ImagePath = SelectedActor.ImagePath;
        }

        public ActorViewModel()
        {
            IsAdmin = DataStore.AdminMode;
            if (DataStore.IsInDesignMode)
            {
                IsAdmin = true;
                SelectedActor = new Actor
                {
                    Id = 1,
                    Name = "The Weeknd",
                    Description = "Канадский певец, автор песен и музыкальный продюсер.",
                    ImagePath = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/The_Weeknd_-_Blinding_Lights_%28single%29.png/800px-The_Weeknd_-_Blinding_Lights_%28single%29.png"
                };

                var music1 = new Music
                {
                    Id = 1,
                    Name = "Blinding Lights",
                    ActorId = 1,
                    Actor = SelectedActor,
                    MusicPath = "/music/weeknd_blinding_lights.mp3",
                    ImagePath = "https://upload.wikimedia.org/wikipedia/en/0/05/The_Weeknd_-_After_Hours.png",
                    TextPath = "/lyrics/weeknd_blinding_lights.txt"
                };

                var music2 = new Music
                {
                    Id = 2,
                    Name = "Save Your Tears",
                    ActorId = 1,
                    Actor = SelectedActor,
                    MusicPath = "/music/weeknd_save_your_tears.mp3",
                    ImagePath = "https://upload.wikimedia.org/wikipedia/en/0/05/The_Weeknd_-_After_Hours.png",
                    TextPath = "/lyrics/weeknd_save_your_tears.txt"
                };

                SelectedActor.Musics.Add(music1);
                SelectedActor.Musics.Add(music2);

                ImagePath = SelectedActor.ImagePath;
            }
        }

        [RelayCommand]
        public async void DeleteActor()
        {
            try
            {
                await ActorService.DeleteActorAsync(SelectedActor.Id);
                DataStore.MainViewModel.Message = "Исполнитель удален";
            }
            catch (Exception)
            {
                MessageBox.Show("Исполнитель не найден!");
            }
            DataStore.NavigationService.GoBack();
        }

        [RelayCommand]
        public void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение исполнителя",
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
                    SelectedActor.ImagePath = selectedFilePath;
                    ImagePath = selectedFilePath;
                    DataStore.MainViewModel.Message = "Изображение загружено";
                }
                else
                {
                    MessageBox.Show("Выберите файл изображения (PNG, JPG, JPEG, BMP).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        [RelayCommand]
        public async void SaveActor()
        {
            if (SelectedActor.Id == 0)
            {
                await ActorService.AddActorAsync(SelectedActor);
                DataStore.MainViewModel.Message = "Исполнитель добавлен";
                return;
            }
            await ActorService.UpdateActorAsync(SelectedActor);
            DataStore.MainViewModel.Message = "Исполнитель изменен";

            DataStore.NavigationService.GoBack();
        }
    }
}