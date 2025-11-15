using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entity.Models.MusicEntity;
using Infastructure.Services.Music;

using System.Collections.ObjectModel;
using UserApp.ViewModels.Base;

namespace UserApp.ViewModels.MusicVM
{
    public partial class MainMusicViewModel : ObservableObject
    {
        public ObservableCollection<Music> Musics { get; set; }
        public ObservableCollection<Music> FilteredMusics { get; set; }
        public ObservableCollection<Actor> Actors { get; set; }

        public ObservableCollection<Actor> FilteredActors { get; set; }

        public ObservableCollection<PlayList> PlayLists { get; set; }

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        bool isAdmin;

        public MainMusicViewModel()
        {
            IsAdmin = DataStore.AdminMode;
            LoadData();
        }

        public async void LoadData()
        {
            Musics = new ObservableCollection<Music>(await MusicService.GetAllMusicsAsync());
            FilteredMusics = new ObservableCollection<Music>(Musics);
            Actors = new ObservableCollection<Actor>(await ActorService.GetAllActorsAsync());
            FilteredActors = new(Actors);
            PlayLists = new ObservableCollection<PlayList>(await PlayListService.GetAllPlayListsAsync());

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    FilterAndSortMusics();
                }
            };
        }

        [RelayCommand]
        void FilterByGenre(string genre)
        {
            if (Musics == null || !Musics.Any())
                return;

            var filtered = string.IsNullOrWhiteSpace(genre)
                ? Musics.ToList() // Если жанр не выбран, показываем всю музыку
                : Musics.Where(music => music.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

            FilteredMusics.Clear();
            foreach (var music in filtered)
            {
                FilteredMusics.Add(music);
            }

            OnPropertyChanged(nameof(FilteredMusics));
        }

        public void FilterAndSortMusics()
        {
            if (Musics == null || !Musics.Any())
                return;

            // Фильтрация по SearchText (если не пусто)
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Musics.ToList() // Если строка поиска пуста, берем все треки
                : Musics.Where(music =>
                    music.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    music.Actor?.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true).ToList();

            // Сортировка по названию (Name)
            var sortedMusics = filtered.OrderBy(music => music.Name).ToList();

            // Фильтрация по SearchText (если не пусто)
            var filteredA = string.IsNullOrWhiteSpace(SearchText)
                ? Actors.ToList() // Если строка поиска пуста, берем все треки
                : Actors.Where(music =>
                    music.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            // Сортировка по названию (Name)
            var sortedA = filteredA.OrderBy(music => music.Name).ToList();

            // Обновляем FilteredMusics
            FilteredMusics.Clear();
            FilteredActors.Clear();
            foreach (var music in sortedMusics)
            {
                FilteredMusics.Add(music);
            }
            foreach (var item in sortedA)
            {
                FilteredActors.Add(item);
            }

            // Уведомляем UI об изменении коллекции
            OnPropertyChanged(nameof(FilteredMusics));
        }

        [RelayCommand]
        void ChooseAnActor(Actor actor)
        {
            var page = DataStore.Instance.ActorPage;
            page.DataContext = actor == null ? new ActorViewModel() : new ActorViewModel(actor);
            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void AddAnActor()
        {
            var page = DataStore.Instance.ActorPage;
            page.DataContext = new ActorViewModel();
            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void ChooseAMusic(Music music)
        {
            var page = DataStore.Instance.MusicPage;
            page.DataContext = music == null ? new MusicViewModel() : new MusicViewModel(music);
            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void AddAMusic()
        {
            var page = DataStore.Instance.MusicPage;
            page.DataContext = new MusicViewModel();
            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void AddPlayList()
        {
            var page = DataStore.Instance.PlayListPage;
            page.DataContext = new PlayListViewModel();
            DataStore.NavigationService.Navigate(page);
        }

        [RelayCommand]
        void ChoosePlayList(PlayList list)
        {
            var page = DataStore.Instance.PlayListPage;
            page.DataContext = new PlayListViewModel(list);
            DataStore.NavigationService.Navigate(page);
        }
    }
}