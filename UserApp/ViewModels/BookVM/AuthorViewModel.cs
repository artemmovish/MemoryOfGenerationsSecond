using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Enums;
using Entity.Models;
using Infastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using UserApp.ViewModels.Base;
using static System.Net.WebRequestMethods;

namespace UserApp.ViewModels.BookVM
{
    public partial class AuthorViewModel : ObservableObject
    {
        [ObservableProperty]
        Author selectedAuthor = new();

        [ObservableProperty]
        string photoPath;

        [ObservableProperty]
        bool isAdmin;
        public AuthorViewModel(Author author)
        {
            IsAdmin = DataStore.AdminMode;
            SelectedAuthor = author;
            PhotoPath = SelectedAuthor.PhotoPath;
        }
        public AuthorViewModel()
        {
            IsAdmin = DataStore.AdminMode;
            if (DataStore.IsInDesignMode)
            {
                IsAdmin = true;
                SelectedAuthor = new Author
                {
                    Id = 1,
                    Name = "Лев Толстой",
                    Biography = "Русский писатель, мыслитель, философ и публицист.",
                    LifeDate = "1900-1956",
                    BirthDate = new DateTime(1828, 9, 9),
                    DeathDate = new DateTime(1910, 11, 20),
                    PhotoPath = "https://avatars.mds.yandex.net/get-entity_search/10920629/1132268124/S600xU_2x",
                    InterestingFacts = "Автор романов 'Война и мир' и 'Анна Каренина'."
                };

                string defaultBookImage = "https://avatars.mds.yandex.net/get-entity_search/2362199/483039622/S168x252_2x";

                var book1 = new Book
                {
                    Id = 1,
                    Title = "Война и мир",
                    AuthorId = 1,
                    Author = SelectedAuthor,
                    Description = "Роман-эпопея, описывающий русское общество в эпоху войн против Наполеона.",
                    Genre = Genre.Роман,
                    CoverImagePath = defaultBookImage,
                    BookFilePath = @"E:\Project\Учебный процесс\КПиЯП\Cursach\DataMemory\Маленький принц\Маленький принц.epub",
                    AudioBookPath = @"E:\Project\Учебный процесс\КПиЯП\Cursach\DataMemory\Маленький принц\Маленький принц.mp3"
                };

                var book2 = new Book
                {
                    Id = 2,
                    Title = "Анна Каренина",
                    AuthorId = 1,
                    Author = SelectedAuthor,
                    Description = "Трагическая история любви замужней женщины.",
                    Genre = Genre.Роман,
                    CoverImagePath = defaultBookImage,
                    BookFilePath = "anna_karenina.pdf"
                };

                SelectedAuthor.Books.Add(book1);
                SelectedAuthor.Books.Add(book2);

                PhotoPath = SelectedAuthor.PhotoPath;
            }
        }

        [RelayCommand]
        public async void DeleteAuthor()
        {
            try
            {
                await AuthorService.DeleteAuthorWithBooksAsync(SelectedAuthor.Id);
                DataStore.MainViewModel.Message = "Автор удален";
            }
            catch (Exception)
            {
                MessageBox.Show("Автор не найден!");
            }
            DataStore.NavigationService.GoBack();
        }

        [RelayCommand]
        public void AddPhoto()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение автора",
                Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Проверяем расширение файла (опционально)
                string extension = Path.GetExtension(selectedFilePath).ToLower();
                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp")
                {
                    SelectedAuthor.PhotoPath = selectedFilePath;
                    PhotoPath = selectedFilePath;
                    DataStore.MainViewModel.Message = "Фото загружено";
                }
                else
                {
                    MessageBox.Show("Выберите файл изображения (PNG, JPG, JPEG, BMP).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
        }

        [RelayCommand] 
        public async void SaveAuthor()
        {
            if(SelectedAuthor.Id == 0)
            {
                await AuthorService.AddAuthorAsync(SelectedAuthor);
                DataStore.MainViewModel.Message = "Автор добавлен";
                return;
            }
            await AuthorService.UpdateAuthorAsync(SelectedAuthor);
            DataStore.MainViewModel.Message = "Автор изменен";

            DataStore.NavigationService.GoBack();
        }
    }
}
