using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infastructure.Services
{
    public class AudioService : IDisposable
    {
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioFileReader;
        private string _currentFilePath;

        // Статический список треков
        public static List<string> TrackList { get; } = new List<string>();
        // Текущий индекс в списке треков
        public int CurrentTrackIndex { get; private set; } = -1;

        public bool IsPlaying => _waveOut?.PlaybackState == PlaybackState.Playing;
        public bool IsPaused => _waveOut?.PlaybackState == PlaybackState.Paused;
        public TimeSpan CurrentTime => _audioFileReader?.CurrentTime ?? TimeSpan.Zero;
        public TimeSpan TotalTime => _audioFileReader?.TotalTime ?? TimeSpan.Zero;
        public string CurrentFilePath => _currentFilePath;

        // Событие для уведомления об изменениях в списке треков
        public event Action TrackListChanged;

        // Добавляет трек в список
        public void AddTrack(string filePath)
        {
            if (!TrackList.Contains(filePath))
            {
                TrackList.Add(filePath);
                TrackListChanged?.Invoke();

                // Если это первый трек - автоматически загружаем его
                if (TrackList.Count == 1)
                {
                    LoadAudio(filePath);
                    CurrentTrackIndex = 0;
                }
            }
        }

        // Удаляет текущий трек из списка
        public void RemoveCurrentTrack()
        {
            if (CurrentTrackIndex < 0 || CurrentTrackIndex >= TrackList.Count)
                return;

            bool wasPlaying = IsPlaying;
            string trackToRemove = TrackList[CurrentTrackIndex];

            // Останавливаем воспроизведение если удаляем текущий трек
            if (_currentFilePath == trackToRemove)
            {
                Stop();
                DisposeCurrentAudio();
            }

            TrackList.RemoveAt(CurrentTrackIndex);
            TrackListChanged?.Invoke();

            // Корректируем индекс текущего трека
            if (TrackList.Count == 0)
            {
                CurrentTrackIndex = -1;
            }
            else if (CurrentTrackIndex >= TrackList.Count)
            {
                CurrentTrackIndex = TrackList.Count - 1;
                LoadAudio(TrackList[CurrentTrackIndex]);
                if (wasPlaying) Play();
            }
        }

        // Загружает следующий трек
        public void NextTrack()
        {
            if (TrackList.Count == 0) return;

            bool wasPlaying = IsPlaying;
            CurrentTrackIndex = (CurrentTrackIndex + 1) % TrackList.Count;
            LoadAudio(TrackList[CurrentTrackIndex]);
            if (wasPlaying) Play();
        }

        // Загружает предыдущий трек
        public void PreviousTrack()
        {
            if (TrackList.Count == 0) return;

            bool wasPlaying = IsPlaying;
            CurrentTrackIndex = (CurrentTrackIndex - 1 + TrackList.Count) % TrackList.Count;
            LoadAudio(TrackList[CurrentTrackIndex]);
            if (wasPlaying) Play();
        }

        // Загружает аудиофайл по указанному пути
        public void LoadAudio(string filePath)
        {
            DisposeCurrentAudio();

            _audioFileReader = new AudioFileReader(filePath);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFileReader);
            _currentFilePath = filePath;
            CurrentTrackIndex = TrackList.IndexOf(filePath);
        }

        // Воспроизводит загруженный файл
        public void Play()
        {
            if (_waveOut == null)
                throw new InvalidOperationException("Аудиофайл не загружен!");

            _waveOut.Play();
        }

        // Ставит на паузу
        public void Pause()
        {
            if (_waveOut == null)
                throw new InvalidOperationException("Аудиофайл не загружен!");

            _waveOut.Pause();
        }

        // Переключает между воспроизведением и паузой
        public void TogglePlayPause()
        {
            if (IsPlaying)
                Pause();
            else
                Play();
        }

        // Останавливает воспроизведение и сбрасывает позицию
        public void Stop()
        {
            if (_waveOut == null)
                throw new InvalidOperationException("Аудиофайл не загружен!");

            _waveOut.Stop();
            _audioFileReader.Position = 0;
        }

        // Перематывает на указанное время (в секундах)
        public void Seek(double seconds)
        {
            if (_audioFileReader == null)
                throw new InvalidOperationException("Аудиофайл не загружен!");

            var time = TimeSpan.FromSeconds(seconds);
            if (time > _audioFileReader.TotalTime)
                time = _audioFileReader.TotalTime;

            _audioFileReader.CurrentTime = time;
        }

        public double GetCurrentPositionPercent()
        {
            if (_audioFileReader == null)
                throw new InvalidOperationException("Аудиофайл не загружен!");

            double totalSeconds = _audioFileReader.TotalTime.TotalSeconds;
            double currentSeconds = _audioFileReader.CurrentTime.TotalSeconds;
            return (currentSeconds / totalSeconds) * 100.0;
        }

        // Освобождает ресурсы
        public void Dispose()
        {
            DisposeCurrentAudio();
        }

        private void DisposeCurrentAudio()
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _audioFileReader?.Dispose();
            _waveOut = null;
            _audioFileReader = null;
            _currentFilePath = null;
        }
    }
}