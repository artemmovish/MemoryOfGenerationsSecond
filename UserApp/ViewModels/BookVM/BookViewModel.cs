using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Enums;
using Entity.Models;
using Infastructure.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserApp.ViewModels.Base;
using VersOne.Epub;

namespace UserApp.ViewModels.BookVM
{
    public partial class BookViewModel : ObservableObject
    {
        [ObservableProperty]
        Book selectedBook = new();
        [ObservableProperty]
        string photoPath;
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();
        public ObservableCollection<MyThought> MyThoughts { get; set; } = new ObservableCollection<MyThought>();

        [ObservableProperty]
        User currentUser;
        [ObservableProperty]
        bool isAdmin;

        // New properties for EPUB viewing
        [ObservableProperty]
        private EpubBook _book;
        [ObservableProperty]
        private int _currentChapterIndex = 0;
        [ObservableProperty]
        private string _tempExtractPath;
        [ObservableProperty]
        private string _currentChapterContent;
        [ObservableProperty]
        private bool _isEpubLoaded;
        public BookViewModel()
        {
            
            if (DataStore.IsInDesignMode)
            {
                Author author = new()
                {
                    Name = "Антуан де Сент-Экзюпери"
                };

                SelectedBook = new Book
                {
                    Id = 1,
                    Title = "Маленький принц",
                    AuthorId = 1,
                    Author = author,
                    Description = "Маленький принц – владелец собственной планеты, ставшей домом. Персонаж полюбил Розу. Но цветок оказался капризным. В итоге мальчик оставил подопечную и от скуки отправился в космическое путешествие. Маленький принц посетил 7 планет и познакомился с их обитателями. Основные герои сюжета изменили мировоззрение юного странника.",
                    Genre = Genre.Фэнтези,
                    CoverImagePath = @"E:\Project\Учебный процесс\КПиЯП\Cursach\DataMemory\Маленький принц\Маленький принц.jfif",
                    BookFilePath = @"E:\Project\Учебный процесс\КПиЯП\Cursach\DataMemory\Маленький принц\Маленький принц.epub",
                    AudioBookPath = @"E:\Project\Учебный процесс\КПиЯП\Cursach\DataMemory\Маленький принц\Маленький принц.mp3"
                };

                CurrentUser = new User
                {
                    Id = 1,
                    AvatarPath = "/path/to/avatar.jpg",
                    Username = "TestUser",
                    Password = "TestPassword123",

                    // Инициализация коллекций
                    MyThoughts = new List<MyThought>
                        {
                            new MyThought
                            {
                                Id = 1,
                                BookId = 1,
                                Chapter = 1,
                                Text = "Это моя первая мысль о книге"
                            },
                            new MyThought
                            {
                                Id = 1,
                                BookId = 1,
                                Chapter = 1,
                                Text = "Это моя первая мысль о книге"
                            },
                            new MyThought
                            {
                                Id = 1,
                                BookId = 1,
                                Chapter = 1,
                                Text = "Это моя первая мысль о книге"
                            },
                            new MyThought
                            {
                                Id = 1,
                                BookId = 1,
                                Chapter = 1,
                                Text = "Это моя первая мысль о книге"
                            }
                        },

                    FavoriteBooks = new List<FavoriteBook>()
                };

                string defaultAuthorImage = "https://avatars.mds.yandex.net/get-entity_search/2362199/483039622/S168x252_2x";

                var author1 = new Author
                {
                    Id = 1,
                    Name = "Лев Толстой",
                    Biography = "Русский писатель, мыслитель, философ и публицист.",
                    BirthDate = new DateTime(1828, 9, 9),
                    DeathDate = new DateTime(1910, 11, 20),
                    PhotoPath = defaultAuthorImage,
                    InterestingFacts = "Автор романов 'Война и мир' и 'Анна Каренина'."
                };

                var author2 = new Author
                {
                    Id = 2,
                    Name = "Фёдор Достоевский",
                    Biography = "Русский писатель, мыслитель, философ и публицист.",
                    BirthDate = new DateTime(1821, 11, 11),
                    DeathDate = new DateTime(1881, 2, 9),
                    PhotoPath = defaultAuthorImage,
                    InterestingFacts = "Автор 'Преступления и наказания'."
                };

                var author3 = new Author
                {
                    Id = 3,
                    Name = "Стивен Кинг",
                    Biography = "Американский писатель, работающий в разнообразных жанрах.",
                    BirthDate = new DateTime(1947, 9, 21),
                    PhotoPath = defaultAuthorImage,
                    InterestingFacts = "Король ужасов."
                };

                Authors.Add(author1);
                Authors.Add(author2);
                Authors.Add(author3);

                return;
            }

            LoadData();
        }

        public BookViewModel(Book book)
        {
            IsAdmin = DataStore.AdminMode;
            SelectedBook = book;
            PhotoPath = book.CoverImagePath;
            LoadData();

            if (!string.IsNullOrEmpty(book.BookFilePath))
            {
                LoadEpub(book.BookFilePath);
            }
        }

        [RelayCommand]
        void LoadAudio()
        {
            if (!string.IsNullOrEmpty(SelectedBook.AudioBookPath))
            {
                DataStore.AudioService.AddTrack(SelectedBook.AudioBookPath);
                DataStore.AudioService.Play();
            }

            OnPropertyChanged(nameof(AudioService.TrackList));
        }
        private void LoadEpub(string filePath)
        {
            try
            {
                // Clean up old temp directory
                if (!string.IsNullOrEmpty(_tempExtractPath) && Directory.Exists(_tempExtractPath))
                {
                    Directory.Delete(_tempExtractPath, true);
                }

                _tempExtractPath = Path.Combine(Path.GetTempPath(), "epub_temp_" + Guid.NewGuid());
                ZipFile.ExtractToDirectory(filePath, _tempExtractPath);

                _book = EpubReader.ReadBook(filePath);
                _currentChapterIndex = 0;
                IsEpubLoaded = true;
                LoadChapter(_currentChapterIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки EPUB: {ex.Message}");
                IsEpubLoaded = false;
            }
        }

        private void LoadChapter(int index)
        {
            if (_book == null || _book.ReadingOrder.Count == 0 || index < 0 || index >= _book.ReadingOrder.Count)
                return;

            var chapter = _book.ReadingOrder[index];
            string filePath = Path.Combine(_tempExtractPath, chapter.FilePath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(filePath))
            {
                CurrentChapterContent = File.ReadAllText(filePath);
            }
            else
            {
                CurrentChapterContent = "<html><body>Файл главы не найден</body></html>";
            }
        }

        [RelayCommand]
        private void NextChapter()
        {
            if (_book != null && _currentChapterIndex + 1 < _book.ReadingOrder.Count)
            {
                _currentChapterIndex++;
                LoadChapter(_currentChapterIndex);
                LoadThoughts();
            }
        }

        [RelayCommand]
        private async Task PreviousChapter()
        {
            if (_book != null && _currentChapterIndex - 1 >= 0)
            {
                _currentChapterIndex--;
                LoadChapter(_currentChapterIndex);
                LoadThoughts();
            }
        }

        public void LoadThoughts()
        {
            var list = new ObservableCollection<MyThought>(DataStore.Instance.User.MyThoughts
                    .Where(t => t.Chapter == CurrentChapterIndex && t.BookId == this.SelectedBook.Id));

            MyThoughts.Clear();
            foreach (var item in list)
            {
                MyThoughts.Add(item);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!string.IsNullOrEmpty(_tempExtractPath) && Directory.Exists(_tempExtractPath))
            {
                try { Directory.Delete(_tempExtractPath, true); }
                catch { /* Ignore cleanup errors */ }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task LoadData()
        {
            Authors = new ObservableCollection<Author>(await AuthorService.GetAllAuthorsAsync());
        }

        
        private void LoadUser()
        {
            CurrentUser = DataStore.Instance.User;
        }

        public async Task AddMyThought(string str)
        {
            var user = DataStore.Instance.User;

            MyThought myThought = new()
            {
                UserId = DataStore.Instance.User.Id,
                BookId = SelectedBook.Id,
                Chapter = CurrentChapterIndex,
                Text = str,
            };

            await MyThoughtService.AddThoughtAsync(myThought);

            user.MyThoughts = await MyThoughtService.GetUserThoughtsAsync(user.Id);

            MyThoughts.Add(myThought);
        }

        [RelayCommand]
        public async void AddFavorite()
        {
            var user = DataStore.Instance.User;
            if (!await FavoriteBookService.IsBookFavoriteAsync(user.Id, SelectedBook.Id))
            {
                FavoriteBook book = new()
                {
                    UserId = user.Id,
                    BookId = SelectedBook.Id
                };
                await FavoriteBookService.AddFavoriteAsync(book);

                DataStore.MainViewModel.Message = "Избранные добавлены";
            }
            else
            {
                await FavoriteBookService.RemoveFavoriteAsync(user.Id, SelectedBook.Id);
                DataStore.MainViewModel.Message = "Избранные удалены";
            }
        }

        [RelayCommand]
        public async void DeleteBook()
        {
            try
            {
                await BookService.DeleteBookAsync(SelectedBook.Id);
                DataStore.MainViewModel.Message = "Книга удалена";
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
                    SelectedBook.CoverImagePath = selectedFilePath;
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
        public void AddAudio()
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

                // Проверяем расширение файла
                string extension = Path.GetExtension(selectedFilePath).ToLower();
                if (extension == ".mp3" || extension == ".wav" || extension == ".ogg" || extension == ".flac")
                {
                    SelectedBook.AudioBookPath = selectedFilePath;
                    DataStore.MainViewModel.Message = "Аудио файл загружен";
                }
                else
                {
                    MessageBox.Show("Выберите файл аудио (MP3, WAV, OGG, FLAC).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        [RelayCommand]
        public void AddBook()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл книги",
                Filter = "EPUB файлы (*.epub)|*.epub|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Проверяем расширение файла
                string extension = Path.GetExtension(selectedFilePath).ToLower();
                if (extension == ".epub")
                {
                    SelectedBook.BookFilePath = selectedFilePath;
                    DataStore.MainViewModel.Message = "Файл книги загружен";
                }
                else
                {
                    MessageBox.Show("Выберите файл в формате EPUB.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        [RelayCommand]
        public void AddAuthor(Author author)
        {
            SelectedBook.Author = author;
            SelectedBook.AuthorId = author.Id;
            DataStore.MainViewModel.Message = $"Добавлен автор: {author.Name}";
        }

        [RelayCommand]
        public async void SaveBook()
        {
            if (SelectedBook.Id == 0)
            {
                await BookService.AddBookAsync(SelectedBook);
                DataStore.MainViewModel.Message = "Книга добавлена";
                DataStore.NavigationService.GoBack();
                return;
            }
            await BookService.UpdateBookAsync(SelectedBook);
            DataStore.MainViewModel.Message = "Книга изменена";

            DataStore.NavigationService.GoBack();
        }
    }
}
