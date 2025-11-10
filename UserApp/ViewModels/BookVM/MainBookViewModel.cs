using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entity.Models;
using Infastructure.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel; // Не забудьте using!
using System.Windows;
using UserApp.ViewModels.Base;
using Entity.Enums;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Navigation; // DependencyObject

namespace UserApp.ViewModels.BookVM
{
    

    public partial class MainBookViewModel : ObservableObject
    {
        public ObservableCollection<Book> AllBooks { get; set; }

        public ObservableCollection<Book> AllBooksSorted { get; set; }

        public ObservableCollection<Book> SortBooks { get; set; }

        public ObservableCollection<Book> SortBooksAudio { get; set; }

        public ObservableCollection<Author> Authors { get; set; }
        public ObservableCollection<Author> SortAuthors { get; set; }

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        bool isAdmin;
        public MainBookViewModel()
        {
            IsAdmin = DataStore.AdminMode;
            if (DataStore.IsInDesignMode)
            {
                return;
            }
            LoadData();
        }

        private async void LoadData()
        {
            AllBooks = new ObservableCollection<Book>(await BookService.GetAllBooksAsync());

            AllBooksSorted = new ObservableCollection<Book>(AllBooks);

            

            Authors = new ObservableCollection<Author>(await AuthorService.GetAllAuthorsAsync());

            SortAuthors = new ObservableCollection<Author>(Authors);

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    FilterAndSortBooks();
                }
            };

            LoadSortBooks();
        }

        public void LoadSortBooks()
        {
            SortBooks = new ObservableCollection<Book>(
                AllBooksSorted);

            SortBooksAudio = new ObservableCollection<Book>(
                AllBooksSorted.Where(t => !string.IsNullOrEmpty(t.AudioBookPath)));
        }

        [RelayCommand]
        void FilterByGenre(Genre genre)
        {
            if (AllBooks == null || !AllBooks.Any())
                return;

            var filtered = genre == Genre.Другое
                ? AllBooks.ToList() // Если выбран "Другое", показываем все книги
                : AllBooks.Where(book => book.Genre == genre).ToList();

            AllBooksSorted.Clear();
            foreach (var book in filtered)
            {
                AllBooksSorted.Add(book);
            }

            LoadSortBooks();

            OnPropertyChanged(nameof(SortBooks));
            OnPropertyChanged(nameof(SortBooksAudio));
        }
        public void FilterAndSortBooks()
        {
            if (AllBooks == null || !AllBooks.Any())
                return;

            var filteredBooks = string.IsNullOrWhiteSpace(SearchText)
                ? AllBooks.ToList()
                : AllBooks.Where(book =>
                    book.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            var sortedBooks = filteredBooks.OrderBy(book => book.Title).ToList();

            SortBooks.Clear();
            foreach (var book in sortedBooks)
            {
                SortBooks.Add(book);
            }

            var filteredAuthors = string.IsNullOrWhiteSpace(SearchText)
                ? Authors.ToList()
                : Authors.Where(author =>
                    author.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            SortAuthors.Clear();
            foreach (var author in filteredAuthors)
            {
                SortAuthors.Add(author);
            }

            SortBooksAudio = new ObservableCollection<Book>(
                SortBooks.Where(t => !string.IsNullOrEmpty(t.AudioBookPath)));

            OnPropertyChanged(nameof(SortBooks));
            OnPropertyChanged(nameof(SortBooksAudio));
            OnPropertyChanged(nameof(SortAuthors));
        }




        [RelayCommand]
        void ChooseAnAuthor(Author author)
        {
            var page = DataStore.Instance.AuthorPage;

            page.DataContext = author == null ? new AuthorViewModel() : new AuthorViewModel(author);

            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void AddAnAuthor()
        {
            var page = DataStore.Instance.AuthorPage;

            page.DataContext = new AuthorViewModel();

            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void ChooseAnBook(Book book)
        {
            var page = DataStore.Instance.BookPage;

            page.DataContext = book == null ? new BookViewModel() : new BookViewModel(book);

            DataStore.NavigationService.Navigate(page);
        }
        [RelayCommand]
        void AddAnBook()
        {
            var page = DataStore.Instance.BookPage;

            page.DataContext = new BookViewModel();

            DataStore.NavigationService.Navigate(page);
        }
    }
}
