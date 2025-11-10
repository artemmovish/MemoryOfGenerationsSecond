using Entity.Models;
using Infastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UserApp.ViewModels.BookVM;
using UserApp.Views.Pages.Book;
using UserApp.Views.Pages.Music;

namespace UserApp.ViewModels.Base
{
    public class DataStore
    {
        // Приватный статический экземпляр класса
        private static DataStore _instance;

        // Приватный конструктор для предотвращения создания экземпляров извне
        private DataStore()
        {

        }

        // Публичное статическое свойство для доступа к экземпляру
        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataStore();
                }
                return _instance;
            }
        }
        public static bool IsInDesignMode
        {
            get
            {
                // Проверяем, находимся ли мы в дизайнере
                return DesignerProperties.GetIsInDesignMode(new DependencyObject());
            }
        }
        public static bool AdminMode { get; set; } = false;
        public User User { get; set; }

        public static MainViewModel MainViewModel { get; set; } = new();
        public static NavigationService NavigationService { get; set; }

        public static AudioService AudioService = new AudioService();

        #region Book
        public StartBookPage StartBookPage { get; set; } = new();
        public MainBookPage MainBookPage { get; set; } = new();
        public BookPage BookPage { get; set; } = new();
        public AuthorPage AuthorPage { get; set; } = new();

        public MainBookViewModel MainBookViewModel { get; set; }
        public AuthorViewModel AuthorViewModel { get; set; }
        #endregion

        #region Music
        public StartMusicPage StartMusicPage { get; set; } = new();
        public MainMusicPage MainMusicPage { get; set; } = new();
        public MusicPage MusicPage { get; set; } = new();

        public PlayListPage PlayListPage { get; set; } = new();
        public ArtistPage ActorPage { get; set; } = new ArtistPage();
        #endregion
    }
}
