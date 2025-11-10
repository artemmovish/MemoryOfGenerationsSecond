using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infastructure.Services;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using UserApp.ViewModels.Base;

namespace UserApp.ViewModels
{
    public partial class AudioPlayerViewModel : ObservableObject
    {
        private readonly AudioService _audioService;
        private readonly DispatcherTimer _positionUpdateTimer;

        [ObservableProperty]
        private string _currentTrackName;

        [ObservableProperty]
        private string _playPauseButtonContent = "▶";

        public AudioPlayerViewModel()
        {
            _audioService = DataStore.AudioService;
            _positionUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200) // Обновление каждые 200 мс
            };
            _positionUpdateTimer.Tick += PositionUpdateTimer_Tick;

            // Подписываемся на события изменения состояния
            _audioService.TrackListChanged += OnTrackListChanged;

            // Инициализация текущего трека
            UpdateCurrentTrackInfo();
        }

        private void PositionUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Обновляем свойство CurrentPosition, что вызовет уведомление об изменении
            OnPropertyChanged(nameof(CurrentPosition));
        }

        private void OnTrackListChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateCurrentTrackInfo();
                // Принудительное обновление состояния команд
                PlayPauseCommand.NotifyCanExecuteChanged();
                PreviousTrackCommand.NotifyCanExecuteChanged();
                NextTrackCommand.NotifyCanExecuteChanged();
                RemoveCurrentTrackCommand.NotifyCanExecuteChanged();
            });
        }

        private void UpdateCurrentTrackInfo()
        {
            CurrentTrackName = _audioService.CurrentFilePath != null
                ? Path.GetFileName(_audioService.CurrentFilePath)
                : "Нет трека";

            PlayPauseButtonContent = _audioService.IsPlaying ? "⏸" : "▶";

            // Уведомляем об изменении CurrentPosition и TrackDuration
            OnPropertyChanged(nameof(CurrentPosition));
            OnPropertyChanged(nameof(TrackDuration));

            // Управляем таймером в зависимости от состояния воспроизведения
            if (_audioService.IsPlaying)
            {
                _positionUpdateTimer.Start();
            }
            else
            {
                _positionUpdateTimer.Stop();
            }
        }

        [RelayCommand]
        private void PlayPause()
        {
            

            try
            {
                if (_audioService.IsPlaying)
                {
                    _audioService.Pause();
                    _positionUpdateTimer.Stop();
                }
                else
                {
                    OnPropertyChanged(nameof(CurrentPosition));
                    OnPropertyChanged(nameof(TrackDuration));
                    _audioService.Play();
                    _positionUpdateTimer.Start();
                }
                PlayPauseButtonContent = _audioService.IsPlaying ? "⏸" : "▶";
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Нет аудио");
            }
            catch (Exception)
            {
                _audioService.LoadAudio(AudioService.TrackList[0]);
                PlayPause();
            }
            
        }

        [RelayCommand(CanExecute = nameof(HasTracks))]
        private void PreviousTrack()
        {
            _audioService.PreviousTrack();
            UpdateCurrentTrackInfo();
        }

        [RelayCommand(CanExecute = nameof(HasTracks))]
        private void NextTrack()
        {
            _audioService.NextTrack();
            UpdateCurrentTrackInfo();
        }

        [RelayCommand(CanExecute = nameof(HasCurrentTrack))]
        private void RemoveCurrentTrack()
        {
            _audioService.RemoveCurrentTrack();
            UpdateCurrentTrackInfo();
        }

        [RelayCommand]
        private void Seek(double position)
        {
            _audioService.Seek(position);
        }

        private bool HasTracks() => AudioService.TrackList.Count > 0;
        private bool HasCurrentTrack() => _audioService.CurrentFilePath != null;

        // Для привязки к ползунку
        public double CurrentPosition
        {
            get => _audioService.CurrentTime.TotalSeconds;
            set => Seek(value);
        }

        public double TrackDuration => _audioService.TotalTime.TotalSeconds;
    }
}