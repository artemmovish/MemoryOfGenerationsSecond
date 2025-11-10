using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
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
using UserApp.ViewModels.BookVM;
using VersOne.Epub;

namespace UserApp.Views.Pages.Book
{
    /// <summary>
    /// Логика взаимодействия для BookPage.xaml
    /// </summary>
    public partial class BookPage : Page
    {
        public BookPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Управление видимостью панелей
            AdminPanel.Visibility = DataStore.AdminMode ? Visibility.Visible : Visibility.Collapsed;
            UserPanel.Visibility = DataStore.AdminMode ? Visibility.Collapsed : Visibility.Visible;

            // Инициализация WebView2
            try
            {
                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.Settings.IsStatusBarEnabled = false;

                // Подписка на изменение содержимого главы
                if (DataContext is BookViewModel viewModel)
                {
                    viewModel.PropertyChanged += ViewModel_PropertyChanged;
                    viewModel.LoadThoughts();
                    // Если контент уже загружен (например, при возврате на страницу)
                    if (!string.IsNullOrEmpty(viewModel.CurrentChapterContent))
                    {
                        webView.CoreWebView2.NavigateToString(viewModel.CurrentChapterContent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации WebView: {ex.Message}");
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BookViewModel.CurrentChapterContent) &&
                DataContext is BookViewModel viewModel)
            {
                Dispatcher.Invoke(() =>
                {
                    webView.CoreWebView2?.NavigateToString(viewModel.CurrentChapterContent);
                });
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Отписка от событий при закрытии страницы
            if (DataContext is BookViewModel viewModel)
            {
                viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
        }

        private async void BoxThoughts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is BookViewModel viewModel)
                {
                    await viewModel.AddMyThought(BoxThoughts.Text);
                }
            }  
        }
    }
}
