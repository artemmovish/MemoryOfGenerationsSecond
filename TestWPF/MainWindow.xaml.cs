using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using VersOne.Epub;

namespace TestWPF
{
    public partial class MainWindow : Window
    {
        private EpubBook _book;
        private int _currentChapterIndex = 0;
        private string _tempExtractPath;

        public MainWindow()
        {
            InitializeComponent();
            InitWebView();
        }

        private async void InitWebView()
        {
            await webView.EnsureCoreWebView2Async(null);
        }

        private void OpenEpub_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EPUB files (*.epub)|*.epub"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Очистка старой временной папки
                    if (!string.IsNullOrEmpty(_tempExtractPath) && Directory.Exists(_tempExtractPath))
                    {
                        Directory.Delete(_tempExtractPath, true);
                    }

                    _tempExtractPath = Path.Combine(Path.GetTempPath(), "epub_temp_" + Guid.NewGuid());
                    ZipFile.ExtractToDirectory(openFileDialog.FileName, _tempExtractPath);

                    _book = EpubReader.ReadBook(openFileDialog.FileName);
                    _currentChapterIndex = 0;
                    LoadChapter(_currentChapterIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки EPUB: {ex.Message}");
                }
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
                webView.CoreWebView2.Navigate(new Uri(filePath).AbsoluteUri);
            }
            else
            {
                webView.CoreWebView2.NavigateToString("<html><body>Файл главы не найден</body></html>");
            }
        }

        private void NextChapter_Click(object sender, RoutedEventArgs e)
        {
            if (_book != null && _currentChapterIndex + 1 < _book.ReadingOrder.Count)
            {
                _currentChapterIndex++;
                LoadChapter(_currentChapterIndex);
            }
        }

        private void PreviousChapter_Click(object sender, RoutedEventArgs e)
        {
            if (_book != null && _currentChapterIndex - 1 >= 0)
            {
                _currentChapterIndex--;
                LoadChapter(_currentChapterIndex);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (!string.IsNullOrEmpty(_tempExtractPath) && Directory.Exists(_tempExtractPath))
            {
                try { Directory.Delete(_tempExtractPath, true); }
                catch { /* Игнорировать ошибки удаления */ }
            }
        }
    }
}
